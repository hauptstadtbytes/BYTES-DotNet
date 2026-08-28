using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.Network
{
    /// <summary>
    /// Saves information about a RemoteRessource from a network share
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class RemoteRessource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string LocalName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string RemoteName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Provider;
    }

    /// <summary>
    /// Defines how/ where a ressource is registered
    /// </summary>
    public enum ResourceScope : int
    {
        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    };

    /// <summary>
    /// Defines the type of network ressource being connected to
    /// </summary>
    public enum ResourceType : int
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    }

    /// <summary>
    /// Specifies how the ressource should be displayed/ classified
    /// </summary>
    public enum ResourceDisplaytype : int
    {
        Generic = 0x0,
        Domain = 0x01,
        Server = 0x02,
        Share = 0x03,
        File = 0x04,
        Group = 0x05,
        Network = 0x06,
        Root = 0x07,
        Shareadmin = 0x08,
        Directory = 0x09,
        Tree = 0x0a,
        Ndscontainer = 0x0b
    }
}
