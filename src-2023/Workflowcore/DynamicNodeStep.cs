using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using WorkflowCore;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using Graph;

namespace WorkflowcoreLib
{
    /// <summary>
    /// Create WorkflowCore-native Steps for the workflow
    /// </summary>
    public class DynamicNodeStep : StepBody
    {
        public string NodeId { get; set; } = "";
        public string Label { get; set; } = "";
        public string? ActionMethod { get; set; }
        public Dictionary<string, object>? Arguments { get; set; }
        public List<IncomingEdge> IncomingEdges { get; set; } = new();

        public FileHelper FileHelper { get; set; } = null!;

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            WorkflowData data = (WorkflowData)context.Workflow.Data;

            if (!EvaluateGate(data))
            {
                data.Skipped[NodeId] = true;
                Console.WriteLine($"[GATE] {NodeId} ({Label}) -> SKIPPED");
                return ExecutionResult.Next();
            }

            if (string.IsNullOrEmpty(ActionMethod))
            {
                Console.WriteLine($"[BUILD] {NodeId} ({Label}) - Durchlauf-Node");
                return ExecutionResult.Next();
            }

            object? result = ExecuteAction();

            data.NodeResults[NodeId] = result;

            Console.WriteLine(
                $"[RUN] {NodeId} ({Label}) -> {ActionMethod}");

            Console.WriteLine($"      Result: {result}");

            return ExecutionResult.Next();
        }

        private object? ExecuteAction()
        {
            MethodInfo? method = typeof(FileHelper).GetMethod(
                ActionMethod!,
                BindingFlags.Public | BindingFlags.Instance);

            if (method == null)
            {
                throw new InvalidOperationException(
                    $"FileHelper enthält keine Methode '{ActionMethod}'.");
            }

            ParameterInfo[] parameters = method.GetParameters();
            object?[] args = new object?[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                string parameterName = parameters[i].Name!;

                if (Arguments == null ||
                    !Arguments.TryGetValue(parameterName, out object? raw))
                {
                    throw new InvalidOperationException(
                        $"Argument '{parameterName}' für '{ActionMethod}' fehlt.");
                }

                args[i] = ConvertArgument(
                    raw,
                    parameters[i].ParameterType);
            }

            return method.Invoke(FileHelper, args);
        }

        private static object? ConvertArgument(object? raw, Type targetType)
        {
            if (raw is JsonElement element)
            {
                return element.Deserialize(targetType);
            }

            if (raw == null)
            {
                return null;
            }

            if (targetType.IsInstanceOfType(raw))
            {
                return raw;
            }

            return Convert.ChangeType(raw, targetType);
        }

        private bool EvaluateGate(WorkflowData data)
        {
            if (IncomingEdges.Count == 0)
            {
                return true;
            }

            foreach (IncomingEdge edge in IncomingEdges)
            {
                if (data.Skipped.TryGetValue(edge.Source, out bool wasSkipped)
                    && wasSkipped)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(edge.Condition))
                {
                    return true;
                }

                if (data.NodeResults.TryGetValue(edge.Source, out object? value))
                {
                    bool boolValue = Convert.ToBoolean(value);

                    bool expected =
                        edge.Condition == "Ja" ||
                        edge.Condition == "Yes";

                    if (boolValue == expected)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
