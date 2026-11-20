# Systemm namespace
The BYTES.NET.IO.System namespace build upon several System.IO classes and expands them with comfort functions.

## Classes
**AdapterInfo** stores information about system adapters.
**DriveInfo** stores information about system drives.
**MemoryInfo** Stores the amount of memory, and offers functions to converts memory types.
**SystemInfo** Combines the previous classes to give information about the system as a whole.

## Different behavior in different .NET versions
The code behaves mostly the same in different .NET versions. The only difference is the handling of RAM.
TO recieve the amount of total RAM on the system, we use three different methods:

1. NETFULL
We use `Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;` to recieve the amount of RAM.

2. .NET 6.0-windows, 8.0-windows, 10.0-windows
We use `ObjectQuery` to recieve the information from `Win32_ComputerSystem` and search for `TotalPhysicalRAM` using `ManagementObjectSearcher`.

3. As a fallback, we use `GC.GetGCMemoryInfo().TotalAvailableMemoryBytes`.

Keep this difference in behavior in mind when using `BYTES.NET.IO.System.DriveInfo`.