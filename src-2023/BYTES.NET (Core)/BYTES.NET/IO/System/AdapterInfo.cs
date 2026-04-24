//import (default) DotNet namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.System
{
    /// <summary>
    /// Class for collecting information about the system adapters
    /// </summary>
    public class AdapterInfo
    {
        #region private properties

        private readonly NetworkInterface _interface;

        #endregion


        #region public properties

        public string Name { get => _interface.Name; }

        public string Description { get => _interface.Description; }

        public string Id { get => _interface.Id; }

        public string Address { get => _interface.GetPhysicalAddress().ToString(); }

        public NetworkInterfaceType Type { get => _interface.NetworkInterfaceType; }

        #endregion


        #region constructor

        public AdapterInfo(NetworkInterface intrfce)
        {
            _interface = intrfce;
        }

        #endregion
    }
}
