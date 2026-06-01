//import (default) DotNet namespaces
using System;
using System.Net;

namespace BYTES.NET.IO
{
    /// <summary>
    /// Class for storing user information
    /// </summary>
    public class UserInfo
    {
        #region private fields
        private readonly string _userName;
        private readonly string _userDomain;
        private readonly string _userPassword;
        #endregion

        #region public properties
        public string Name => _userName;
        public string Domain => _userDomain;
        public string Password => _userPassword;
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_userDomain))
                    return _userName;

                return $"{_userDomain}\\{_userName}";
            }
        }
        #endregion


        #region public methods
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        /// <param name="user">Username (required).</param>
        /// <param name="password">Password (optional).</param>
        /// <param name="domain">Domain (optional).</param>
        public UserInfo(string username, string password = null, string domain = null)
        {
            _userName = username;
            _userPassword = password;
            _userDomain = domain;
        }

        /// <summary>
        /// Converts this user info to a <see cref="NetworkCredential"/> instance.
        /// </summary>
        /// <returns>A <see cref="NetworkCredential"/> with username, password, and optionally domain.</returns>
        public NetworkCredential ToNetworkCredential()
        {
            if (string.IsNullOrEmpty(_userDomain))
            {
                if(_userPassword == string.Empty)
                    return new NetworkCredential(_userName, string.Empty);

                return new NetworkCredential(_userName, _userPassword);
            }

            return new NetworkCredential(_userName, _userPassword, _userDomain);
        }
        #endregion
    }
}