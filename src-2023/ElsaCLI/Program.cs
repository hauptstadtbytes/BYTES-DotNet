using System;
using System.IO;
using System.Threading.Tasks;
using ElsaLib;

namespace ElsaCLI
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // read json
            string jsonText = File.ReadAllText("f2.json");

            // choose which variant to run
            Console.WriteLine("Welche Variante?");
            Console.WriteLine("[1] Sequence (DynamicNodeActivity)");
            Console.WriteLine("[2] Flowchart (DynamicFlowNode)");
            Console.Write("Auswahl: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ElsaRunner.RunAsync(jsonText);
                    break;
                case "2":
                    await ElsaFlowchartRunner.RunAsync(jsonText);
                    break;
            }
        }
    }
}