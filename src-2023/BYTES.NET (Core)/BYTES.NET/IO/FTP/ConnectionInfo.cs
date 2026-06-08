using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.FTP
{
    /// <summary>
    /// Returns information about FTP connection
    /// </summary>
    public class ConnectionInfo
    {
        #region private properties

        private readonly string _uri;
        private readonly UserInfo _user;

        #endregion

        #region constructor

        /// <summary>
        /// Create new ConnectionInfo
        /// </summary>
        public ConnectionInfo(string uri, UserInfo user = null)
        {
            _uri = uri;
            _user = new UserInfo("anonymous");

            if(user != null)
            {
                _user = user;
            }
        }

        #endregion


        #region public methods

        /// <summary>
        /// Returns list of remote items
        /// </summary>
        /// <returns></returns>
        public FTPRemoteItem[] GetItems()
        {
            List<FTPRemoteItem> output = new List<FTPRemoteItem>();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_uri);

            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = _user.ToNetworkCredential();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            while(reader.EndOfStream == false)
            {
                FTPRemoteItem item = new FTPRemoteItem(reader.ReadLine(), this);

                if(!(item.Name == ".") & !(item.Name == ".."))
                {
                    output.Add(item);
                }
            }
            return output.ToArray();
        }

        #endregion
    }
}
