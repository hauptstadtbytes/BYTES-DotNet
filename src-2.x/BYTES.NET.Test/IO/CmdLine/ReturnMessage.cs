//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BYTES.NET.IO.CmdLine;

//import internal namespace(s) required
using BYTES.NET.IO.CmdLine.API;

namespace BYTES.NET.Test.IO.CmdLine
{
    public class ReturnMessage : ICmdLineMethod
    {
        public string Description => "Writes a message (e.g. to console)";

        public CmdLineArgumentDefinition[] Arguments
        {
            get
            {
                return new CmdLineArgumentDefinition[] { new CmdLineArgumentDefinition("msg", true) {Description = "The message text" }, new CmdLineArgumentDefinition("uppercase") { Description = "Makes all characters upper case"} };
            }
        }

        public void Execute(ref CmdLineExecutionContext context, CmdLineArguments args)
        {
            string message = args["msg"];

            if (args.ContainsKey("uppercase"))
            {
                message = message.ToUpper();
            }

            context.WriteMessage(message);
        }
    }
}
