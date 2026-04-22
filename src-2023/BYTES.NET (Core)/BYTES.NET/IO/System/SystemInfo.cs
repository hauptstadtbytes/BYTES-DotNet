//import (default) DotNet namespaces
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace BYTES.NET.IO.System
{
    /// <summary>
    /// Collects Information about the System 
    /// (RAM, Hostname, IP, Processors, Drives, Adapters, User)
    /// </summary>
    public class SystemInfo
    {
        #region public properties

        public string Name { get => Dns.GetHostName(); }

        public int Processors { get => Environment.ProcessorCount; }

        public Dictionary<NetworkInterfaceType, List<AdapterInfo>> Adapters { get => GetClusteredAdapters(); }
        
        public AdapterInfo[] GetAdapters(NetworkInterfaceType adapterType) =>
            Adapters.TryGetValue(adapterType, out var adapters) ? adapters.ToArray() : Array.Empty<AdapterInfo>();

        public Dictionary<DriveType, List<DriveInfo>> Drives { get => GetClusteredDrives(); }

        public DriveInfo[] GetDrives(DriveType driveType) =>
            Drives.TryGetValue(driveType, out var drives) ? drives.ToArray() : Array.Empty<DriveInfo>();

        public string Domain
        {
            get
            {
                var localIPProperties = IPGlobalProperties.GetIPGlobalProperties();
                return localIPProperties.DomainName;
            }
        }

        public MemoryInfo Memory()
        {
            ulong totalRAM = 0;

            #if NETFULL
                totalRAM = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory; //returns bytes

            #elif NETFRAMEWORK || NET6_0_WINDOWS || NET7_0_WINDOWS || NET8_0_WINDOWS
                ObjectQuery query = new ObjectQuery("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                foreach (ManagementObject mo in searcher.Get())
                {
                    totalRAM = (ulong)mo["TotalPhysicalMemory"];
                }

            #else
                //Fallback
                totalRAM = (ulong) GC.GetGCMemoryInfo().TotalAvailableMemoryBytes; //returns bytes
            #endif

            return new MemoryInfo(totalRAM);
        }

        public IO.UserInfo CurrentUser
        {
            get
            {
                string userName = Environment.UserName;
                string userDomain = Environment.UserDomainName;
                return new UserInfo(userName, null, userDomain);
            }
        }

        #endregion


        #region private methods

        private Dictionary<NetworkInterfaceType, List<AdapterInfo>> GetClusteredAdapters()
        {
            var output = new Dictionary<NetworkInterfaceType, List<AdapterInfo>>();
            foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                var typeValue = adapter.NetworkInterfaceType;

                if (!Enum.IsDefined(typeof(NetworkInterfaceType), typeValue))
                {
                    // Handle unknown or invalid enum values here:
                    // For example, skip or group under a special 'Unknown' key

                    // Option 1: Skip this adapter
                    continue;

                    // Option 2: Use a fallback key — you might create a special enum value for unknown or cast int to enum
                    // but since enum is fixed, better to use a separate dictionary key
                    const NetworkInterfaceType UnknownType = (NetworkInterfaceType)(-1); // your own "unknown" key
                    if (!output.TryGetValue(UnknownType, out var adapterList))
                    {
                        adapterList = new List<AdapterInfo>();
                        output[UnknownType] = adapterList;
                    }
                    adapterList.Add(new AdapterInfo(adapter));
                    continue;
                }

                if (!output.TryGetValue(typeValue, out var list))
                {
                    list = new List<AdapterInfo>();
                    output[typeValue] = list;
                }
                list.Add(new AdapterInfo(adapter));
            }
            return output;
        }

        private Dictionary<DriveType, List<DriveInfo>> GetClusteredDrives()
        {
            var output = new Dictionary<DriveType, List<DriveInfo>>();
            foreach (var drive in global::System.IO.DriveInfo.GetDrives())
            {
                if (!output.TryGetValue(drive.DriveType, out var driveList))
                {
                    driveList = new List<DriveInfo>();
                    output[drive.DriveType] = driveList;
                }
                driveList.Add(new DriveInfo(drive));
            }
            return output;
        }
        #endregion
    }
}
