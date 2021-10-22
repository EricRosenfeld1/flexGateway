using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using flexGateway.Interface;
using flexGateway.Common.Device;
using Microsoft.Extensions.Logging;
using System;

namespace flexGateway.Common.Node
{
    public class NodeSynchronizationService : BackgroundService
    {
        private readonly IDeviceManager _deviceManager;
        private readonly ILogger<NodeSynchronizationService> _logger;
        private int _pollingThreshold = 500;

        public bool IsRunning { get; private set; } = false;

        public NodeSynchronizationService(ILogger<NodeSynchronizationService> logger, IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async() =>
        {
            Stopwatch sw = new Stopwatch();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_deviceManager.Source == null || _deviceManager.Publishers.Count == 0)
                    {
                        await StopAsync(new CancellationToken());
                        break;
                    }

                    sw.Restart();

                    // 1. get changed source nodes
                    if (!_deviceManager.Source.IsConnected && _deviceManager.Source.LastException != null)
                    {
                        await StopAsync(new CancellationToken());
                        break;
                    }

                    List<INode> dirtySourceNodes = new();
                    try
                    {
                        dirtySourceNodes = await _deviceManager.Source.GetDirtyNodesAsync();
                    } 
                    catch (Exception ex)
                    {
                        _deviceManager.Source.LastException = ex.InnerException;
                        _deviceManager.Source.IsConnected = false;
                        await StopAsync(new CancellationToken());
                        break;
                    }

                    Dictionary<Guid, object> sourceChanges = dirtySourceNodes.ToDictionary(x => x.Guid, x => x.Value);


                    // 2. get changed publisher nodes
                    var pullTaskBindings = new Dictionary<IDevice, Task<List<INode>>>();

                    foreach (var publisher in _deviceManager.Publishers)
                        if (publisher.IsConnected && publisher.LastException == null)
                        {
                            var t = publisher.GetDirtyNodesAsync();
                            pullTaskBindings.Add(publisher, t);
                        }

                    List<INode>[] pullResults = null;
                    var pullTask = Task.WhenAll(pullTaskBindings.Values);
                    try
                    {
                       pullResults = await pullTask;
                    } 
                    catch (Exception)
                    {
                        foreach(var task in pullTaskBindings)
                            if (task.Value.IsFaulted)
                            {
                                task.Key.LastException = task.Value.Exception.InnerException;
                                task.Key.IsConnected = false;
                            }
                    }

                    if(pullResults == null)
                    {
                        await StopAsync(new CancellationToken());
                        break;
                    }

                    // 3. combine publisher and source nodes, ignore nodes where source already changed
                    Dictionary<Guid, object> publisherChanges = new Dictionary<Guid, object>(sourceChanges);
                    foreach (var result in pullResults)
                        foreach (var change in result)
                            publisherChanges.TryAdd(change.ParentGuid, change.Value);

                    // 4. push combined nodes to publishers
                    var pushTaskBindings = new Dictionary<IDevice, Task>();
                    foreach (var publisher in _deviceManager.Publishers)
                        if (publisher.IsConnected && publisher.LastException == null)
                            pushTaskBindings.Add(publisher, publisher.PushParentChangesAsync(publisherChanges));
                

                    // 5. remove source nodes form dict so we dont double update source nodes
                    foreach (var change in sourceChanges)
                        publisherChanges.Remove(change.Key);

                    // 6. push changes to source
                    pushTaskBindings.Add(_deviceManager.Source, _deviceManager.Source.PushChangesAsync(publisherChanges));

                    // 7. update all
                    var pushTask = Task.WhenAll(pushTaskBindings.Values);
                    try
                    {
                        await pushTask;
                    } 
                    catch (Exception)
                    {
                        foreach(var task in pushTaskBindings)
                            if (task.Value.IsFaulted)
                            {
                                task.Key.LastException = task.Value.Exception.InnerException;
                                task.Key.IsConnected = false;
                            }
                    }

                    // check if polling threshold is reached
                    int elapsedMs;
                    int.TryParse(sw.Elapsed.TotalMilliseconds.ToString(), out elapsedMs);
                    int waitMs = _pollingThreshold - elapsedMs;
                    if (waitMs > 0)
                        await Task.Delay(waitMs);
                } 
                catch (Exception ex)
                {
                    this.IsRunning = false;
                    Debug.WriteLine(ex.Message);
                }              
            }
        });        

    }
}
