using BYTES.NET.IO;
using BYTES.NET.IO.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Saves information about connection to a FileShare
    /// </summary>
    /// <remark> only required when creating a new 'RemotefolderInfo' using a dedicated user account</remark>
    /// <remark> using https://stackoverflow.com/questions/295538/how-to-provide-user-name-and-password-when-connecting-to-a-network-share as a reference</remark>
    public class RemoteConnection: IDisposable
    {
        #region dll imports

        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        private static extern int WNetAddConnection2(RemoteRessource netResource,
            string password, string username, int flags);

        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        private static extern int WNetCancelConnection2(string name, int flags,
            bool force);

        #endregion


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
        public RemoteConnection(string path, UserInfo user, ResourceScope scope, ResourceType resourceType, ResourceDisplaytype displayType)
        {
            _path = path;
            _user = user;

            RemoteRessource netResource = new RemoteRessource()
            {
                Scope = scope,
                ResourceType = resourceType,
                DisplayType = displayType,
                RemoteName = path
            };

            int result = WNetAddConnection2(netResource, user.Password, user.Name, 0);
            
            if(result != 0)
            {
                throw new Win32Exception(result);
            }
        }

        /// <summary>
        /// Overloading constructor with standard values
        /// </summary>
        /// <param name="path"></param>
        /// <param name="user"></param>
        public RemoteConnection(string path, UserInfo user)
        : this(path, user, ResourceScope.GlobalNetwork, ResourceType.Disk, ResourceDisplaytype.Share) { }

        /// <summary>
        /// Destructor
        /// </summary>
        ~RemoteConnection()
        {
            Dispose(true);
        }

        /// <summary>
        /// Deletes the connection to the share and frees the memory
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Severs the connection to the share
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_path, 0, true);
        }

        #endregion

    }
    
}
