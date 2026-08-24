using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Activities.Flowchart.Activities;
using Elsa.Workflows.Activities.Flowchart.Models;
using Graph;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ElsaLib
{
    /// <summary>
    /// Use Elsa's native Flowchart Type to run the graph
    /// Automatically evaluates which node to run, evaluates which conditions are met
    /// so we dont need the topological sorting of the graph, but I dont want to write the graph class again without it
    /// In total we only need the nodes and edges
    /// </summary>
    public static class ElsaFlowchartRunner
    {
        public static async Task RunAsync(string jsonText)
        {
            // load json
            JsonSerializerOptions options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };
            FlowGraphJson flowGraphJson = JsonSerializer.Deserialize<FlowGraphJson>(jsonText, options)!;
            
            // create graph
            WorkflowGraph executionGraph = GraphBuilder.Build(flowGraphJson);

            ServiceCollection services = new ServiceCollection();
            services.AddElsa();
            ServiceProvider provider = services.BuildServiceProvider();

            Flowchart flowchart = Build(executionGraph);

            IWorkflowRunner runner = provider.GetRequiredService<IWorkflowRunner>();
            var result = await runner.RunAsync(flowchart);

            Console.WriteLine($"\nWorkflow-Status: {result.WorkflowState.Status}");
        }

        private static Flowchart Build(WorkflowGraph graph)
        {
            // Für jede Node genau eine Activity-Instanz anlegen -
            // wird gebraucht, um Connections zwischen denselben Objekten zu bauen.
            Dictionary<string, DynamicFlowNode> activities = graph.SortedNodes.ToDictionary(
                node => node.Id,
                node => new DynamicFlowNode
                {
                    NodeId = node.Id,
                    Label = node.Label,
                    ActionMethod = node.ActionMethod,
                    Arguments = node.Arguments
                });

            List<Connection> connections = new List<Connection>();

            foreach (ExecutionNode node in graph.SortedNodes)
            {
                foreach (IncomingEdge edge in node.IncomingEdges)
                {
                    string port = string.IsNullOrEmpty(edge.Condition) ? "Done" : edge.Condition;

                    connections.Add(new Connection
                    {
                        Source = new Endpoint { Activity = activities[edge.Source], Port = port },
                        Target = new Endpoint { Activity = activities[node.Id] }
                    });
                }
            }

            return new Flowchart
            {
                Activities = activities.Values.Cast<IActivity>().ToList(),
                Connections = connections
            };
        }
    }
}