using BYTES.NET.WPF.MVVM;
using IO.System;
using BYTES.NET.WPF.App.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.WPF.App.ViewModels
{
    public class SystemVM : ViewModel
    {
        private readonly SystemView _systemView = new SystemView();
        private readonly Info _systemInfo = new Info();

        public string HostName => _systemInfo.Name;
        public string DomainName
        {
            get
            {
                if (string.IsNullOrEmpty(_systemInfo.Domain))
                    return "Not Found";
                return _systemInfo.Domain;
            }
        }
        public double MemoryGB => _systemInfo.Memory("GB", true); 

        public int ProcessorCount => _systemInfo.Processors;
        public string CurrentUser => _systemInfo.CurrentUser.FullName;
        public Dictionary<NetworkInterfaceType, List<Adapter>> Adapters => _systemInfo.Adapters;
        public Dictionary<DriveType, List<Drive>> Drives => _systemInfo.Drives;

        public Dictionary<DriveType, List<object>> FormattedDrives
        {
            get
            {
                var result = new Dictionary<DriveType, List<object>>();

                foreach (var driveGroup in _systemInfo.Drives)
                {
                    var driveType = driveGroup.Key;
                    var drives = driveGroup.Value;

                    var formattedDrives = new List<object>();

                    foreach (var drive in drives)
                    {
                        formattedDrives.Add(new
                        {
                            Path = drive.Path,
                            Type = drive.Type,
                            TotalSpace = drive.IsReady ? $"{drive.TotalSpace():0.##} GB" : "Not Ready", //format specifier (:0.##)
                            FreeSpace = drive.IsReady ? $"{drive.FreeSpace():0.##} GB" : "Not Ready"
                        });
                    }

                    result.Add(driveType, formattedDrives);
                }

                return result;
            }
        }


        public SystemView View => _systemView;

        public SystemVM()
        {
            _systemView.DataContext = this;
        }
    }
}
