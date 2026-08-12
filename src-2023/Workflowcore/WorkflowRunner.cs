using System;
using System.Text.Json;
using System.Threading.Tasks;
using Graph;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;


namespace WorkflowcoreLib
{
    // Einziger öffentlicher Einstiegspunkt der Bibliothek.
    // Nimmt rohen JSON-Text entgegen, macht alles Weitere selbst:
    // Deserialisieren -> Graph bauen -> WorkflowCore konfigurieren -> ausführen.
    public static class WorkflowRunner
    {
        public static async Task RunAsync(string jsonText)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            FlowGraphJson flowGraphJson = JsonSerializer.Deserialize<FlowGraphJson>(jsonText, options)!;
            Graph.WorkflowGraph executionGraph = GraphBuilder.Build(flowGraphJson);

            ActionRegistry registry = new ActionRegistry();
            registry.Register(new FileHelper());

            ServiceCollection services = new ServiceCollection();
            services.AddLogging(config => config.AddConsole());
            services.AddWorkflow();
            ServiceProvider provider = services.BuildServiceProvider();

            IWorkflowHost workflowHost = provider.GetRequiredService<IWorkflowHost>();
            workflowHost.Start();

            Workflow<FlowWorkflowData> definition = new Workflow<FlowWorkflowData>
            {
                Id = "json-flow",
                Version = 1
            };

            IWorkflowBuilder<FlowWorkflowData> stepBuilder = provider
                .GetRequiredService<IWorkflowBuilder>()
                .UseData<FlowWorkflowData>();

            WorkflowCoreConfigurator.Configure(stepBuilder, executionGraph, registry);
            definition.Steps = stepBuilder.Build();

            provider.GetRequiredService<IWorkflowRegistry>().RegisterWorkflow(definition);
            string workflowId = await workflowHost.StartWorkflow("json-flow", new FlowWorkflowData());

            Console.WriteLine($"\nWorkflow gestartet, Id: {workflowId}");
            Console.WriteLine("Drücke Enter zum Beenden...");
            Console.ReadLine();

            await workflowHost.Stop();
        }
    }
}
