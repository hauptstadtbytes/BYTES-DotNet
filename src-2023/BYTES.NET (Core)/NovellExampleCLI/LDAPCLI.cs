// Use standard .Net amespaces
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Use Dependencies
using BYTES.NET.IO;
using Novell.Directory.Ldap;


/// <summary>
/// Connect user to AD, return all users with email and name 
/// Uses Novell
/// </summary>
internal class LDAPCLI
{
    #region main method

    /// <summary>
    /// CLI to showcase how to use Novell
    /// Uses BYTES.NET UserInfo class to save user information
    /// </summary>
    static async Task Main()
    {
        Console.Write("Host: ");
        string host = Console.ReadLine()!;

        Console.Write("Username: ");
        string username = Console.ReadLine()!;
        string password = GetPasswordInput();

        using var conn = await ConnectAsync(host);

        string? baseDn = await GetBaseDnAsync(conn);
        string domain = formatDomain(baseDn);

        Console.WriteLine("---Authenticate---\n");
        UserInfo user = new UserInfo(username, password, domain);

        bool authenticated = await AuthenticateAsync(conn, host, user);
        if (!authenticated)
        {
            return;
        }

        LdapSearchConstraints cons = conn.SearchConstraints;
        cons.ReferralFollowing = true;
        conn.Constraints = cons;

        Console.WriteLine("---Get all users with email and name---\n");
        SearchWithFilter(conn, host, baseDn);

        Console.WriteLine("---List all entries---\n");
        GetAllProperties(conn, baseDn);

        Console.WriteLine("\n---OUTPUT END---\n\n");
    }

    #endregion


    #region static methods

    /// <summary>
    /// Connect to service
    /// </summary>
    static async Task<LdapConnection> ConnectAsync(string host)
    {
        var conn = new LdapConnection();
        await conn.ConnectAsync(host, LdapConnection.DefaultPort);

        return conn;
    }

    /// <summary>
    /// Authenticate the user with username and password
    /// </summary>
    static async Task<bool> AuthenticateAsync(LdapConnection conn, string host, UserInfo user)
    {
        try
        {
            if (host == "localhost")
            {
                await conn.BindAsync(user.Name, user.Password);
            }
            else
            {
                await conn.BindAsync(user.FullName, user.Password);
            }

            Console.WriteLine($"Login successful for {user.Name} for domain {user.Domain}\n");

            return true;
        }
        catch (LdapException ex)
        {
            Console.WriteLine($"Login failed for {user.Name}. {ex.Message}\n");
            return false;
        }
    }

    /// <summary>
    /// Get the name of the domain
    /// </summary>
    static async Task<string?> GetBaseDnAsync(LdapConnection conn)
    {
        LdapEntry root = await conn.ReadAsync("", new[]{"defaultNamingContext", "namingContexts", "rootDomainNamingContext"});

        string? domain = null;

        if (root.GetAttributeSet().ContainsKey("defaultNamingContext"))
        {
            domain = root.Get("defaultNamingContext").StringValue;
        }

        if (root.GetAttributeSet().ContainsKey("rootDomainNamingContext"))
        {
            domain = root.Get("rootDomainNamingContext").StringValue;
        }

        if (root.GetAttributeSet().ContainsKey("namingContexts"))
        {
            domain = root.Get("namingContexts").StringValue;
        }
            
        return domain;
    }

    /// <summary>
    /// Convenience function
    /// Format the domain to be more readable
    /// </summary>
    static string formatDomain(string domain)
    {
        string formattedDomain = string.Join(".",
            domain.Split(',')
            .Where(x => x.StartsWith("DC=", StringComparison.OrdinalIgnoreCase))
            .Select(x => x.Substring(3)));

        return formattedDomain;
    }

    /// <summary>
    /// Search the service for all users
    /// </summary>
    static async void SearchWithFilter(LdapConnection conn, string host, string? baseDn)
    {
        if (string.IsNullOrWhiteSpace(baseDn))
        {
            return;
        }

        ILdapSearchResults results = null;

        results = await conn.SearchAsync(baseDn, LdapConnection.ScopeSub, "(&(objectCategory=person)(objectClass=user))", new[] { "displayName", "mail" }, false);

        await foreach (LdapEntry entry in results)
        {
            if (entry.GetAttributeSet().ContainsKey("mail"))
            {
                Console.WriteLine(entry.Dn.Split(',')[0].Split('=')[1]);
                Console.WriteLine("mail: " + entry.GetAttributeSet().GetAttribute("mail")?.StringValue + "\n");
            }
        }
    }

    /// <summary>
    /// Return all properties
    /// </summary>
    static async void GetAllProperties(LdapConnection conn, string? baseDn)
    {
        if (string.IsNullOrWhiteSpace(baseDn))
        {
            return;
        }

        ILdapSearchResults results = await conn.SearchAsync(baseDn, LdapConnection.ScopeBase, "(objectClass=*)", null, false);

        await foreach (LdapEntry entry in results)
        {
            PrintProperties(entry);
            break;
        }
    }

    #endregion


    #region helper methods

    /// <summary>
    /// Print properties in a more readable format
    /// </summary>
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

    /// <summary>
    /// Hide password input in terminal
    /// </summary>
    static string GetPasswordInput()
    {
        StringBuilder input = new StringBuilder();
        Console.Write("Enter password....\n");
        while (true)
        {
            var b = Console.ReadKey(true);

            if (b.Key == ConsoleKey.Enter)
                break;
            if (b.Key == ConsoleKey.Backspace && input.Length > 0)
                input.Remove(input.Length - 1, 1);
            else if (b.Key != ConsoleKey.Backspace)
                input.Append(b.KeyChar);
        }
        return input.ToString();
    }

    #endregion
}