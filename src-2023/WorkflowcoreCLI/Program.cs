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
            string filePath = args.Length > 0 ? args[0] : "f2.json";

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Datei nicht gefunden: {filePath}");
                return;
            }
            Console.WriteLine($"Arbeitsverzeichnis: {Directory.GetCurrentDirectory()}");
            string jsonText = File.ReadAllText(filePath);
            await WFCRunner.RunAsync(jsonText);
        }
    }
}