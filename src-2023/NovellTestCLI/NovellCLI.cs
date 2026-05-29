using Novell.Directory.Ldap;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

Console.Write("Host: ");
string host = Console.ReadLine()!;

Console.Write("Username: ");
string username = Console.ReadLine()!;

Console.Write("Password: ");
string password = Console.ReadLine()!;


//Setup connection
using var conn = new LdapConnection();
await conn.ConnectAsync(host, LdapConnection.DefaultPort);

// Auth user with password
Console.WriteLine("---Authenticate---");
try
{
    await conn.BindAsync(username, password);
    Console.WriteLine("Login successful for " + username);
}
catch (LdapException ex) when (ex.ResultCode == LdapException.InvalidCredentials)
{
    Console.WriteLine("Invalid credentials");
    return;
}

//Get Domain name
Console.WriteLine("---Domain---");
LdapEntry root = await conn.ReadAsync(
    "",
    new[]
    {
        "namingContexts",
        "defaultNamingContext",
        "*",
        "+"
    });


string? baseDn = null;

if (root.GetAttributeSet().ContainsKey("defaultNamingContext"))
{
    baseDn = root.Get("defaultNamingContext").StringValue;
}
else if (root.GetAttributeSet().ContainsKey("namingContexts"))
{
    baseDn = root.Get("namingContexts").StringValue;
}

Console.WriteLine(baseDn);

// search with filter
// string @base, int scope, string filter, string[] attrs, bool typesOnly, CancellationToken ct = default
Console.WriteLine("---Search with filter (objectClass=user)---");
ILdapSearchResults res = await conn.SearchAsync(baseDn, LdapConnection.ScopeSub, "(objectClass=person)", ["mail"], typesOnly: false);

await foreach (LdapEntry entry in res)
{
    Console.WriteLine(entry.Dn);
    LdapAttributeSet attrb = entry.GetAttributeSet();
    Console.WriteLine("mail: " + attrb.GetAttribute("mail")?.StringValue);
}

// get properties of a single entry
Console.WriteLine("---List all properties---");
ILdapSearchResults singleres = await conn.SearchAsync(baseDn, LdapConnection.ScopeBase, "(objectClass=*)", attrs: null, typesOnly: false);

await foreach (LdapEntry singleEntry in singleres)
{
    PrintProperties(singleEntry);
    break;
}

// helper print function

static void PrintProperties(LdapEntry e)
{
    foreach (LdapAttribute a in e.GetAttributeSet())
    {
        foreach (string value in a.StringValueArray)
        {
            Console.WriteLine($"{a.Name}: {value}");
        }
    }
}