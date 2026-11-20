# Breaking Changes

To update and modernize the old codebase, some namespaces have been retired. For users of BYTES.NET version 1.x, these changes might break existing code when updating to newer versions of the framework. The specific changes are listed down below.

Unless specified, the reasons for retiring namespaces/ code is the same: The legacy code was years old. Rather than rewriting it, we decided to retire it. To still offer the same functionality, we evaluated an external library that offers the same, and in most cases more methods, to do the same things as our legacy code.
For external libraries, we have created a CLI under BYTES.NET/References, that shows how to use the package. 


| Namespace | Retired | Replaced by |
|------|---------|-------------|
| BYTES.NET.IO.FTP  | Legacy in-house FTP code | [FluentFTP](https://github.com/robinrodricks/FluentFTP) |
| BYTES.NET.IO.LDAP | Legacy in-house LDAP code | [Novell.Directory.Ldap.NETStandard](https://github.com/dsbenghe/Novell.Directory.Ldap.NETStandard) |
| BYTES.NET.IO.HTTP | Custom HTTP client class | `System.Net.Http.HttpClient` (built into .NET) |
| BYTES.NET.Collections.MSSQL  | Legacy in-house database access | [linq2db](https://github.com/linq2db/linq2db) |
 


## BYTES.NET.IO.FTP

### New library
We suggest using [FluentFTP](https://github.com/robinrodricks/FluentFTP) when working with FTP servers. The library is widely used and is still being maintained.

### Migrating legacy code
To migrate legacy code using the BYTES.NET.IO.FTP *namespace*, you need to install FluentFTP. 

1. Install FluentFTP
To install the package, you can run the following in a developer console within Visual Studio:

```
dotnet add package FluentFTP
dotnet add package SSH.NET
```

2. Replace usage of the old `FTP/Connection` class with the FluentFTP `AsyncFtpClient` class.
3. Review the connection settings. 

### How to use FluentFTP
Below is a short snippet showing hot to connect to an FTP server with FluentFTp and upload, as well as download some files. Keep in mind that you might need to change the path to the files depending on your setup and needs.

```csharp
using FluentFTP;
 
await using var client = new AsyncFtpClient("ftp.example.com", "user", "password", 21);
await client.Connect();
 
await client.UploadFile(@"C:\local\report.csv", "/remote/report.csv");
await client.DownloadFile(@"C:\local\data.csv", "/remote/data.csv");
 
await client.Disconnect();
```

### Further documentation
The documentation for FluentFTP can found under https://github.com/robinrodricks/FluentFTP/wiki.
An example project showcasing some of FluentFTPs capabilities is located in the BYTES.NET package, under References/FtpCLI.

## BYTES.NET.IO.LDAP
### New library
We suggest using [Novell.Directory.Ldap.NETStandard](https://github.com/dsbenghe/Novell.Directory.Ldap.NETStandard) when working with the Microsoft Active Directory. 

### Migrating legacy code
To migrate code using our legacy code, follow the steps below.

1. Install Novell using the command:

```
dotnet add package Novell.Directory.Ldap.NETStandard
```
2. Replace the use of the old BYTES.NET LDAP connection class with the Novell `LdapConnection` class.
3. Set the connection parameters (server, port, bind domain name) for the LdapConnection object.

### How to use

The following code shows how to connect to the AD via Novell, as well return all entries found and print them.

```csharp
using Novell.Directory.Ldap;
 
using var connection = new LdapConnection();
connection.ConnectAsync("ldap.example.com", 389);
connection.BindAsync("cn=admin,dc=example,dc=com", "password");
 
ILdapSearchResults results = await conn.SearchAsync(baseDn, LdapConnection.ScopeBase, "(objectClass=*)", null, false);

await foreach (LdapEntry entry in results)
{
    PrintProperties(entry);
    break;
}
 
connection.Disconnect();
```

### Further documentation
The documentation for Novell can found under https://github.com/dsbenghe/Novell.Directory.Ldap.NETStandard
An example project showcasing some of Novells capabilities is located in the BYTES.NET package, under References/LDAPCLI.

## Http
### New library
Instead of using the legacy Http/Client class, we recommend using the dotnet HTTPClient class. The built-in .NET `HttpClient` covers the same functionality, is maintained by Microsoft, and is better tested and more performant.

### Migrating legacy code
1. Replace all uses of BYTES.NET/Http/Client with `HttpClient`.

### How to use
The following code shows how to use the HttpClient to connect to a website and send a POST-request with a json body, as well as write the response onto the console.

```csharp
HttpClient client = new HttpClient();
// create the authorization header
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "ABC123");

string json = JsonSerializer.Serialize(new { token = "ABC123" });
StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

HttpResponseMessage response = await client.PostAsync("https://example.com", content);
Console.WriteLine(response);
```
### Further documentation
The documentation for the HttpClient can be found under https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-11.0
An example of how to use the HttpClient class to send more types of HTTP requests is located in the BYTES.NET package, under References/HTTPClientCLI.

## BYTES.NET.Collections.MSSQL
In addition to our code being outdated, the library we recommend instead is also faster, offers safer queries and has more capabilities than our legacy code.

### New library
We recommend using [linq2db](https://github.com/linq2db/linq2db). It is a powerful library offering methods to connect and work with several types of databases. 

### Migrating legacy code
1. Add the NuGet package (plus the ADO.NET provider for your database if needed):
```
dotnet add package linq2db
```
2. Define mapping classes for your tables.
3. Replace old query calls with `DataConnection` / LINQ queries.
4. Review any hand-written SQL, as it can still be executed via `db.Execute(...)` / `db.Query<T>(...)`.

### How to use
The following code shows how to set up a class to use as a table. It also shows how to connect to, query and insert new items into the database.

```csharp
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;
 
[Table("Users")]
public class User
{
    [PrimaryKey, Identity] public int Id { get; set; }
    [Column] public string Name { get; set; } = "";
    [Column] public bool IsActive { get; set; }
}
 
// Init
var options = new DataOptions().UseSqlServer("Server=...;Database=...;");
using var db = new DataConnection(options);
 
// Query
var activeUsers = db.GetTable<User>()
    .Where(u => u.IsActive)
    .OrderBy(u => u.Name)
    .ToList();
 
// Insert
db.Insert(new User { Name = "Jane", IsActive = true });
```

### Further documentation
The documentation for the linq2db can be found under https://linq2db.github.io/
An example of how to use the library to create tables, as well as work with them, is located in the BYTES.NET package, under References/LinqToDBCLI.