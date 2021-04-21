using Microsoft.Extensions.Hosting;
using flexGateway.Common.Adapter;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using flexGateway.Interface;
using Microsoft.Extensions.Logging;

namespace flexGateway.Common.MachineNode
{
    public class NodeSynchronizationService : BackgroundService
    {
        private IAdapterManager adapterManager;
        private int pollingIntervalMs = 500;
        private ILogger<NodeSynchronizationService> logger;

        public bool IsRunning { get; private set; } = false;

        public NodeSynchronizationService(ILogger<NodeSynchronizationService> logger, IAdapterManager adapterManager)
        {
            this.adapterManager = adapterManager;
            this.logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            IsRunning = true;
            logger.LogInformation("service start");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            IsRunning = false;
            logger.LogInformation("service stop");
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async() =>
        {
            Stopwatch sw = new Stopwatch();

            while (!stoppingToken.IsCancellationRequested)
            {
                if (adapterManager.Source == null || adapterManager.Publishers.Count == 0)
                    await StopAsync(new CancellationToken());

                sw.Restart();

                List<INode> dirtySourceNodes = await adapterManager.Source.GetDirtyNodesAsync();
                Dictionary<INode, object> sourceChanges = dirtySourceNodes.ToDictionary(x => x, x => x.Value);

                // add changes to dictionary, if source is already dirty ignore it
                foreach (var publisher in adapterManager.Publishers)
                    foreach (var change in await publisher.GetDirtyNodesAsync())
                        sourceChanges.TryAdd(change.ParentNode, change.Value);

                // write changes to source
                await adapterManager.Source.PushChangesAsync(sourceChanges);

                // update all services
                foreach (var output in adapterManager.Publishers)
                    await output.PushChangesAsync(sourceChanges);

                // check if polling threshold is reached
                int elapsedMs;
                int.TryParse(sw.Elapsed.TotalMilliseconds.ToString(), out elapsedMs);
                int waitMs = pollingIntervalMs - elapsedMs;
                if (waitMs > 0)
                    await Task.Delay(waitMs);
            }
        });        

        
    }
}
