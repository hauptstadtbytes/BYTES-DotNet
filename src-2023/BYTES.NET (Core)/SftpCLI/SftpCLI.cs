// Use standard .Net amespaces
// Use Dependencies
using BYTES.NET.IO;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

public class SftpCLI
{
    public static void Main()
    {
        Console.Write("Host: ");
        string host = Console.ReadLine()!;

        Console.Write("Port (optional): ");
        string port = Console.ReadLine();

        Console.Write("Username: ");
        string username = Console.ReadLine()!;

        Console.Write("Keyfile path: ");
        string keyFilePath = Console.ReadLine()!;

        Console.Write("Keyfile password: ");
        string pass = GetPasswordInput();

        UserInfo user = new UserInfo(username, pass);

        Console.WriteLine("\nConnecting...\n");

        SFTP(host, port, user, keyFilePath);
    }


    #region private methods

    /// <summary>
    /// builds and SFTP connection and returns all files
    /// </summary>
    private static void SFTP(string host, string port, UserInfo user, string keyFilePath)
    {
        PrivateKeyFile keyFile = new PrivateKeyFile(keyFilePath, user.Password);
        SftpClient client;

        if (port == "")
        {
            client = new SftpClient(new PrivateKeyConnectionInfo(host, 22, user.Name, keyFile));
        }
        else
        {
            client = new SftpClient(new PrivateKeyConnectionInfo(host, int.Parse(port), user.Name, keyFile));
        }
        
        client.Connect();

        Console.WriteLine("Connected.");

        Console.WriteLine("Input directory to search (none = root): ");
        string dir = Console.ReadLine();


        IEnumerable<ISftpFile> files;
        if (dir == null)
        {
            files = client.ListDirectory("/");
        }
        else
        {
            files = client.ListDirectory(dir);
        }

        Console.WriteLine("Files:");

        foreach (ISftpFile file in files)
        {
            if (file.Name == "." || file.Name == "..")
                continue;

            Console.WriteLine($"{file.Name} ({file.Length} bytes), Last modified: {file.LastWriteTime}");
        }

        client.Disconnect();
    }

    /// <summary>
    /// EXAMPLE
    /// Uploads a file to the SFTP server. Does not check for duplicates
    /// </summary>
    /*
    private string? UploadDocument(string host, UserInfo user, string keyFilePath, string filepath, string filename, string rootdir)
    {
        // Create new PrivateKeyFile to auth with, create client
        PrivateKeyFile keyFile = new PrivateKeyFile(keyFilePath, user.Password);
        using SftpClient client = new SftpClient(new PrivateKeyConnectionInfo(host, 2222, user.Name, keyFile));

        string remoteFileName = filename;

        client.Connect();

        //upload the file
        using (FileStream fs = new FileStream(filepath, FileMode.Open))
        {
            client.UploadFile(fs, rootdir + remoteFileName);
        }

        client.Disconnect();

        return rootdir + remoteFileName;
    }
    */

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