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
        FTP();
    }

    #region methods

    /// <summary>
    /// Builds and FTP connection and returns all files
    /// </summary>
    public static void FTP()
    {
        Console.Write("Host: ");
        string host = Console.ReadLine()!;

        Console.Write("Username: ");
        string user = Console.ReadLine()!;

        Console.Write("Password: ");
        string pass = GetPasswordInput();

        Console.WriteLine("\nConnecting...\n");

        var client = new AsyncFtpClient(host, user, pass, 2121);

        client.Connect().GetAwaiter().GetResult();

        var items = client.GetListing("/").GetAwaiter().GetResult();

        Console.WriteLine("Files:");

        foreach (FtpListItem item in items)
        {
            Console.WriteLine($"{item.Name} ({item.Type}, {item.Size} bytes), Last modified: {item.Modified}");
        }

        client.Disconnect().GetAwaiter().GetResult();
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