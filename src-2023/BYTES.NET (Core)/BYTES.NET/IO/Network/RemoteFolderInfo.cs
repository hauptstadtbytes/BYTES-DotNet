using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace BYTES.NET.IO.Network
{
    /// <summary>
    /// 
    /// </summary>
    public class RemoteFolderInfo
    {
        #region private properties

        private string _path;
        private RemoteConnection _conn;

        #endregion


        #region public properties

        public string Path { get => _path;  }

        #endregion


        #region public methods

        /// <summary>
        /// contrucor method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="user"></param>
        public RemoteFolderInfo(string path, UserInfo user = null)
        {
            if (!path.EndsWith(@"\"))
            {
                path = path + @"\";
            }

            _path = path;

            if(user != null)
            {
                _conn = new RemoteConnection(_path, user);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public FileInfo GetFileInfo(string path)
        {
            path = _path + ParsePath(path);

            try
            {
                if(_conn == null)
                {
                    return new FileInfo(path);
                }
                else
                {
                    using (_conn)
                    {
                        return new FileInfo(path);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get FileInfo for " + path + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DirectoryInfo GetFolderInfo(string path = null)
        {
            if(path != null)
            {
                path = _path + ParsePath(path);
            }
            else
            {
                path = _path;
            }

            try
            {
                if(_conn == null)
                {
                    return new DirectoryInfo(path);
                }
                else
                {
                    using (_conn)
                    {
                        return new DirectoryInfo(path);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to getDirectoryInfo for " + path + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DirectoryInfo[] GetFolders(string path = null)
        {
            if (path != null)
            {
                path = ParsePath(path);
            }

            return this.GetFolderInfo(path).GetDirectories();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public FileInfo[] GetFiles(string path = null, string searchPattern = null)
        {
            if (path != null)
            {
                path = ParsePath(path);
            }

            if(searchPattern != null)
            {
                return this.GetFolderInfo(path).GetFiles(searchPattern);
            }
            else
            {
                return this.GetFolderInfo(path).GetFiles();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsReadable()
        {
            try
            {
                if(_conn == null)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(_path);
                }
                else
                {
                    using (_conn)
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(_path);
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FileExists(string path)
        {
            path = _path + ParsePath(path);
             if(_conn == null)
            {
                return File.Exists(path);
            }
            else
            {
                using (_conn)
                {
                    return File.Exists(path);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FolderExists(string path = null)
        {
            if (path != null)
            {
                path = _path + ParsePath(path);
            }
            else
            {
                path = _path;
            }

            if(_conn == null)
            {
                return Directory.Exists(path);
            }
            else
            {
                using (_conn)
                {
                    return Directory.Exists(path);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Byte[] ReadBytes(string path)
        {
            path = _path + ParsePath(path);

            try
            {
                if(_conn == null)
                {
                    return File.ReadAllBytes(path);
                }
                else
                {
                    using (_conn)
                    {
                        return File.ReadAllBytes(path);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Unable to read bytes from " + path + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <exception cref="Exception"></exception>
        public void CopyFileTo(string source, string destination)
        {

            source = _path + ParsePath(source);

            try
            {
                if (_conn == null)
                    File.Copy(source, destination);
                else
                    using (_conn)
                    {
                        File.Copy(source, destination);
                    }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to copy file from " + source + " to " + destination + ": " + ex.Message, ex);
            }
        }

        #endregion


        #region private methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ParsePath(string path)
        {
            if (path.StartsWith(_path))
            {
                path = path.Replace(_path, "");
            }

            if (path.StartsWith(@"\"))
            {
                path = path.Substring(1);
            }
            return path;
        }

        #endregion
    }
}
