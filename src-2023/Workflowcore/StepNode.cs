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
    /// WFC uses StepBody for the parts of a workflow
    /// </summary>
    public class StepNode : StepBody
    {
        // define data fields
        public string NodeId { get; set; } = "";
        public string Label { get; set; } = "";
        public string? ActionMethod { get; set; }
        public Dictionary<string, object>? Arguments { get; set; }
        public List<IncomingEdge> IncomingEdges { get; set; } = new();

        // define the methods
        public FileHelper FileHelper { get; set; } = null!;

        /// <summary>
        /// Entry point called for execution of node
        /// </summary>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // get workflow data -> result of earlier nodes
            WorkflowData data = (WorkflowData)context.Workflow.Data;

            // check if we cannot execute the current node
            // due to unmet conditions, etc
            if (!EvaluateGate(data))
            {
                // mark as skipped
                data.Skipped[NodeId] = true;
                Console.WriteLine($"[GATE] {NodeId} ({Label}) -> SKIPPED");

                // continue with the next step
                return ExecutionResult.Next();
            }

            // if no method specified, node is only pass-through node
            if (string.IsNullOrEmpty(ActionMethod))
            {
                Console.WriteLine($"[BUILD] {NodeId} ({Label}) - Durchlauf-Node");
                return ExecutionResult.Next();
            }

            // execute specified method
            object? result = ExecuteAction();

            // save result
            data.NodeResults[NodeId] = result;

            Console.WriteLine($"[RUN] {NodeId} ({Label}) -> {ActionMethod}");
            Console.WriteLine($"Result: {result}");

            return ExecutionResult.Next();
        }

        /// <summary>
        /// Find and execute method specified in node
        /// use FileHelper class as a "repo" for all available methods
        /// </summary>
        private object? ExecuteAction()
        {
            // Use reflection to find ActionMethod in FileHelper
            MethodInfo? method = typeof(FileHelper).GetMethod(ActionMethod!, BindingFlags.Public | BindingFlags.Instance);

            // stop if method doesnt exist
            if (method == null)
            {
                throw new InvalidOperationException($"FileHelper enthält keine Methode '{ActionMethod}'.");
            }

            // get info about required parameters
            ParameterInfo[] parameters = method.GetParameters();

            // create array for arguments and fill it   
            object?[] args = new object?[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                string parameterName = parameters[i].Name!;

                // get argument value
                if (Arguments == null || !Arguments.TryGetValue(parameterName, out object? raw))
                {
                    throw new InvalidOperationException($"Argument '{parameterName}' for '{ActionMethod}' is missing.");
                }
                args[i] = ConvertArgument(raw, parameters[i].ParameterType);
            }

            // invoke the method on the FileHelper Instance, return stuff to the caller
            return method.Invoke(FileHelper, args);
        }

        /// <summary>
        /// Convert the argument to the type needed
        /// </summary>
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

        /// <summary>
        /// Evaluate if we can run the current node
        /// Can only evaluate bools for now (prototype)
        /// </summary>
        private bool EvaluateGate(WorkflowData data)
        {
            // can always execute starting node
            if (IncomingEdges.Count == 0)
            {
                return true;
            }

            // iterate over each incomming edge
            foreach (IncomingEdge edge in IncomingEdges)
            {
                // ignore edge if node connected to it was skipped
                if (data.Skipped.TryGetValue(edge.Source, out bool wasSkipped) && wasSkipped)
                {
                    continue;
                }

                // if no condition, can always execute node
                if (string.IsNullOrEmpty(edge.Condition))
                {
                    return true;
                }

                // get result from previous node
                if (data.NodeResults.TryGetValue(edge.Source, out object? value))
                {
                    bool boolValue = Convert.ToBoolean(value);

                    // if condition met, we can execute this node
                    bool expected = edge.Condition == "Ja" || edge.Condition == "Yes";
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
