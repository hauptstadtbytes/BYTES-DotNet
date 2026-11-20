# Getting Started
This short documentation shows how to get started using FluentFTP.

## Connect to an FTP server
To connect to an FTP server, you need the host adress, a username and the coresponding password. We recommend using the BYTES.NET.IO.UserInfo class to safe user information. The following snippet shows how to save the given username and password into an object of that class.

```csharp
using BYTES.NET.IO;
UserInfo user = new UserInfo(username, pass);
```
By using `user.Name` and `user.Password` you can retrieve the saved data.
To connect to the FTP server, you can choose to either connect to the standard port, or connect to a different port, depending on the setup of the server.

Connect to the standard port:
`FTPclient client = new FtpClient(host, user.Name, user.Password, 2121);`

Connect to a different port (port needs to be an integer):
`FTPclient client = new FtpClient(host, user.Name, user.Password, port);`

## Disconnecting from the server
Once an established connection is not needed anymore, the client can be disconnected. To do so, use
`client.Disconnect();`

## Get all files from the server
After connecting to a server, you can query the server for files. 

```csharp
FtpListItem[] items;
items = client.GetListing("/");
```

## Download/Upload Files
To download files, use `Download()`:
`client.DownloadFile(@"C:\MyVideo.mp4");`

To upload files, use `Upload()`:
`client.UploadFile(@"C:\MyVideo.mp4");`