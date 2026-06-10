// Use standard .Net amespaces
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Use Dependencies
using BYTES.NET.IO;
using FluentFTP;

public class FtpCLI
{
    public static void Main()
    {
        Console.Write("Host: ");
        string host = Console.ReadLine()!;

        Console.WriteLine("Port (optional): ");
        string port = Console.ReadLine();

        Console.Write("Username: ");
        string username = Console.ReadLine()!;

        Console.Write("Password: ");
        string pass = GetPasswordInput();

        Console.WriteLine("\nConnecting...\n");

        UserInfo user = new UserInfo(username, pass);
        FTP(host, port, user);
    }

    #region methods

    /// <summary>
    /// Builds and FTP connection and returns all files
    /// </summary>
    public static void FTP(string host, string port, UserInfo user)
    {
        FtpClient client;

        if (port == null)
        {
            client = new FtpClient(host, user.Name, user.Password, 2121);
        }
        client = new FtpClient(host, user.Name, user.Password, int.Parse(port));

        client.Connect();
        Console.WriteLine("Connected.");

        Console.WriteLine("Input directory to search (none = root): ");
        string dir = Console.ReadLine();

        FtpListItem[] items;
        if (dir == null)
        {
           items = client.GetListing("/");
        }
        else
        {
           items = client.GetListing(dir);
        }
           

        Console.WriteLine("Files:");

        foreach (FtpListItem item in items)
        {
            Console.WriteLine($"{item.Name} ({item.Size} bytes), Last modified: {item.Modified}");
        }

        client.Disconnect();
    }

    #endregion


    #region helper methods

    /// <summary>
    /// hide password input in terminal
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