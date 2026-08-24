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
        
        public Dictionary<DriveType, List<DriveInfo>> Drives { get => GetClusteredDrives(); }

        public string Domain
        {
            get
            {
                var localIPProperties = IPGlobalProperties.GetIPGlobalProperties();

                return localIPProperties.DomainName;
            }
        }

        public IO.UserInfo User
        {
            get
            {
                string userName = Environment.UserName;

                string userDomain = Environment.UserDomainName;

                return new UserInfo(userName, null, userDomain);
            }
        }

        /// <summary>
        /// Returns total physical RAM of the system
        /// </summary>
        public MemoryInfo Memory()
        {
            ulong totalRAM = 0;

            #if NETFULL
                            totalRAM = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;

            #elif NET6_0_WINDOWS || NET7_0_WINDOWS || NET8_0_WINDOWS || NET10_0_WINDOWS
                            ObjectQuery query = new ObjectQuery("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
                            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                            foreach (ManagementObject mo in searcher.Get())
                            {
                                totalRAM = (ulong)mo["TotalPhysicalMemory"];
                            }

            #else
                        //Fallback using total amount of available bytes used by the garbage collector
                        totalRAM = (ulong)GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
            #endif

            return new MemoryInfo(totalRAM);
        }

        #endregion


        #region public methods

        /// <summary>
        /// Overwrite standard method to get adapters
        /// </summary>
        public AdapterInfo[] GetAdapters(NetworkInterfaceType adapterType)
        {
            if (Adapters.TryGetValue(adapterType, out var adapters))
            {
                return adapters.ToArray();
            } else {
                return Array.Empty<AdapterInfo>();
            }
        }

        /// <summary>
        /// Overwrite standard method to get drives
        /// </summary>
        public DriveInfo[] GetDrives(DriveType driveType)
        {
            if (Drives.TryGetValue(driveType, out var drives))
            {
                return drives.ToArray();
            }
            else
            {
                return Array.Empty<DriveInfo>();
            }
        }

        #endregion


        #region private methods

        /// <summary>
        /// Returns adapters grouped by type
        /// </summary>
        private Dictionary<NetworkInterfaceType, List<AdapterInfo>> GetClusteredAdapters()
        {
            var output = new Dictionary<NetworkInterfaceType, List<AdapterInfo>>();
            foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                var typeValue = adapter.NetworkInterfaceType;

                if (!Enum.IsDefined(typeof(NetworkInterfaceType), typeValue))
                {
                    // Create a UnknownType in case an adapter has an unknown Enum
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

        /// <summary>
        /// Returns drives grouped by type
        /// </summary>
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
