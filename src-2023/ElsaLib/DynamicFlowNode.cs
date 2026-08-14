using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Elsa.Workflows;
using Elsa.Workflows.Activities.Flowchart.Attributes;
using Graph;

namespace ElsaLib
{
    // [FlowNode] deklariert, welche Outcomes diese Activity innerhalb
    // eines Flowcharts überhaupt haben darf - "Done" für unbedingte Kanten,
    // "Ja"/"Nein" für unsere bool-Ergebnis-Verzweigungen.
    [FlowNode("Ja", "Nein", "Done")]
    public class DynamicFlowNode : Activity
    {
        public required string NodeId { get; init; }
        public required string Label { get; init; }
        public string? ActionMethod { get; init; }
        public Dictionary<string, object>? Arguments { get; init; }

        private readonly FileHelper fileHelper = new FileHelper();


        /// <summary>
        /// 
        /// </summary>
        protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            if (ActionMethod is null)
            {
                Console.WriteLine($"[BUILD] {NodeId} ({Label}) - Durchlauf-Node");
                await context.CompleteActivityWithOutcomesAsync("Done");
                return;
            }

            string filepath = GetString("filepath");
            Console.WriteLine($"[RUN] {NodeId} ({Label}) -> {ActionMethod}");

            object? result = ActionMethod switch
            {
                "fileExists" => fileHelper.fileExists(filepath),
                "createFile" => Void(() => fileHelper.createFile(filepath)),
                "modifyFile" => Void(() => fileHelper.modifyFile(filepath, GetString("input"))),
                "readFile" => fileHelper.readFile(filepath),
                _ => throw new InvalidOperationException($"Unbekannte Action: {ActionMethod}")
            };

            Console.WriteLine($"      Result: {result}");

            // Bool-Ergebnis -> "Ja"/"Nein"-Outcome (steuert die Verzweigung),
            // alles andere -> "Done" (unbedingter Weiterlauf).
            string outcome = result is bool b ? (b ? "Ja" : "Nein") : "Done";
            await context.CompleteActivityWithOutcomesAsync(outcome);
        }

        /// <summary>
        /// Executes an action that does not return a value.
        /// The Action is wrapped in a delegate so that it can be
        /// passed to this helper method.
        /// </summary>
        private static object? Void(Action action)
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