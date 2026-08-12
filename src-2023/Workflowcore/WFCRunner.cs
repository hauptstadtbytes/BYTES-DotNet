using System;
using System.Text.Json;
using System.Threading.Tasks;
using Graph;
using Microsoft.Extensions.DependencyInjection;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowcoreLib
{
    public static class WFCRunner
    {
        public static async Task RunAsync(string jsonText)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            FlowGraphJson flowGraphJson = JsonSerializer.Deserialize<FlowGraphJson>(jsonText, options)!;
            WorkflowGraph executionGraph = GraphBuilder.Build(flowGraphJson);

            ActionRegistry registry = new ActionRegistry();
            registry.Register(new FileHelper());

            ServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddWorkflow();
            ServiceProvider provider = services.BuildServiceProvider();

            IWorkflowHost workflowHost = provider.GetRequiredService<IWorkflowHost>();
            workflowHost.Start();

            // Kein "Workflow<WFCData>" mehr - stattdessen Builder befüllen
            // und daraus eine WorkflowDefinition erzeugen lassen.
            IWorkflowBuilder<WFCData> stepBuilder = provider
                .GetRequiredService<IWorkflowBuilder>()
                .UseData<WFCData>();

            WFCAdapter.Configure(stepBuilder, executionGraph, registry);

            string workflowId = "json-flow";
            int version = 1;
            WorkflowDefinition definition = stepBuilder.Build(workflowId, version);

            provider.GetRequiredService<IWorkflowRegistry>().RegisterWorkflow(definition);
            string runId = await workflowHost.StartWorkflow(workflowId, version, new WFCData());

            Console.WriteLine($"\nWorkflow gestartet, Id: {runId}");
            Console.WriteLine("Drücke Enter zum Beenden...");
            Console.ReadLine();
        }
    }
}