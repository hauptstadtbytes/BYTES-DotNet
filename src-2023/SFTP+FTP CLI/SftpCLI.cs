// Use standard .Net amespaces
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Use Dependencies
using BYTES.NET.IO;
using Renci.SshNet;
using Renci.SshNet.Sftp;

public class SftpCLI
{
    public static void Main()
    {
        SFTP();
    }


    #region private methods

    /// <summary>
    /// builds and SFTP connection and returns all files
    /// </summary>
    private static void SFTP()
    {
        Console.Write("Host: ");
        string host = Console.ReadLine()!;

        Console.Write("Username: ");
        string user = Console.ReadLine()!;

        Console.Write("Password: ");
        string pass = GetPasswordInput();

        Console.WriteLine("\nConnecting...\n");

        using SftpClient client = new SftpClient(host, 2222, user, pass);
        client.Connect();

        var files = client.ListDirectory("/");

        Console.WriteLine("Files:");

        foreach (ISftpFile file in files)
        {
            if (file.Name == "." || file.Name == "..")
                continue;

            Console.WriteLine($"{file.Name} ({file.Length} bytes), Last modified: {file.LastWriteTime}");
        }

        client.Disconnect();
    }

    #endregion


    #region helper methods

    /// <summary>
    /// hide password input in terminal
    /// </summary>
    /// <returns></returns>
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