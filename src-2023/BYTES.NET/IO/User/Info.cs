using System;
using System.Net;

namespace BYTES.NET.IO.User
{
    public class Info
    {
        #region private fields
        private readonly string _userName;
        private readonly string _userDomain;
        private readonly string _userPassword;
        #endregion

        #region public properties
        public string Name => _userName;
        public string FullName => string.IsNullOrEmpty(_userDomain) ? _userName : $"{_userDomain}\\{_userName}";
        public string? Domain => _userDomain;
        public string? Password => _userPassword;
        #endregion
        #region public methods
        /// <summary>
        /// Initializes a new instance of the <see cref="Info"/> class.
        /// </summary>
        /// <param name="user">Username (required).</param>
        /// <param name="password">Password (optional).</param>
        /// <param name="domain">Domain (optional).</param>
        public Info(string user, string password = null, string domain = null)
        {
            _userName = user;
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
                return new NetworkCredential(_userName, _userPassword ?? string.Empty);
            }
            return new NetworkCredential(_userName, _userPassword, _userDomain);
        }
        #endregion
    }
}
