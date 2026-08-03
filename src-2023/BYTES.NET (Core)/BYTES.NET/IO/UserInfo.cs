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

        public string FullName => GetFullName();

        #endregion


        #region public methods

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        public UserInfo(string username, string password = null, string domain = null)
        {
            _userName = username;
            _userPassword = password;
            _userDomain = domain;
        }

        /// <summary>
        /// Converts this user info to a <see cref="NetworkCredential"/> instance.
        /// </summary>
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


        #region private methods

        private string GetFullName()
        {
            if (string.IsNullOrEmpty(_userDomain))
            {
                return _userName;
            }

            return $"{_userName}@{_userDomain}";
        }

        #endregion
    }
}