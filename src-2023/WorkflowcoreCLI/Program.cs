using System;
using System.IO;
using System.Threading.Tasks;
using WorkflowcoreLib;

namespace WorkflowcoreCli
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // load the workflow json and create workflow
            string jsonText = File.ReadAllText("f2.json");
            await WFCRunner.RunAsync(jsonText);
        }
    }
}