using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.System
{
    public class AdapterInfo
    {
        #region private fields
        private readonly NetworkInterface _interface;
        #endregion

        #region public properties
        public string Name
        {
            get { return _interface.Name; }
        }
        public string Description
        {
            get { return _interface.Description; }
        }

        public string Id
        {
            get { return _interface.Id; }
        }
        public string Address
        {
            get { return _interface.GetPhysicalAddress().ToString(); }
        }
        public NetworkInterfaceType Type
        {
            get { return _interface.NetworkInterfaceType; }
        }
        #endregion
        #region public methods

        public AdapterInfo(NetworkInterface intrfce)
        {
            _interface = intrfce;
        }
        #endregion


    }
}
