//import (default) DotNet namespaces
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace BYTES.NET.IO.System
{
    /// <summary>
    /// Class for collecting drive information
    /// </summary>
    public class DriveInfo
    {
        #region private properties

        private readonly global::System.IO.DriveInfo _drive;
        private readonly bool _isRemovable;
        private readonly MemoryInfo _totalSpace;
        private readonly MemoryInfo _freeSpace;

        #endregion


        #region public properties

        public global::System.IO.DriveType Type => _drive.DriveType;

        public bool IsRemovable => _isRemovable;

        public string Path => _drive.Name;

        public bool IsReady => _drive.IsReady;

        /// <summary>
        /// Returns total disk space
        /// </summary>
        public MemoryInfo TotalSpace()
        {
            if (!IsReady)
                return new MemoryInfo(0);

            return _totalSpace;
        }

        /// <summary>
        /// Returns free disk space
        /// </summary>
        public MemoryInfo FreeSpace()
        {
            if (!IsReady)
                return new MemoryInfo(0);

            return _freeSpace;
        }
        #endregion


        #region constructors

        /// <summary>
        /// Create a Drive instance from a DriveInfo object.
        /// </summary>
        public DriveInfo(global::System.IO.DriveInfo drive)
        {
            _drive = drive;
            _isRemovable = CheckIfRemovable(drive);
            (_freeSpace, _totalSpace) = GetDriveSpace(drive.Name);
        }

        /// <summary>
        /// Create a Drive instance using a drive letter (e.g., "C").
        /// </summary>
        public DriveInfo(string letter) : this(new global::System.IO.DriveInfo(letter))
        {
        }

        #endregion


        #region private methods

        /// <summary>
        /// Gets the available space and total drive size
        /// </summary>
        private (MemoryInfo free, MemoryInfo total) GetDriveSpace(string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
                return (new MemoryInfo(0), new MemoryInfo(0));

            if (!folderName.EndsWith("\\"))
                folderName += "\\";

            ulong freeBytes = (ulong) _drive.AvailableFreeSpace;
            ulong totalBytes = (ulong) _drive.TotalSize;

            return (new MemoryInfo(freeBytes), new MemoryInfo(totalBytes));
        }

        /// <summary>
        /// Check if the Drive is removable
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        private bool CheckIfRemovable(global::System.IO.DriveInfo drive)
        {
            return drive.DriveType == global::System.IO.DriveType.Removable || drive.DriveType == global::System.IO.DriveType.CDRom;
        }

        #endregion
    }
}
