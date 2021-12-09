using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var p = new Program();
            await p.Update();

            Console.ReadKey();
        }

        private async Task Update()
        {
            var source = new Adapter() { Guid = Guid.NewGuid() };
            List<Adapter> publishers = new List<Adapter>();

            publishers.Add(new Adapter() { Guid = Guid.NewGuid() });
            publishers.Add(new Adapter() { Guid = Guid.NewGuid() });
            //publishers.Add(new Adapter() { Guid = Guid.NewGuid() });

            List<Node> sourceNodes = new List<Node>();
            sourceNodes.Add(new Node() { Guid = Guid.NewGuid(), Value = 1, IsDirty = true });
            sourceNodes.Add(new Node() { Guid = Guid.NewGuid(), Value = 2 });
            sourceNodes.Add(new Node() { Guid = Guid.NewGuid(), Value = 3 });

            List<Node> publisherNodesB = new List<Node>();
            publisherNodesB.Add(new Node() { Guid = Guid.NewGuid(), Binding = sourceNodes[0].Guid, Value = 1 });
            publisherNodesB.Add(new Node() { Guid = Guid.NewGuid(), Binding = sourceNodes[1].Guid, Value = 1, IsDirty = true});
            publisherNodesB.Add(new Node() { Guid = Guid.NewGuid(), Binding = sourceNodes[2].Guid, Value = 1, IsDirty = true });

            publisherNodesB.Add(new Node() { Guid = Guid.NewGuid(), Binding = sourceNodes[0].Guid, Value = 2, IsDirty = true });
            publisherNodesB.Add(new Node() { Guid = Guid.NewGuid(), Binding = sourceNodes[1].Guid, Value = 3, IsDirty = true });
            publisherNodesB.Add(new Node() { Guid = Guid.NewGuid(), Binding = sourceNodes[2].Guid, Value = 2 });

            //publisherNodesB.Add(new Node() { Guid = Guid.NewGuid(), Binding = sourceNodes[0].Guid, Value = 1 });
            //publisherNodesB.Add(new Node() { Guid = Guid.NewGuid(), Binding = sourceNodes[1].Guid, Value = 1 });
            //publisherNodesB.Add(new Node() { Guid = Guid.NewGuid(), Binding = sourceNodes[2].Guid, Value = 1 });

            Dictionary<Guid, List<Node>> publisherNodes = new Dictionary<Guid, List<Node>>();
            publisherNodes.Add(publishers[0].Guid, new List<Node>());
            publisherNodes[publishers[0].Guid].Add(publisherNodesB[0]);
            publisherNodes[publishers[0].Guid].Add(publisherNodesB[1]); 
            publisherNodes[publishers[0].Guid].Add(publisherNodesB[2]);

            publisherNodes.Add(publishers[1].Guid, new List<Node>());
            publisherNodes[publishers[1].Guid].Add(publisherNodesB[3]);
            publisherNodes[publishers[1].Guid].Add(publisherNodesB[4]);
            publisherNodes[publishers[1].Guid].Add(publisherNodesB[5]);


            // update source nodes
            await source.ReadNodeAsync(sourceNodes);

            // update publisher nodes
            foreach (var publisher in publishers)
                await publisher.ReadNodeAsync(publisherNodes[publisher.Guid]);

            // <ParentGuid, PublisherNode>
            Dictionary<Guid, List<Node>> bindings = sourceNodes.ToDictionary(x => x.Guid, y => new List<Node>());
            foreach (var node in publisherNodesB)
                bindings[node.Binding].Add(node);



            foreach (var sourceNode in sourceNodes)
                if (sourceNode.IsDirty)
                    foreach (var node in bindings[sourceNode.Guid])
                        node.UpdateValue(sourceNode.Value);
                else
                    foreach (var node in bindings[sourceNode.Guid])
                        if (node.IsDirty)
                        {
                            sourceNode.UpdateValue(node.Value);
                            foreach (var node2 in bindings[sourceNode.Guid])
                                if (node2 != node)
                                    node2.UpdateValue(node.Value);
                        }

            Console.ReadKey();

        }
    }

    public class Node
    {
        public Guid Guid { get; set; }
        public Guid Binding { get; set; }
        public bool IsDirty { get; set; }
        public object Value { get; set; }
        public void UpdateValue(object newValue)
        {
            if(Value != newValue)
            {
                IsDirty = true;
                Value = newValue;
            }
        }
    }

    public class Adapter
    {
        public Guid Guid { get; set; }
        public Task ReadNodeAsync(List<Node> nodes)
        {
            return Task.CompletedTask;
        }
    }

}
