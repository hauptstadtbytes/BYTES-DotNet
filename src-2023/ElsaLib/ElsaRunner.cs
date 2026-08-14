using System;
using System.Text.Json;
using System.Threading.Tasks;
using Elsa.Extensions;
using Elsa.Workflows;
using Graph;
using Elsa.Workflows.Activities;
using Microsoft.Extensions.DependencyInjection;

namespace ElsaLib
{
    public static class ElsaRunner
    {
        /// <summary>
        /// 
        /// </summary>
        public static async Task RunAsync(string jsonText)
        {
            JsonSerializerOptions options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };

            // create graph from the json
            FlowGraphJson flowGraphJson = JsonSerializer.Deserialize<FlowGraphJson>(jsonText, options)!;
            WorkflowGraph executionGraph = GraphBuilder.Build(flowGraphJson);

            // load data
            WorkflowData data = new WorkflowData();

            // create dependency injection service, register Elsa services
            ServiceCollection services = new ServiceCollection();
            services.AddElsa();

            // build service provider
            ServiceProvider provider = services.BuildServiceProvider();

            // convert graph into Elsa Workflow
            IActivity workflow = Build(executionGraph, data);

            // get the runner from the service provider
            // run the workflow
            IWorkflowRunner runner = provider.GetRequiredService<IWorkflowRunner>();
            var result = await runner.RunAsync(workflow);

            Console.WriteLine($"\nWorkflow-Status: {result.WorkflowState.Status}");
        }

        /// <summary>
        ///  Create the workflow from the graph
        /// </summary>
        private static IActivity Build(WorkflowGraph graph, WorkflowData data)
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