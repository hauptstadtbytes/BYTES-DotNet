//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.CmdLine
{
    public class CmdLineArgumentDefinition
    {
        #region public properties

        public string Name { get;set;}
        public string Description { get; set; }
        public bool IsCompulsory { get; set; }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isCompulsory"></param>
        public CmdLineArgumentDefinition(string name, bool isCompulsory = false)
        {
            this.Name = name;
            this.IsCompulsory = isCompulsory;
            this.Description = String.Empty;
        }

        #endregion
    }
}
