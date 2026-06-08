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
using Renci.SshNet;

public class SftpFtpCLI
{
    #region main programm

    public static void Main()
    {
        Console.WriteLine("ftp: f\nsftp: s");
        string protocol = Console.ReadLine()!;

        if(protocol == "f")
        {
            FTP();
        }
        else if (protocol == "s")
        {
            SFTP();
        }
    }

    #endregion


    #region functions

    private static void FTP()
    {
        Console.Write("Host: ");
        string host = Console.ReadLine()!;

        Console.Write("Username: ");
        string user = Console.ReadLine()!;

        Console.Write("Password: ");
        string pass = Console.ReadLine()!;


        Console.WriteLine("\nConnecting...\n");

        var client = new AsyncFtpClient(host, user, pass, 2121);

        client.Connect().GetAwaiter().GetResult();

        var items = client.GetListing("/").GetAwaiter().GetResult();

        Console.WriteLine("Files:");

        foreach (var item in items)
        {
            Console.WriteLine($"{item.Name} ({item.Type})");
        }

        client.Disconnect().GetAwaiter().GetResult();
    }


    private static void SFTP()
    {
        Console.Write("Host: ");
        string host = Console.ReadLine()!;

        Console.Write("Username: ");
        string user = Console.ReadLine()!;

        Console.Write("Password: ");
        string pass = Console.ReadLine()!;

        Console.WriteLine("\nConnecting...\n");

        using SftpClient client = new SftpClient(host, 2222, user, pass);
        client.Connect();

        var files = client.ListDirectory("/");

        Console.WriteLine("Files:");

        foreach (var file in files)
        {
            if (file.Name == "." || file.Name == "..")
                continue;

            Console.WriteLine($"{file.Name}");
        }

        client.Disconnect();
    }

    #endregion


}