using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Elsa.Workflows;
using Elsa.Workflows.Activities.Flowchart.Attributes;
using Graph;

namespace ElsaLib
{
    /// <summary>
    /// Node for Activity (like Step in Workflowcore)
    /// Used for The Flowchart
    /// Dont need to parse node/ edges from json
    /// </summary>
    public class DynamicFlowNode : Activity
    {
        public required string NodeId { get; init; }
        public required string Label { get; init; }
        public string? ActionMethod { get; init; }
        public Dictionary<string, object>? Arguments { get; init; }

        private readonly FileHelper fileHelper = new FileHelper();

        // Build step + execute
        // dont need to parse the full json
        protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            // runthrough node
            if (ActionMethod is null)
            {
                Console.WriteLine($"[BUILD] {NodeId} ({Label})");
                await context.CompleteActivityWithOutcomesAsync("Done");
                return;
            }

            // get argument if defined -> helper method
            string filepath = GetString("filepath");
            Console.WriteLine($"[RUN] {NodeId} ({Label}) -> {ActionMethod}");

            // run the methods for the node
            object? result = ActionMethod switch
            {
                "fileExists" => fileHelper.fileExists(filepath),
                "createFile" => Void(() => fileHelper.createFile(filepath)),
                "modifyFile" => Void(() => fileHelper.modifyFile(filepath, GetString("input"))),
                "readFile" => fileHelper.readFile(filepath)
            };

            Console.WriteLine($"Result: {result}");

            // outcome/ result of node -> for now (this example) only bool or done for runthrough nodes
            string outcome = result is bool b ? (b ? "Ja" : "Nein") : "Done";
            await context.CompleteActivityWithOutcomesAsync(outcome);
        }

        /// <summary>
        /// Executes an action that does not return a value.
        /// Need to wrap it, cause otherwise we get an error because of no return type/ void
        /// </summary>
        private static object? Void(Action action)
        {
            action();
            return null;
        }

        /// <summary>
        /// Get argument string from json
        /// </summary>
        private string GetString(string key)
        {
            if (Arguments != null && Arguments.TryGetValue(key, out object? raw) && raw is JsonElement el)
            {
                return el.GetString() ?? "";
            }
            return "";
        }
    }
}