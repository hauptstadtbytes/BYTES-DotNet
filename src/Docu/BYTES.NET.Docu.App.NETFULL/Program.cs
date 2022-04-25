//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.AOP;
using BYTES.NET.AOP.API;

//import namespace(s) required from 'Newtonsoft.json' framework
using Newtonsoft.Json.Linq;

//import internal namespace(s) required
using BYTES.NET.Docu.App.NETFULL.Types.AOP;
using BYTES.NET.Docu.App.NETFULL.Types.AOP.JSON;

namespace BYTES.NET.Docu.App.NETFULL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //perform the AOP demo
            AOPDemo();

            //do not close the console
            Console.ReadLine();
        }

        private static void AOPDemo()
        {
            //create the action(s) list
            List<IAOPAction<InheritedDemoObject>> actions = new List<IAOPAction<InheritedDemoObject>>();

            AOPAction<InheritedDemoObject> actionOne = new AOPAction<InheritedDemoObject>("get_hello", (InterceptionType type, string method, InheritedDemoObject item, object[] arguments) => { Console.WriteLine("Method " + method + " called"); });
            AOPAction<InheritedDemoObject> actionTwo = new AOPAction<InheritedDemoObject>("*hello", (InterceptionType type, string method, InheritedDemoObject item, object[] arguments) => { Console.WriteLine("Method " + method + " called and recognized again"); });
            AOPAction<InheritedDemoObject> actionThree = new AOPAction<InheritedDemoObject>((InterceptionType type, string method, InheritedDemoObject item, object[] arguments) => {return item.Hello == null;}, (InterceptionType type, string method, InheritedDemoObject item, object[] arguments) => { item.Hello = "set programmatically"; });

            actions.Add(actionOne);
            actions.Add(actionTwo);
            actions.Add(actionThree);

            //perform demo one
            InheritedDemoObject instance = new InheritedDemoObject() { Hello = "Test World!" };
            InheritedDemoObject proxy = AOPProxy<InheritedDemoObject>.Create(instance, actions.ToArray());

            Console.WriteLine("Demo One: " + proxy.Hello);
            Console.WriteLine("Demo One: " + proxy.AnotherProperty);

            //perform demo two
            proxy = AOPProxy<InheritedDemoObject>.Create(new InheritedDemoObject(), actions.ToArray());

            Console.WriteLine("Demo Two: " + proxy.Hello);
            Console.WriteLine("Demo Two: " + proxy.AnotherProperty);

            //perform demo three
            string animalsSource = "[{\"spec\":\"Dog\",\"age\":2,\"eatsMeat\":\"true\"},{\"spec\":\"Cat\",\"age\":5,\"eatsMeat\":\"true\"},{\"spec\":\"Birg\",\"age\":1,\"eatsMeat\":\"false\"}]";

            foreach (JObject obj in JArray.Parse(animalsSource))
            {
                Animal animal = Animal.Create(obj);
                Console.WriteLine("Demo Three: "  + animal.Species + " of age " + animal.Age + " eats meat: " + animal.EatsMeat);
            }
        }
    }
}
