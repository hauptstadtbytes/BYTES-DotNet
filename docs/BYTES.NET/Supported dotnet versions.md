# Supported versions
Our framework currently supports:
**.NET Framework**
- 4.8

**.NET Core**
- 6.x
- 8.x
- 10.x

# Breaking changes between the versions
The following changes differ between .NET 8.x and .NET 10.x:

## Networking
In .NET 9.0, changed to HttpClient have been made. The standard requirement level of `server.port` has been changed from *required conditionally* to *required*. This might break monitoring software sending queries. 

This change is behavioral. 

This affect:
- .NET HttpClient class, that we suggest to be used instead of our legacy HttpClient class

Further reading:
- https://learn.microsoft.com/en-au/dotnet/core/compatibility/networking/9.0/server-port-attribute

## DriveInfo
In .NET 10.0 under Linux, when using System.IO.DriveInfo, it returns Linux filesystem types now. This allows a more granular representation of the filesystem type.

This change is behavioral.
This affects:
- BYTES.NET.IO.System

Further reading:
- https://learn.microsoft.com/mt-mt/dotnet/core/compatibility/core-libraries/10.0/driveinfo-driveformat-linux
