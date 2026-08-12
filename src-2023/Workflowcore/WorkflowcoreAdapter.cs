using System;
using System.Collections.Generic;
using System.Text;
using Graph;
using WorkflowCore.Interface;

namespace WorkflowcoreLib
{
    public static class WorkflowCoreAdapter
    {
        // Nimmt den fertigen, sortierten Graphen (aus WorkflowGraph.GraphBuilder.Build)
        // und hängt für jeden Node einen DynamicNodeStep an den Builder.
        public static void Configure(
            IWorkflowBuilder<FlowWorkflowData> builder,
            WorkflowGraph graph,
            ActionRegistry registry)
        {
            IStepBuilder<FlowWorkflowData, DynamicNodeStep>? step = null;

            foreach (ExecutionNode node in graph.SortedNodes)
            {
                step = step is null
                    ? builder.StartWith<DynamicNodeStep>()
                    : step.Then<DynamicNodeStep>();

                step.Input(s => s.NodeId, _ => node.Id)
                    .Input(s => s.Label, _ => node.Label)
                    .Input(s => s.ActionClass, _ => node.ActionClass)
                    .Input(s => s.ActionMethod, _ => node.ActionMethod)
                    .Input(s => s.Arguments, _ => node.Arguments)
                    .Input(s => s.IncomingEdges, _ => node.IncomingEdges)
                    .Input(s => s.Registry, _ => registry);
            }
        }
    }
}
