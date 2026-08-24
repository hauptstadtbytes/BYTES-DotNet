using BYTES.NET.IO;
using BYTES.NET.IO.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;


namespace BYTES.NET.IO.Network
{

    public class RemoteConnection: IDisposable
    {
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource,
            string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags,
            bool force);

        #region private properties

        string _path;
        UserInfo _user;

        #endregion


        #region constructor

        public RemoteConnection(string path, UserInfo user)
        {
            _path = path;
            _user = user;

            //create new Networkressource
            NetResource netResource = new NetResource()
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = path
            };

            var result = WNetAddConnection2(netResource, user.Password, user.Name, 0);
            /*if(result != 0)
            {
                throw new Exception(result);
            }*/
        }

        ~RemoteConnection()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_path, 0, true);
        }
        #endregion

    }
    
}
