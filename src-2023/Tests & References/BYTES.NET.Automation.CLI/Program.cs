//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Automation.Scripting;
using BYTES.NET.Persistance;
using BYTES.NET.Logging;

//import namespace(s) required from 'BYTES.NET.Automation'
namespace BYTES.NET.Automation.CLI

{
    internal class Program
    {
        static void Main(string[] args)
        {
            //create a new script
            Script myScript = new Script();
            myScript.Metadata.Name = "a simple test script";
            myScript.Metadata.Description = "a simple script for testing purposes";

            Sequence myRootSequence = new Sequence();
            myRootSequence.Metadata.Name = "the root sequence";
            myRootSequence.Metadata.Description = "including a simple routine";

            MethodCall myCommentCall = new MethodCall() { Method = "Comment" };
            MethodCall mytestCall = new MethodCall() { Method = "NotWorking" };

            //myRootSequence.Calls.Add(myCommentCall);
            myRootSequence.Calls.Add(mytestCall);

            myScript.Sequences.Add(myRootSequence);
            myScript.SetRootSequence(myRootSequence.ID);

            //write the script to disk file
            //myScript.WriteToXML("D:\\Test.xml");

            //execute script
            ScriptExecutionContext myContext = new ScriptExecutionContext();
            //Log myLog = new Log();
            //myLog.Logged += OnLogged; //append the logging

            //myContext.Log = myLog;

            ScriptExecutionResult result = myScript.Execute(myContext);

            if(result.Successful)
            {
                Console.WriteLine("Script finished successfully");
            } else
            {
                Console.WriteLine("Script failed with message '" + result.Message + "' at step '" + result.Step + "' in sequence '" + result.Sequence.ToString() + "'");
            }
            
        }

    }
}
