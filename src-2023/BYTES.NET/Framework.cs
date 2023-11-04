//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BYTES.NET
{
    /// <summary>
    /// (global) framework method(s)
    /// </summary>
    public static class Framework
    {
        #region public properties

        /// <summary>
        /// the file system path for the 'BYTES.NET' library assembly
        /// </summary>
        /// <returns></returns>
        public static string AssemblyPath { get
            {
                Uri assembly = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase);
                return assembly.LocalPath;
            }
        }

        /// <summary>
        /// the 'BYTES.NET' assembly's parent file system path
        /// </summary>
        public static string AssemblyDirectory { get
            {
                return Path.GetDirectoryName(AssemblyPath);
            }
        }

        #endregion
    }
}
