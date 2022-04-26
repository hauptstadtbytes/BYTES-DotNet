//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'Newtonsoft.json' framework
using Newtonsoft.Json.Linq;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.HTTP;

using BYTES.NET.Docu.App.NETFULL.Types.AOP.JSON;

namespace BYTES.NET.Docu.App.NETFULL.Types
{
    public class MockupClient : Client
    {
        #region private variable(s)

        private string _rootURL;

        private string _usr;
        private string _pwd;
        private string _token;

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="rootURL"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public MockupClient(string rootURL, string user, string password)
        {
            _rootURL = rootURL;

            if (!_rootURL.EndsWith("/"))
            {
                _rootURL = _rootURL + "/";
            }

            _usr = user;
            _pwd = password;

            _token = GetToken();
            _headers.Add("Authorization", "Bearer " + _token);
        }

        #endregion

        #region public method(s)

        public APIUser[] GetUsers()
        {
            JObject response = JObject.Parse(base.GET(_rootURL + "users"));

            List<APIUser> output = new List<APIUser>();

            foreach(JObject obj in response["value"])
            {
                output.Add(APIUser.Create(obj));
            }

            return output.ToArray();
        }

        #endregion

        #region private method(s)

        private string GetToken()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>() { { "user", _usr }, { "password", _pwd }, };
            JObject response = JObject.Parse(base.POST<string>(_rootURL + "auth", dic));

            return response["value"].ToString();
        }

        #endregion
    }
}
