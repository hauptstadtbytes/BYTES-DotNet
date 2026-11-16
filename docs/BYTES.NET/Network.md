# Network Namespace

The functions in the Network namespace make it possible to connect to a file share as well as perform [user impersonation](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/impersonate-another-user). 

The namespace consists of:
 - **RemoteConnection.cs**: A class that stores information about the connection to a file share
 - **RemoteFolderInfo.cs**: A class that stores information about the file share itself and the data it contains
 - **RemoteRessource.cs**: A class that stores information about the resources of the file share 

Login credentials are stores in the BYTES.NET **UserInfo** class, part of the BYTES.NET.IO namespace.

In the BYTES.NET.Tests project, usage examples for the Network namespace can be found under IO/Network/TestRemoteFolderInfo.cs. It shows how to use the functions to connect to a file share and to impersonate a user.

# HTTPClient
The **HTTPClientCLI** project contains an implementation showing how to use the .net HTTPClient class to communicate with websites via HTTP requests.
