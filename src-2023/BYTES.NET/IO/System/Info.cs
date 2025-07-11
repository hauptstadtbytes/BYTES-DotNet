using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using BYTES.NET.IO.Formatter;

namespace BYTES.NET.IO.System
{
    public class Info
    {
        #region public properties

        public string Name
        {
            get { return Dns.GetHostName(); }
        }
        public string Domain
        {
            get
            {
                var localIPProperties = IPGlobalProperties.GetIPGlobalProperties();
                return localIPProperties.DomainName;
            }
        }

        public double Memory(string displayUnit = "GB", bool fullUnitsOnly = false)
        {
        #if NETFRAMEWORK || NET6_0_WINDOWS || NET7_0_WINDOWS || NET8_0_WINDOWS
            ObjectQuery query = new ObjectQuery("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ulong totalMemory = 0;

            foreach (ManagementObject mo in searcher.Get())
            {
                totalMemory = (ulong)mo["TotalPhysicalMemory"];
            }
            return Formatter.Formatter.FormatMemory(totalMemory, displayUnit, fullUnitsOnly);
        #else
            // Optional: provide fallback for non-Windows frameworks or throw exception
            return 15.6; // or throw new PlatformNotSupportedException();
        #endif
        }

        public int Processors
        {
            get
            {
                return Environment.ProcessorCount;
            }
        }


        public Dictionary<NetworkInterfaceType, List<Adapter>> Adapters
        {
            get { return GetClusteredAdapters(); }
        }
        public Adapter[] GetAdapters(NetworkInterfaceType adapterType) =>
            Adapters.TryGetValue(adapterType, out var adapters) ? adapters.ToArray() : Array.Empty<Adapter>();


        public User.Info CurrentUser
        {
            get
            {
                string userName = Environment.UserName;
                string userDomain = Environment.UserDomainName;
                return new User.Info(userName, null, userDomain);
            }
        }

        public Dictionary<DriveType, List<Drive>> Drives
        {
            get { return GetClusteredDrives(); }
        }

        public Drive[] GetDrives(DriveType driveType) =>
            Drives.TryGetValue(driveType, out var drives) ? drives.ToArray() : Array.Empty<Drive>();

        #endregion

        #region private methods

        private Dictionary<NetworkInterfaceType, List<Adapter>> GetClusteredAdapters()
        {
            var output = new Dictionary<NetworkInterfaceType, List<Adapter>>();
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
                        adapterList = new List<Adapter>();
                        output[UnknownType] = adapterList;
                    }
                    adapterList.Add(new Adapter(adapter));
                    continue;
                }

                if (!output.TryGetValue(typeValue, out var list))
                {
                    list = new List<Adapter>();
                    output[typeValue] = list;
                }
                list.Add(new Adapter(adapter));
            }
            return output;
        }

        private Dictionary<DriveType, List<Drive>> GetClusteredDrives()
        {
            var output = new Dictionary<DriveType, List<Drive>>();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (!output.TryGetValue(drive.DriveType, out var driveList))
                {
                    driveList = new List<Drive>();
                    output[drive.DriveType] = driveList;
                }
                driveList.Add(new Drive(drive));
            }
            return output;
        }
        #endregion
    }
}
