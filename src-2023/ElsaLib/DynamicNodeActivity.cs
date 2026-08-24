using System;
using System.Collections.Generic;
using System.Text.Json;
using Elsa.Workflows;
using Graph;

namespace ElsaLib
{
    /// <summary>
    /// Create Elsa-native Steps for the workflow
    /// Elsa uses CodeActivity for the parts of a workflow
    /// </summary>
    public class DynamicNodeActivity : CodeActivity
    {
        // define data fields
        public required string NodeId { get; init; }
        public required string Label { get; init; }
        public string? ActionMethod { get; init; }
        public Dictionary<string, object>? Arguments { get; init; }
        public required List<IncomingEdge> IncomingEdges { get; init; }
        public required WorkflowData Data { get; init; }

        // define available methods
        private readonly FileHelper fileHelper = new FileHelper();

        /// <summary>
        /// Checks if node can be executed, and executes it if possible
        /// </summary>
        protected override void Execute(ActivityExecutionContext context)
        {
            // always run if start node
            bool shouldRun = IncomingEdges.Count == 0;

            foreach (IncomingEdge edge in IncomingEdges)
            {
                // if previous node was skipped, execute this one
                if (Data.Skipped.TryGetValue(edge.Source, out bool wasSkipped) && wasSkipped)
                {
                    continue;
                }

                // if no condition, execute this one
                if (string.IsNullOrEmpty(edge.Condition))
                {
                    shouldRun = true;
                    break;
                }

                // if the conditions are met, execute node
                // currently only boolean conditions
                if (Data.NodeResults.TryGetValue(edge.Source, out object? value))
                {
                    bool boolValue = Convert.ToBoolean(value);

                    // node should be expected to run when the condition is YES
                    bool expected = edge.Condition == "Ja" || edge.Condition == "Yes";
                    if (boolValue == expected)
                    {
                        shouldRun = true;
                        break;
                    }
                }
            }

            // skip node if it cant be executed
            if (!shouldRun)
            {
                Data.Skipped[NodeId] = true;
                Console.WriteLine($"[GATE] {NodeId} ({Label}) -> SKIPPED");
                return;
            }

            // if node has no method, run-through
            if (ActionMethod is null)
            {
                Console.WriteLine($"[BUILD] {NodeId} ({Label}) - Runthrough");
                return;
            }
            // run node

            // get filepath argument from node
            string filepath = GetString("filepath");
            Console.WriteLine($"[RUN] {NodeId} ({Label}) -> FileHelper::{ActionMethod}");

            // select correct method
            object? result = ActionMethod switch
            {
                "fileExists" => fileHelper.fileExists(filepath),
                "createFile" => Execute(() => fileHelper.createFile(filepath)),
                "modifyFile" => Execute(() => fileHelper.modifyFile(filepath, GetString("input"))),
                "readFile" => fileHelper.readFile(filepath),
                _ => throw new InvalidOperationException($"Unknown Action: {ActionMethod}")
            };

            Data.NodeResults[NodeId] = result;
            Console.WriteLine($"      Result: {result}");
        }

        /// <summary>
        /// Executes an action that does not return a value.
        /// The Action is wrapped in a delegate so that it can be
        /// passed to this helper method.
        /// </summary>
        private static object? Execute(Action action)
        {
            action();
            return null;
        }

        /// <summary>
        /// Gets a string argument from the Arguments dictionary.
        /// The argument is expected to be stored as a JsonElement.
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