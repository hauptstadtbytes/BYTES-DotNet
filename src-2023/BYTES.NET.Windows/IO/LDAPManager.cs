using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Windows.IO
{
    /// <summary>
    /// the LDAP manager
    /// </summary>
    /// <remarks>based on the article found at 'https://www.codemag.com/article/1312041?fb_comment_id=1421144321437986_1620428098176273'</remarks>
    public class LDAPManager
    {
        #region private properties
        
        private string _domainPath;

        #endregion


        #region constructor

        /// <summary>
        /// create a new Manager Instance
        /// </summary>
        public LDAPManager(string domainPath)
        {
            if (String.IsNullOrEmpty(domainPath))
            {
                _domainPath = GetCurrentDomain(true);
            }
            else
            {
                _domainPath = domainPath;
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// method authenticating a user by user name and password
        /// </summary>
        public bool Authenticate(string user, string password)
        {
            try
            {
                DirectoryEntry entry = new DirectoryEntry(_domainPath, user, password);

                DirectorySearcher searcher = new DirectorySearcher(entry);

                searcher.FindOne();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// method searching for entities, using the given filter
        /// </summary>
        public Dictionary<string, object>[] Search(string filter, string[] properties)
        {
            List<Dictionary<string, object>> output = new List<Dictionary<string, object>>();

            foreach (SearchResult result in GetSearchResult(_domainPath, filter))
            {
                output.Add(ParseProperties(result, properties));
            }

            return output.ToArray();
        }

        /// <summary>
        /// method returning a list of all property names
        /// </summary>
        public string[] GetProperties(string filter)
        {
            List<string> output = new List<string>();

            // get the search results and parse the output
            foreach (SearchResult result in GetSearchResult(_domainPath, filter))
            {
                foreach (string propName in ParseProperties(result).Keys)
                {
                    if (!output.Contains(propName))
                        output.Add(propName);
                }
            }

            return output.ToArray();
        }

        /// <summary>
        /// method returning the current domain properties
        /// </summary>
        /// <returns></returns>
        public static Domain GetCurrentDomain()
        {
            return Domain.GetCurrentDomain();
        }

        /// <summary>
        /// method returning the domain's "distinguished name"
        /// </summary>
        public static string? GetCurrentDomain(bool addPrefix)
        {
            DirectoryEntry entry = new DirectoryEntry("LDAP://RootDSE");

            string prefix = string.Empty;

            if (addPrefix)
            {
                prefix = "LDAP://";
            }
               
            var domainName = entry.Properties["defaultNamingContext"]?.Value;

            if (domainName == null)
            {
                return null;    
            }
            else
            {
                return prefix + domainName.ToString();
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// method searching the domain given, applying the filter given
        /// </summary>
        private SearchResult[] GetSearchResult(string domainPath, string filter = "(objectClass=simpleSecurityObject)")
        {
            DirectoryEntry entry = new DirectoryEntry(domainPath);

            DirectorySearcher searcher = new DirectorySearcher(entry);
            searcher.Filter = filter;

            List<SearchResult> output = new List<SearchResult>();
            foreach (SearchResult result in searcher.FindAll())

                output.Add(result);

            return output.ToArray();
        }

        /// <summary>
        /// method parsing a search result for the properties given
        /// </summary>
        private Dictionary<string, object?> ParseProperties(SearchResult result, string[] properties = null)
        {
            Dictionary<string, object?> output = new Dictionary<string, object?>();

            if (properties != null)
            {
                foreach (string name in properties)
                {
                    if (result.Properties[name].Count > 0)
                        output.Add(name, result.Properties[name][0]);
                    else
                        output.Add(name, null);
                }
            }
            else
                foreach (DictionaryEntry prop in result.Properties)
                {
                    if (prop.Value != null)
                        output.Add((string) prop.Key, prop.Value);
                    else
                        output.Add((string) prop.Key, null);
                }

            return output;
        }

        #endregion
    }
}