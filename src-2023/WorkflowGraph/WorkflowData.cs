using System;
using System.Collections.Generic;
using System.Text;

namespace Graph
{
    /// <summary>
    /// Data about the workflow:
    ///     results of the individual nodes
    ///     which nodes have been skipped during execution
    /// </summary>
    public class WorkflowData
    {
        public Dictionary<string, object?> NodeResults { get; } = new Dictionary<string, object?>();
        public Dictionary<string, bool> Skipped { get; } = new Dictionary<string, bool>();
    }
}
