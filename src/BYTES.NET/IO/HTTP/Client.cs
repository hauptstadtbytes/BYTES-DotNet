//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.HTTP
{
    public abstract class Client
    {
        #region protected variable(s)

        protected WebHeaderCollection _headers = new WebHeaderCollection();

        #endregion

        #region public method(s)

        /// <summary>
        /// returns the GET response of a given URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual string GET(string url)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers = _headers;

                return wc.DownloadString(new Uri(url));
            }
        }

        /// <summary>
        /// returns the POST response of a given string content for a given URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public virtual string POST(string url, string content, string contentType = "application/json")
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers = _headers;
                wc.Headers[HttpRequestHeader.ContentType] = contentType;

                return wc.UploadString(new Uri(url),"POST",content);
            }
        }

        #endregion
    }
}
