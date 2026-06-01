using Novell.Directory.Ldap;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class NovellExampleCLI
{
    static async Task Main()
    {
        Console.Write("Host: ");
        string host = Console.ReadLine()!;

        Console.Write("Username: ");
        string username = Console.ReadLine()!;

        Console.Write("Password: ");
        string password = Console.ReadLine()!;

        using var conn = await ConnectAsync(host);

        bool authenticated = await AuthenticateAsync(conn, username, password);
        if (!authenticated)
            return;

        string? baseDn = await GetBaseDnAsync(conn);
        Console.WriteLine(baseDn);

        await SearchWithFilter(conn, baseDn);

        await PrintAllProperties(conn, baseDn);
    }

    //connect to service
    static async Task<LdapConnection> ConnectAsync(string host)
    {
        var conn = new LdapConnection();
        await conn.ConnectAsync(host, 389);

        return conn;
    }

    //authenticate user
    static async Task<bool> AuthenticateAsync(LdapConnection conn, string username, string password)
    {
        Console.WriteLine("---Authenticate---\n");
        try
        {
            await conn.BindAsync(username, password);
            Console.WriteLine($"Login successful for {username}");

            return true;
        }
        catch (LdapException ex)
        {
            Console.WriteLine($"Login failed. {ex.Message}");
            return false;
        }
    }

    static async Task<string?> GetBaseDnAsync(LdapConnection conn)
    {
        Console.WriteLine("---Domain---\n");

        LdapEntry root = await conn.ReadAsync("", new[]{"defaultNamingContext", "namingContexts", "rootDomainNamingContext"});

        if (root.GetAttributeSet().ContainsKey("defaultNamingContext"))
            return root.Get("defaultNamingContext").StringValue;

        if (root.GetAttributeSet().ContainsKey("rootDomainNamingContext"))
            return root.Get("rootDomainNamingContext").StringValue;

        if (root.GetAttributeSet().ContainsKey("namingContexts"))
            return root.Get("namingContexts").StringValue;

        return null;
    }

    //Search with filter
    static async Task SearchWithFilter(LdapConnection conn, string? baseDn)
    {
        if (string.IsNullOrWhiteSpace(baseDn))
            return;

        Console.WriteLine("---Search with filter (objectClass=person)---");

        ILdapSearchResults results =
            await conn.SearchAsync(baseDn, LdapConnection.ScopeSub, "(&(objectCategory=person)(objectClass=user))", new[] { "mail" }, false);

        await foreach (LdapEntry entry in results)
        {
            Console.WriteLine(entry.Dn);
            Console.WriteLine("mail: " + entry.GetAttributeSet().GetAttribute("mail")?.StringValue);
        }
    }

    //get and print all properties for entry
    static async Task PrintAllProperties(LdapConnection conn, string? baseDn)
    {
        if (string.IsNullOrWhiteSpace(baseDn))
            return;

        Console.WriteLine("---List all properties---");

        ILdapSearchResults results = await conn.SearchAsync(baseDn, LdapConnection.ScopeBase, "(objectClass=*)", null, false);

        await foreach (LdapEntry entry in results)
        {
            PrintProperties(entry);
            break;
        }
    }

    //helper class to print properties
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
}