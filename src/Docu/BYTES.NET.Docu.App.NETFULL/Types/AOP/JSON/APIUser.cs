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
    public class APIUser : LazyJSONObject
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public APIUser(JObject raw) : base(raw)
        {
        }

        public static APIUser Create(JObject raw)
        {
            List<AOPAction<APIUser>> actions = new List<AOPAction<APIUser>>();

            actions.Add(new AOPAction<APIUser>("get_FirstName", (InterceptionType type, string method, APIUser item, object[] arguments) => { if (item.IsPropertyNullOrDefault("FirstName")) { item.FirstName = item.Raw["firstName"].ToString(); }; }));
            actions.Add(new AOPAction<APIUser>("get_LastName", (InterceptionType type, string method, APIUser item, object[] arguments) => { if (item.IsPropertyNullOrDefault("LastName")) { item.LastName = item.Raw["familyName"].ToString(); }; }));
            actions.Add(new AOPAction<APIUser>("get_Age", (InterceptionType type, string method, APIUser item, object[] arguments) => { if (item.IsPropertyNullOrDefault("Age")) { item.Age = int.Parse(item.Raw["age"].ToString()); }; }));

            return APIUser.Create<APIUser>(raw, actions.ToArray());
        }
    }
}
