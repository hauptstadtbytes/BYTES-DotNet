//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Test
{
    public class TestObject : MarshalByRefObject
    {
        public string Group { get; set; } 

        public string Name { get; set;}

        public int Indexer { get;set;}

        public TestObject Parent { get; set;}

        public bool IsMatch(string name)
        {
            if (Name.Length > 3)
            {
                return true;
            }

            return false;
        }
    }
}
