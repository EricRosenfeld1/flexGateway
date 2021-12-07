using flexGateway.Common.Adapters;
using flexGateway.Plugin;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace flexGateway.Common.Nodes
{
    public class NodeSynchronizationService : BackgroundService
    {
        private readonly IAdapterManager _adapterManager;
        private readonly ILogger<NodeSynchronizationService> _logger;
        private int _pollingThreshold = 500;

        public bool IsRunning { get; private set; } = false;

        public NodeSynchronizationService(ILogger<NodeSynchronizationService> logger, IAdapterManager adapterManager)
        {
            _adapterManager = adapterManager;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (_adapterManager.Adapters.Count == 0)
            {
                _logger.LogInformation("No registered adapters. Service not started.");
                return Task.CompletedTask;
            }

            IsRunning = true;
            _logger.LogInformation("Node synchronization service started.");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            IsRunning = false;
            _logger.LogInformation("Node synchronization service stoped.");
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
        {
            Stopwatch sw = new Stopwatch();
            try
            {
                 while (!stoppingToken.IsCancellationRequested)
                {
                    var source = _adapterManager.Adapters.SingleOrDefault(x => x.IsSource & x.IsConnected);
                    if (source == null)
                    {
                        IsRunning = false;
                        _logger.LogWarning("No vaild source found. Stopping service..");
                        return;
                    }

                    var publishers = _adapterManager.Adapters.Where(x => x.IsSource == false & x.IsConnected);
                    if (publishers.Count() == 0)
                    {
                        IsRunning = false;
                        _logger.LogWarning("No publishers found. Stopping service..");
                        return;
                    }


                    // 1. get changed nodes from source
                    var sourceChanges = new HashSet<NodeChange>(new NodeChangeEqualityComparer());
                    try
                    {
                        foreach (var node in await source.GetDirtyNodesAsync())
                            sourceChanges.Add(new NodeChange(node.Guid, node.Value, node.DataType));
                    }
                    catch (Exception ex)
                    {
                        source.LastException = ex.InnerException;
                        await source.DisconnectAsync();
                        throw new Exception($"Error while getting dirty nodes from source: '{source.Name}'");
                    }

                    // 2. get changed publisher nodes
                    var pullTaskBindings = new Dictionary<Adapter, Task<List<Node>>>();

                    foreach (var publisher in publishers)
                        if (publisher.IsConnected && publisher.LastException == null)
                        {
                            var t = publisher.GetDirtyNodesAsync();
                            pullTaskBindings.Add(publisher, t);
                        }

                    List<Node>[] pullResults = null;
                    var pullTask = Task.WhenAll(pullTaskBindings.Values);
                    try
                    {
                        pullResults = await pullTask;
                    }
                    catch (Exception)
                    {
                        foreach (var task in pullTaskBindings)
                            if (task.Value.IsFaulted)
                            {
                                task.Key.LastException = task.Value.Exception.InnerException;
                                await task.Key.DisconnectAsync();
                            }
                    }

                    if (pullResults == null)
                    {
                        await StopAsync(new CancellationToken());
                        break;
                    }

                    // 3. combine publisher and source nodes, ignore nodes where source already changed
                    var combinedChanges = new HashSet<NodeChange>(sourceChanges, new NodeChangeEqualityComparer());

                    foreach (var result in pullResults)
                        foreach (var change in result)
                            if (change.ParentGuid != Guid.Empty) // if no binding dont add it
                                combinedChanges.Add(new NodeChange(change.ParentGuid, change.Value, change.DataType));

                    // 4. push combined nodes to publishers
                    var pushTaskBindings = new Dictionary<Adapter, Task>();
                    foreach (var publisher in publishers)
                        if (publisher.IsConnected && publisher.LastException == null)
                            pushTaskBindings.Add(publisher, publisher.PushParentChangesAsync(combinedChanges));

                    // 5. remove source nodes form dict so we dont double update source nodes
                    foreach (var change in sourceChanges)
                        combinedChanges.Remove(change);

                    // 6. push changes to source
                    pushTaskBindings.Add(source, source.PushChangesAsync(combinedChanges));

                    // 7. update all
                    var pushTask = Task.WhenAll(pushTaskBindings.Values);
                    try
                    {
                        await pushTask;
                    }
                    catch (Exception)
                    {
                        foreach (var task in pushTaskBindings)
                            if (task.Value.IsFaulted)
                            {
                                task.Key.LastException = task.Value.Exception.InnerException;
                                await task.Key.DisconnectAsync();
                            }
                    }

                    // check if polling threshold is reached
                    int elapsedMs;
                    int.TryParse(sw.Elapsed.TotalMilliseconds.ToString(), out elapsedMs);
                    int waitMs = _pollingThreshold - elapsedMs;
                    if (waitMs > 0)
                        await Task.Delay(waitMs);
                }

                IsRunning = false;
            }
            catch (Exception ex)
            {
                this.IsRunning = false;
                _logger.LogError(ex.Message);
            }
        });
    }
}
