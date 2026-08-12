using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowcoreLib
{
    public class WFCData
    {
        public Dictionary<string, object?> NodeResults { get; } = new Dictionary<string, object?>();
        public Dictionary<string, bool> Skipped { get; } = new Dictionary<string, bool>();
    }
}
