using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using Graph;

namespace WorkflowcoreLib
{
    public class DynamicNodeStep : StepBody
    {
        public string NodeId { get; set; } = "";
        public string Label { get; set; } = "";
        public string? ActionClass { get; set; }
        public string? ActionMethod { get; set; }
        public Dictionary<string, object>? Arguments { get; set; }
        public List<IncomingEdge> IncomingEdges { get; set; } = new List<IncomingEdge>();
        public ActionRegistry Registry { get; set; } = null!;

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            FlowWorkflowData data = (FlowWorkflowData)context.Workflow.Data;

            if (!EvaluateGate(data))
            {
                data.Skipped[NodeId] = true;
                Console.WriteLine($"[GATE] {NodeId} ({Label}) -> SKIPPED");
                return ExecutionResult.Next();
            }

            if (ActionClass is null || ActionMethod is null)
            {
                Console.WriteLine($"[BUILD] {NodeId} ({Label}) - Durchlauf-Node");
                return ExecutionResult.Next();
            }

            (object instance, MethodInfo method) = Registry.Resolve(ActionClass, ActionMethod);
            ParameterInfo[] parameters = method.GetParameters();
            object?[] args = new object?[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                string paramName = parameters[i].Name!;
                if (Arguments != null && Arguments.TryGetValue(paramName, out object? raw))
                {
                    args[i] = ConvertArgument(raw, parameters[i].ParameterType);
                }
            }

            Console.WriteLine($"[RUN] {NodeId} ({Label}) -> {ActionClass}::{ActionMethod}({string.Join(", ", args)})");
            object? result = method.Invoke(instance, args);
            data.NodeResults[NodeId] = result;
            Console.WriteLine($"      Result: {result}");

            return ExecutionResult.Next();
        }

        private bool EvaluateGate(FlowWorkflowData data)
        {
            if (IncomingEdges.Count == 0)
            {
                return true;
            }

            foreach (IncomingEdge edge in IncomingEdges)
            {
                if (data.Skipped.TryGetValue(edge.Source, out bool wasSkipped) && wasSkipped)
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
                    bool expected = edge.Condition == "Ja" || edge.Condition == "Yes";
                    if (boolValue == expected)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static object? ConvertArgument(object? raw, Type targetType)
        {
            if (raw is JsonElement el)
            {
                if (targetType == typeof(string)) return el.GetString();
                if (targetType == typeof(bool)) return el.GetBoolean();
                if (targetType == typeof(int)) return el.GetInt32();
                if (targetType == typeof(double)) return el.GetDouble();
                return el.GetRawText();
            }
            return raw;
        }
    }
   
}
