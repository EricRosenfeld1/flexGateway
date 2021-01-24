using Microsoft.Extensions.Hosting;
using flexGateway.Common.Adapter;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace flexGateway.Common.MachineNode
{
    public class NodeManager : BackgroundService
    {
        private AdapterManager adapterManager;
        private int pollingIntervalMs = 300;

        public NodeManager(AdapterManager adapterManager)
        {
            this.adapterManager = adapterManager;
        }
   
        #region Synchronization 

        private async Task Update(CancellationToken token)
        {
            Stopwatch sw = new Stopwatch();

            while (!token.IsCancellationRequested)
            {
                sw.Restart();

                List<INode> dirtySourceNodes = await adapterManager.Source.GetDirtyNodesAsync();
                Dictionary<INode, object> sourceChanges = dirtySourceNodes.ToDictionary(x => x, x => x.Value);

                // add changes to dictionary, if source is already dirty ignore it
                foreach(var publisher in adapterManager.Publishers)
                    foreach(var change in await publisher.GetDirtyNodesAsync())
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
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async() =>
        {
            await Update(stoppingToken);
        });
        
        #endregion
    }
}
