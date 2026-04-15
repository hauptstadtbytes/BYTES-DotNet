//import (default) DotNet namespaces
using System;
using DotNet = global::System.IO;
using System.Runtime.InteropServices;

namespace BYTES.NET.IO.System
{
    /// <summary>
    /// Class for collecting drive information
    /// </summary>
    /// 
    /// <remarks>
    /// Based on the DotNet DriveInfo class
    /// </remarks>
    public class DriveInfo
    {
        #region WinAPI
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool GetDiskFreeSpaceEx(
            string lpDirectoryName,
            out ulong lpFreeBytesAvailable,
            out ulong lpTotalNumberOfBytes,
            out ulong lpTotalNumberOfFreeBytes
        );
        #endregion

        #region Private Fields
        private readonly DotNet.DriveInfo _drive;
        private readonly bool _isRemovable;
        private readonly ulong _totalSpace;
        private readonly ulong _freeSpace;
        #endregion

        #region Public Properties
        public DotNet.DriveType Type => _drive.DriveType;
        public bool IsRemovable => _isRemovable;
        public string Path => _drive.Name;
        public bool IsReady => _drive.IsReady;

        /// <summary>
        /// Returns total disk space in specified unit (default: GB).
        /// </summary>
        public double TotalSpace(string displayUnit = "GB", bool fullUnitsOnly = false)
        {
            if (!IsReady)
                return 0;

            return Formatter.Formatter.FormatMemory(_totalSpace, displayUnit, fullUnitsOnly);
        }

        /// <summary>
        /// Returns free disk space in specified unit (default: GB).
        /// </summary>
        public double FreeSpace(string displayUnit = "GB", bool fullUnitsOnly = false)
        {
            if (!IsReady)
                return 0;

            return Formatter.Formatter.FormatMemory(_freeSpace, displayUnit, fullUnitsOnly);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a Drive instance from a DriveInfo object.
        /// </summary>
        public DriveInfo(DotNet.DriveInfo drive)
        {
            _drive = drive;
            _isRemovable = CheckIfRemovable(drive);
            (_freeSpace, _totalSpace) = GetDriveSpace(drive.Name);
        }

        /// <summary>
        /// Create a Drive instance using a drive letter (e.g., "C").
        /// </summary>
        public DriveInfo(string letter) : this(new DotNet.DriveInfo(letter))
        {
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the available space and total drive size
        /// </summary>
        private (ulong free, ulong total) GetDriveSpace(string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
                return (0, 0);

            if (!folderName.EndsWith("\\"))
                folderName += "\\";

            if (GetDiskFreeSpaceEx(folderName, out ulong freeBytes, out ulong totalBytes, out _))
                return (freeBytes, totalBytes);

            return (0, 0);
        }

        private bool CheckIfRemovable(DotNet.DriveInfo drive)
        {
            return drive.DriveType == DotNet.DriveType.Removable || drive.DriveType == DotNet.DriveType.CDRom;
        }
        #endregion
    }
}
