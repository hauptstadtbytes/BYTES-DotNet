//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'Newtonsoft.json' framework
using Newtonsoft.Json.Linq;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.AOP;
using BYTES.NET.Primitives.Extensions;

namespace BYTES.NET.Docu.App.NETFULL.Types.AOP.JSON
{
    public class Animal : LazyJSONObject
    {
        public string Species { get;set; }

        public int Age { get;set;}

        public bool EatsMeat { get;set;}

        public Animal(JObject raw) : base(raw)
        {
        }

        public static Animal Create(JObject raw)
        {
            List<AOPAction<Animal>> actions = new List<AOPAction<Animal>>();

            actions.Add(new AOPAction<Animal>("get_Species", (InterceptionType type, string method, Animal item, object[] arguments) => { if (item.IsPropertyNullOrDefault("Species")) { item.Species = item.Raw["spec"].ToString(); }; }));
            actions.Add(new AOPAction<Animal>("get_Age", (InterceptionType type, string method, Animal item, object[] arguments) => { if (item.IsPropertyNullOrDefault("Age")) { item.Age = int.Parse(item.Raw["age"].ToString()); }; }));
            actions.Add(new AOPAction<Animal>("get_EatsMeat", (InterceptionType type, string method, Animal item, object[] arguments) => { if (item.IsPropertyNullOrDefault("EatsMeat")) { item.EatsMeat = bool.Parse(item.Raw["eatsMeat"].ToString()); }; }));

            return Animal.Create<Animal>(raw, actions.ToArray());
        }
    }
}
