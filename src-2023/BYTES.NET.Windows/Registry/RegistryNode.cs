using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace BYTES.NET.Windows.Registry
{
    /// <summary>
    /// 
    /// </summary>
    public class RegistryNode
    {
        #region public variables

        /// <summary>
        /// 
        /// </summary>
        public enum EnumerationOptions
        {
            IgnoreCase,
            ContainsSearch
        }

        #endregion


        #region private variable

        private Microsoft.Win32.RegistryKey _root;

        #endregion


        #region private properties

        public Microsoft.Win32.RegistryKey Root { get => _root; }

        public string Path { get => _root.Name; }
            
        /// <summary>
        /// Get the values of the key
        /// </summary>
        public Dictionary<string, object> Values
        {
            get
            {
                Dictionary<string, object> output = new Dictionary<string, object>();

                foreach (string name in _root.GetValueNames())
                {
                    output.Add(name, _root.GetValue(name));
                }

                return output;
            }
        }

        /// <summary>
        /// Get the subkeys of the current key
        /// </summary>
        public RegistryNode[] Children
        {
            get
            {
                List<RegistryNode> output = new List<RegistryNode>();

                foreach (string name in _root.GetSubKeyNames())
                {
                    output.Add(new RegistryNode(_root.OpenSubKey(name)));
                }

                return output.ToArray();
            }
        }

        #endregion


        #region constructor 

        /// <summary>
        /// Constructor
        /// Uses the key path
        /// </summary>
        /// <param name="path"></param>
        public RegistryNode(string path)
        {
            _root = RegistryNode.GetKey(path);
        }

        /// <summary>
        /// Overloaded Constructor
        /// Uses the key directly
        /// </summary>
        /// <param name="key"></param>
        public RegistryNode(Microsoft.Win32.RegistryKey key)
        {
            _root = key;
        }

        #endregion


        #region public methods

        /// <summary>
        /// Get the key by path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static RegistryKey GetKey(string path)
        {
            if(path == "HKEY_LOCAL_MACHINE")
            {
                return Microsoft.Win32.Registry.LocalMachine;
            }
            else if (path.StartsWith("HKEY_LOCAL_MACHINE"))
            {
                return GetSubKey(Microsoft.Win32.Registry.LocalMachine, path.Replace("HKEY_LOCAL_MACHINE", ""));
            }

            return null;
        }

        /// <summary>
        /// Returns value of specific key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subkey"></param>
        public object GetKeyValue(string key, string subkey = null)
        {
            RegistryKey target;

            if (subkey == null)
            {
                target = _root;
            }
            else
            {
                target = _root.OpenSubKey(subkey, true);
            }

            return target.GetValue(key);
        }


        /// <summary>
        /// Method searching for children matching the filter given
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public RegistryNode[] SearchForChildren(Dictionary<string, string> filter = null, EnumerationOptions[] options = null)
        {
            List<RegistryNode> output = new List<RegistryNode>();

            if (Information.IsNothing(filter))
            {
                return this.Children;
            }
                
            bool ignoreCase = true;
            bool containsSearch = true;

            if (!options.Contains(EnumerationOptions.IgnoreCase))
            {
                ignoreCase = false;
            }

            if (!options.Contains(EnumerationOptions.ContainsSearch))
            {
                containsSearch = false;
            }

            foreach (RegistryNode child in this.Children)
            {
                if (ValidateNodeByFilter(child, filter, ignoreCase, containsSearch))
                {
                    output.Add(child);
                }        
            }

            return output.ToArray();
        }

        /// <summary>
        /// Delete the subkey
        /// Automatically checks permissions and throws an exception
        /// </summary>
        public void DeleteKey(string key, string subkey = null)
        {
            RegistryKey target;

            if (subkey == null)
            {
                target = _root;
            }
            else
            {
                target = _root.OpenSubKey(subkey, true);
            }

            target.DeleteSubKey(key);
        }

        /// <summary>
        /// Create a new key
        /// When given a parent, create the given key as a child of the parent
        /// </summary>
        /// <param name="key"></param>
        public void AddKey(string key, string parent = null)
        {
            RegistryKey target;

            if (parent == null)
            {
                target = _root;                
            }
            else
            {
                target = _root.OpenSubKey(parent, true);
            }

            target.CreateSubKey(key);
        }

        /// <summary>
        /// Adds a key to the given root and sets it to the value
        /// Can recieve an optional subkey whose value to change instead of the parent key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="subkey"></param>
        public void SetKeyValue(string key, object value, string subkey = null)
        {
            RegistryKey target;

            if (subkey == null)
            {
                target = _root;
            }
            else
            {
                target = _root.OpenSubKey(subkey, true);
            }

            target.SetValue(key, value);
        }

        
        #endregion


        #region private methods

        /// <summary>
        /// Return the subkey
        /// </summary>
        /// <param name="root"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private static Microsoft.Win32.RegistryKey GetSubKey(Microsoft.Win32.RegistryKey root, string path)
        {
            if (path.StartsWith(@"\"))
            {
                path = path.Substring(1);
            }   

            return root.OpenSubKey(path, true);
        }

        /// <summary>
        /// Validate if the values of the node match the filter
        /// </summary>
        /// <param name="node"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private bool ValidateNodeByFilter(RegistryNode node, Dictionary<string, string> filter, bool ignoreCase, bool containsSearch)
        {
            Dictionary<string, object> vals = node.Values;

            foreach (KeyValuePair<string, string> pair in filter)
            {
                if (!vals.ContainsKey(pair.Key))
                {
                    return false;
                }
                    
                if (ignoreCase)
                {
                    if (containsSearch)
                    {
                        if (!vals[pair.Key].ToString().ToLower().Contains(pair.Value.ToLower()))
                        {
                            return false;
                        }    
                    }
                    else if (!(vals[pair.Key].ToString().ToLower() == pair.Value.ToLower()))
                    {
                        return false;
                    }   
                }
                else if (containsSearch)
                {
                    if (!vals[pair.Key].ToString().Contains(pair.Value))
                    {
                        return false;
                    }      
                }
                else if (!(vals[pair.Key].ToString() == pair.Value))
                {
                    return false;
                }     
            }

            return true;
        }

        #endregion
    }
}