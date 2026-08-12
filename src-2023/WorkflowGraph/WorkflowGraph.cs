namespace Graph
{
    // data types for the graph
    public record FlowGraphJson(List<FlowNode> Nodes, List<FlowEdge> Edges);    // input json
    public record FlowNode(string Id, FlowNodeData Data);                       
    public record FlowNodeData(string Label, string? Action, Dictionary<string, object>? Arguments);
    public record FlowEdge(string Source, string Target, FlowEdgeData? Data);
    public record FlowEdgeData(string Condition = "");

    public record IncomingEdge(string Source, string Condition);

    // node for execution with all needed infos
    public record ExecutionNode(
        string Id,
        string Label,
        string? ActionClass,
        string? ActionMethod,
        Dictionary<string, object>? Arguments,
        List<IncomingEdge> IncomingEdges);

    /// <summary>
    /// List of nodes in topological order
    /// </summary>
    public class WorkflowGraph
    {
        public List<ExecutionNode> SortedNodes { get; init; } = new();
    }
}
