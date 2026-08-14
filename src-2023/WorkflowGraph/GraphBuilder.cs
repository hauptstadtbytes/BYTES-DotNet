using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Graph
{
    public static class GraphBuilder
    {
        /// <summary>
        /// Create a graph from the json
        /// </summary>
        public static WorkflowGraph Build(FlowGraphJson json)
        {
            Dictionary<string, FlowNode> nodes = json.Nodes.ToDictionary(n => n.Id);
            Dictionary<string, List<string>> adjacency = nodes.Keys.ToDictionary(id => id, _ => new List<string>());
            Dictionary<string, int> inDegree = nodes.Keys.ToDictionary(id => id, _ => 0);
            Dictionary<string, List<IncomingEdge>> incoming = new Dictionary<string, List<IncomingEdge>>();

            foreach (FlowEdge e in json.Edges)
            {
                string condition = e.Data?.Condition ?? "";     // set condition or ""
                //get list for node or create new one
                if (!incoming.TryGetValue(e.Target, out List<IncomingEdge>? list))
                {
                    incoming[e.Target] = list = new List<IncomingEdge>();
                }
                    
                list.Add(new IncomingEdge(e.Source, condition));

                // save connection
                adjacency[e.Source].Add(e.Target);
                inDegree[e.Target]++;
            }

            // sort nodes topologically
            Dictionary<string, int> remaining = new Dictionary<string, int>(inDegree);
            Queue<string> queue = new Queue<string>(remaining.Where(kv => kv.Value == 0).Select(kv => kv.Key));
            List<string> sortedIds = new List<string>();

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                sortedIds.Add(current);
                foreach (string target in adjacency[current])
                    if (--remaining[target] == 0)
                        queue.Enqueue(target);
            }

            if (sortedIds.Count != nodes.Count)
                throw new InvalidOperationException("Zyklus im Graph erkannt");

            // put sorted nodes into list
            List<ExecutionNode> result = sortedIds.Select(id =>
            {
                FlowNode node = nodes[id];
                List<IncomingEdge> edgesIn = incoming.TryGetValue(id, out List<IncomingEdge> e)
                    ? e
                    : new List<IncomingEdge>();

                // split the method call into class and method
                string? actionMethod = node.Data.Action;

                return new ExecutionNode(id, node.Data.Label, actionMethod, node.Data.Arguments, edgesIn);
            }).ToList();

            return new WorkflowGraph { SortedNodes = result };
        }
    }
}
