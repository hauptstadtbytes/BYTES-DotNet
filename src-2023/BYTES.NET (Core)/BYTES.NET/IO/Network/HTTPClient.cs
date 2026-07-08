//import (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.Primitives;

namespace BYTES.NET.IO.Web
{
    public class HTTPClient
    {
        #region protected variable(s)

        protected WebHeaderCollection _headers = new WebHeaderCollection();

        #endregion

        #region public method(s)

        /// <summary>
        /// performs a (basic) GET request
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
        /// performs a POST request, uploading string content
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

                return wc.UploadString(new Uri(url), "POST", content);
            }
        }

        /// <summary>
        /// performs a POST request, uploading form data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns>a (native) byte array or the respective 'string' type expression</returns>
        /// <remarks><seealso href="https://stackoverflow.com/questions/793755/how-to-fill-forms-and-submit-with-webclient-in-c-sharp"/></remarks>
        public virtual T POST<T>(string url, Dictionary<string, string> content, string contentType = "application/x-www-form-urlencoded")
        {
            //perform the request
            byte[] response = { };

            using (WebClient wc = new WebClient())
            {
                wc.Headers = _headers;
                wc.Headers.Add("Content-Type", contentType);

                wc.Headers[HttpRequestHeader.ContentType] = contentType;

                response = wc.UploadValues(url, "POST", content.ToNameValueCollection());
            }

            //return the output value
            if (typeof(T).Equals(typeof(string)))
            { //check for string request
                return (T)Convert.ChangeType(Encoding.UTF8.GetString(response), typeof(T));
            }
            else
            {
                return (T)Convert.ChangeType(response, typeof(T));
            }
        }

        /// <summary>
        /// downloads a file from webserver
        /// </summary>
        /// <param name="url"></param>
        /// <param name="destination"></param>
        public virtual void Download(string url, string destination)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers = _headers;
                wc.DownloadFile(url, destination);
            }
        }

        #endregion
    }
}
