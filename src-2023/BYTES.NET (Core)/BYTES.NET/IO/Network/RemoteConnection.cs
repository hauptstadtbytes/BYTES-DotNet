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
    /// <summary>
    /// remote connection data structure class
    /// </summary>
    /// <remark> only required when creating a new 'RemotefolderInfo' using a dedicated user account</remark>
    /// <remark> using https://stackoverflow.com/questions/295538/how-to-provide-user-name-and-password-when-connecting-to-a-network-share as a reference</remark>
    public class RemoteConnection: IDisposable
    {
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(RemoteRessource netResource,
            string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags,
            bool force);

        #region private properties

        string _path;
        UserInfo _user;

        #endregion


        #region constructor

        /// <summary>
        /// Create a RemoteConnection
        /// </summary>
        /// <param name="path"></param>
        /// <param name="user"></param>
        public RemoteConnection(string path, UserInfo user)
        {
            _path = path;
            _user = user;

            RemoteRessource netResource = new RemoteRessource()
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = path
            };

            var result = WNetAddConnection2(netResource, user.Password, user.Name, 0);
            
            // why commented out???
            /*if(result != 0)
            {
                throw new Exception(result);
            }*/
        }

        //???
        ~RemoteConnection()
        {
            Dispose(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_path, 0, true);
        }

        #endregion

    }
    
}
