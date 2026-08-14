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
        /// <summary>
        /// Create graph and workflow from json, run workflow
        /// </summary>
        public static async Task RunAsync(string jsonText)
        {
            JsonSerializerOptions options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };

            // load json
            FlowGraphJson flowGraphJson = JsonSerializer.Deserialize<FlowGraphJson>(jsonText, options)!;

            // create graph
            WorkflowGraph executionGraph = GraphBuilder.Build(flowGraphJson);

            // "load" methods
            FileHelper fileHelper = new FileHelper();

            // setup dependency injection container
            // needed internally for IWorkflow etc
            ServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddWorkflow();

            ServiceProvider provider = services.BuildServiceProvider();

            // runs workflows
            IWorkflowHost workflowHost = provider.GetRequiredService<IWorkflowHost>();
            workflowHost.Start();

            // init builder to later create workflow
            IWorkflowBuilder<WorkflowData> stepBuilder = provider
                .GetRequiredService<IWorkflowBuilder>()
                .UseData<WorkflowData>();

            Configure(stepBuilder, executionGraph, fileHelper);

            // create workflow, define id and version statically for now
            WorkflowDefinition definition = stepBuilder.Build("flow", 1);

            // create definition for workflow
            provider.GetRequiredService<IWorkflowRegistry>().RegisterWorkflow(definition);

            // runs workflow
            string runId = await workflowHost.StartWorkflow("flow", 1, new WorkflowData());

            Console.WriteLine($"\nWorkflow gestartet, Id: {runId}");

            // need to wait for workflow to be done before exiting function
            // so we poll if the service is done
            IPersistenceProvider persistence = provider.GetRequiredService<IPersistenceProvider>();

            WorkflowInstance instance = await persistence.GetWorkflowInstance(runId);
            while (instance.Status == WorkflowStatus.Runnable)
            {
                await Task.Delay(200); 
            }

            Console.WriteLine($"Workflow beendet, Status: {instance.Status}");
        }

        /// <summary>
        /// Recieve graph, create DynamicNodeStep for each node
        /// </summary>
        private static void Configure(IWorkflowBuilder<WorkflowData> builder, WorkflowGraph graph, FileHelper fileHelper)
        {
            // memorize last node
            IStepBuilder<WorkflowData, DynamicNodeStep>? step = null;

            // create the step for the workflow
            foreach (ExecutionNode node in graph.SortedNodes)
            {
                // check if we are at the start node
                step = step is null
                    ? builder.StartWith<DynamicNodeStep>()
                    : step.Then<DynamicNodeStep>();

                // fill the properties for each step
                // lambda
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