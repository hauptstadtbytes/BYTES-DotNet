// Use standard .Net amespaces
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

// Use Dependencies
using BYTES.NET.IO;
using Novell.Directory.Ldap;

internal class NovellExampleCLI
{
    #region Main Program

    /// <summary>
    /// CLI to showcase how to use Novell with our UserInfo class
    /// </summary>
    static async Task Main()
    {
        Console.Write("Host: ");
        string host = Console.ReadLine()!;

        Console.Write("Username: ");
        string username = Console.ReadLine()!;

        Console.Write("Password: ");
        string password = GetPasswordInput();


        using var conn = await ConnectAsync(host);

        Console.WriteLine("\n---Domain---\n");

        string? baseDn = await GetBaseDnAsync(conn);
        Console.WriteLine(baseDn + "\n");

        string domain = formatDomain(baseDn);
        Console.WriteLine(domain + "\n");

        Console.WriteLine("---Authenticate---\n");
        UserInfo user = new UserInfo(username, password, domain);

        bool authenticated = await AuthenticateAsync(conn, host, user);
        if (!authenticated)
            return;

        // turn Referral Following on to prevent errors during search of AD
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


    #region Methods

    /// <summary>
    /// Connect to service
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    static async Task<LdapConnection> ConnectAsync(string host)
    {
        var conn = new LdapConnection();
        await conn.ConnectAsync(host, LdapConnection.DefaultPort);

        return conn;
    }

    /// <summary>
    /// authenticate the user with username and password
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="host"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    static async Task<bool> AuthenticateAsync(LdapConnection conn, string host, UserInfo user)
    {
        try
        {
            if (host == "localhost")
                await conn.BindAsync(user.Name, user.Password);
            else
                await conn.BindAsync(user.FullName, user.Password);
            
            Console.WriteLine($"Login successful for {user.Name}\n");

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
    /// <param name="conn"></param>
    /// <returns></returns>
    static async Task<string?> GetBaseDnAsync(LdapConnection conn)
    {
        LdapEntry root = await conn.ReadAsync("", new[]{"defaultNamingContext", "namingContexts", "rootDomainNamingContext"});

        string domain = null;

        if (root.GetAttributeSet().ContainsKey("defaultNamingContext"))
            domain = root.Get("defaultNamingContext").StringValue;

        if (root.GetAttributeSet().ContainsKey("rootDomainNamingContext"))
            domain = root.Get("rootDomainNamingContext").StringValue;

        if (root.GetAttributeSet().ContainsKey("namingContexts"))
            domain = root.Get("namingContexts").StringValue;

        return domain;
    }

    /// <summary>
    /// format the domain to be easier readable
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    static string formatDomain(string domain)
    {
        string formattedDomain = string.Join(".",
            domain.Split(",")
            .Where(x => x.StartsWith("DC=", StringComparison.OrdinalIgnoreCase))
            .Select(x => x.Substring(3)));

        return formattedDomain;
    }

    /// <summary>
    /// Search the service for all users
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="host"></param>
    /// <param name="baseDn"></param>
    static async void SearchWithFilter(LdapConnection conn, string host, string? baseDn)
    {
        if (string.IsNullOrWhiteSpace(baseDn))
            return;

        ILdapSearchResults results = null;

        results = await conn.SearchAsync(baseDn, LdapConnection.ScopeSub, "(&(objectCategory=person)(objectClass=user))", new[] { "displayName", "mail" }, false);

        await foreach (LdapEntry entry in results)
        {
            if (entry.GetAttributeSet().ContainsKey("mail"))
            {
                Console.WriteLine(entry.Dn.Split(",")[0].Split("=")[1]);
                Console.WriteLine("mail: " + entry.GetAttributeSet().GetAttribute("mail")?.StringValue + "\n");
            }
        }
    }

    /// <summary>
    /// Return all properties
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="baseDn"></param>
    static async void GetAllProperties(LdapConnection conn, string? baseDn)
    {
        if (string.IsNullOrWhiteSpace(baseDn))
            return;

        ILdapSearchResults results = await conn.SearchAsync(baseDn, LdapConnection.ScopeBase, "(objectClass=*)", null, false);

        await foreach (LdapEntry entry in results)
        {
            PrintProperties(entry);
            break;
        }
    }

    #endregion


    #region Helper methods

    /// <summary>
    /// Print properties in a more readable format
    /// </summary>
    /// <param name="e"></param>
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
    /// hide password input in terminal
    /// </summary>
    /// <returns></returns>
    static string GetPasswordInput()
    {
        StringBuilder input = new StringBuilder();
        while (true)
        {
            var b = Console.ReadKey(true);
            Console.Write("*");
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