using System;
using System.Collections.Generic;
using System.Text;
using Graph;
using WorkflowCore.Interface;

namespace WorkflowcoreLib
{
    public static class WFCAdapter
    {
        /// <summary>
        /// Recieve graph, create DynamicNodeStep for each node
        /// </summary>
        public static void Configure(
            IWorkflowBuilder<WorkflowData> builder,
            WorkflowGraph graph,
            FileHelper fileHelper)
        {
            IStepBuilder<WorkflowData, DynamicNodeStep>? step = null;

            foreach (ExecutionNode node in graph.SortedNodes)
            {
                step = step is null
                    ? builder.StartWith<DynamicNodeStep>()
                    : step.Then<DynamicNodeStep>();

                step.Input(s => s.NodeId, _ => node.Id)
                    .Input(s => s.Label, _ => node.Label)
                    .Input(s => s.ActionMethod, _ => node.ActionMethod)
                    .Input(s => s.Arguments, _ => node.Arguments)
                    .Input(s => s.IncomingEdges, _ => node.IncomingEdges)
                    .Input(s => s.FileHelper, _ => fileHelper);
            }
        }
    }
}
