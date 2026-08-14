using System;
using System.Collections.Generic;
using System.Text.Json;
using Elsa.Workflows;
using Graph;

namespace ElsaLib
{
    public class DynamicNodeActivity : CodeActivity
    {
        public required string NodeId { get; init; }
        public required string Label { get; init; }
        public string? ActionMethod { get; init; }
        public Dictionary<string, object>? Arguments { get; init; }
        public required List<IncomingEdge> IncomingEdges { get; init; }
        public required WorkflowData Data { get; init; }

        private readonly FileHelper fileHelper = new FileHelper();

        protected override void Execute(ActivityExecutionContext context)
        {
            bool shouldRun = IncomingEdges.Count == 0;
            foreach (IncomingEdge edge in IncomingEdges)
            {
                if (Data.Skipped.TryGetValue(edge.Source, out bool wasSkipped) && wasSkipped)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(edge.Condition))
                {
                    shouldRun = true;
                    break;
                }
                if (Data.NodeResults.TryGetValue(edge.Source, out object? value))
                {
                    bool boolValue = Convert.ToBoolean(value);
                    bool expected = edge.Condition == "Ja" || edge.Condition == "Yes";
                    if (boolValue == expected)
                    {
                        shouldRun = true;
                        break;
                    }
                }
            }

            if (!shouldRun)
            {
                Data.Skipped[NodeId] = true;
                Console.WriteLine($"[GATE] {NodeId} ({Label}) -> SKIPPED");
                return;
            }

            if (ActionMethod is null)
            {
                Console.WriteLine($"[BUILD] {NodeId} ({Label}) - Durchlauf-Node");
                return;
            }

            string filepath = GetString("filepath");
            Console.WriteLine($"[RUN] {NodeId} ({Label}) -> FileHelper::{ActionMethod}");

            object? result = ActionMethod switch
            {
                "fileExists" => fileHelper.fileExists(filepath),
                "createFile" => Execute(() => fileHelper.createFile(filepath)),
                "modifyFile" => Execute(() => fileHelper.modifyFile(filepath, GetString("input"))),
                "readFile" => fileHelper.readFile(filepath),
                _ => throw new InvalidOperationException($"Unbekannte Action: {ActionMethod}")
            };

            Data.NodeResults[NodeId] = result;
            Console.WriteLine($"      Result: {result}");
        }

        private static object? Execute(Action action)
        {
            action();
            return null;
        }

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