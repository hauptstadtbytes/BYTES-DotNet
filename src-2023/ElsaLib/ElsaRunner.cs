using System;
using System.Text.Json;
using System.Threading.Tasks;
using Elsa.Extensions;
using Elsa.Workflows;
using Graph;
using Microsoft.Extensions.DependencyInjection;

namespace ElsaLib
{
    public static class ElsaRunner
    {
        public static async Task RunAsync(string jsonText)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            FlowGraphJson flowGraphJson = JsonSerializer.Deserialize<FlowGraphJson>(jsonText, options)!;
            WorkflowGraph executionGraph = GraphBuilder.Build(flowGraphJson);

            WorkflowData data = new WorkflowData();

            ServiceCollection services = new ServiceCollection();
            services.AddElsa();
            ServiceProvider provider = services.BuildServiceProvider();

            IActivity workflow = ElsaAdapter.Build(executionGraph, data);

            IWorkflowRunner runner = provider.GetRequiredService<IWorkflowRunner>();
            var result = await runner.RunAsync(workflow);

            Console.WriteLine($"\nWorkflow-Status: {result.WorkflowState.Status}");
        }
    }
}