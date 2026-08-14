using System.Collections.Generic;
using System.Linq;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Graph;

namespace ElsaLib
{
    public static class ElsaAdapter
    {
        public static IActivity Build(WorkflowGraph graph, WorkflowData data)
        {
            List<IActivity> activities = graph.SortedNodes.Select(node => (IActivity)new DynamicNodeActivity
            {
                NodeId = node.Id,
                Label = node.Label,
                ActionMethod = node.ActionMethod,
                Arguments = node.Arguments,
                IncomingEdges = node.IncomingEdges,
                Data = data
            }).ToList();

            return new Sequence { Activities = activities };
        }
    }
}