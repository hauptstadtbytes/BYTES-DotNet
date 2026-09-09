using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using MySqlConnector;
using Npgsql;

/// <summary>
/// CLI example of how to user Dapper as an ORM
/// </summary>
/// <remarks>
/// Needs different ADO.NET packages for different providers
/// Works on IDbConnection objects.
/// 
/// Very lightweight.
/// 
/// We need to write our own SQL queries, extension package Dapper.SqlBuilder has helper functions to build SQL query
/// Compare: https://github.com/DapperLib/Dapper
/// </remarks>
public class DapperCli
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Pick database provider: \n [1] MariaDB\n [2] PostgreSQL\n [3] MSSQL\n [4] MySQL\n");
        string choice = Console.ReadLine()!;
        var (provider, host, port, database, user, password) = ParseProvider(choice);

        using var db = ConnectToDB(provider, host, port, database, user, password);
        var (table, name, artist, album, dur) = QuoteTable(provider);

        List<Song> songs = new List<Song>
            {
                new() { Name = "Twin Princes", Artist = "Yuka Kitamura", Album = "Dark Souls 3 (Original Game Soundtrack)", DurationInSeconds = 194 },
                new() { Name = "Darkeater Midir", Artist = "Yuka Kitamura", Album = "Dark Souls 3 (Original Game Soundtrack)", DurationInSeconds = 256 }
            };

        db.Execute(
             $"INSERT INTO {table} ({name}, {artist}, {album}, {dur}) VALUES (@Name, @Artist, @Album, @DurationInSeconds)",
             songs);
        Console.WriteLine($"{songs.Count} Songs eingefügt.");

        foreach (var song in db.Query<Song>($"SELECT * FROM {table}"))
        {
            Console.WriteLine($"[{song.Id}] {song.Artist} - {song.Name} ({song.Album}, {song.DurationInSeconds}s)");
        }
    }

    /// <summary>
    /// Öffnet die Verbindung und legt zum Testen die Tabelle an, falls sie noch nicht existiert.
    /// </summary>
    private static IDbConnection ConnectToDB(string provider, string host, string port, string database, string user, string password)
    {
        IDbConnection db = provider switch
        {
            "postgres" => new NpgsqlConnection($"Host={host};Port={port};Database={database};Username={user};Password={password}"),
            "mssql" => new SqlConnection($"Server={host},{port};Database={database};User Id={user};Password={password};TrustServerCertificate=True;Encrypt=False"),
            "mysql" or "mariadb" => new MySqlConnection($"Server={host};Port={port};Database={database};Uid={user};Pwd={password}"),
            _ => throw new ArgumentException($"Unbekannter Provider: {provider}")
        };

        db.Open();

        CreateTable(db, provider);

        return db;
    }

    /// <summary>
    /// Create the table Songs if it doesnt exist
    /// Otherwise clear the table from previous runs
    /// </summary>
    /// <param name="db"></param>
    /// <param name="provider"></param>
    private static void CreateTable(IDbConnection db, string provider)
    {
        var (table, name, artist, album, dur) = QuoteTable(provider);

        string createTableSql = provider switch
        {
            "mssql" => $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Songs' AND xtype='U') CREATE TABLE {table} (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(255), Artist NVARCHAR(255), Album NVARCHAR(255), DurationInSeconds INT)",
            "postgres" => $"CREATE TABLE IF NOT EXISTS {table} (Id SERIAL PRIMARY KEY, Name VARCHAR(255), Artist VARCHAR(255), Album VARCHAR(255), DurationInSeconds INT)",
            _ => $"CREATE TABLE IF NOT EXISTS {table} (Id INT AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(255), Artist VARCHAR(255), Album VARCHAR(255), DurationInSeconds INT)"
        };

        string truncateSql = provider switch
        {
            "postgres" => $"TRUNCATE TABLE {table} RESTART IDENTITY",
            _ => $"TRUNCATE TABLE {table}"
        };

        db.Execute(createTableSql);
        db.Execute(truncateSql);
    }

    /// <summary>
    /// Put the table into quotation marks, so postgres can use it correctly
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    private static (string table, string name, string artist, string album, string dur) QuoteTable(string provider) => provider switch
    {
        "postgres" => ("\"Songs\"", "\"Name\"", "\"Artist\"", "\"Album\"", "\"DurationInSeconds\""),
        _ => ("Songs", "Name", "Artist", "Album", "DurationInSeconds")
    };

    private static (string provider, string host, string port, string database, string user, string password) ParseProvider(string choice)
    {
        return choice switch
        {
            "1" => ("mariadb", "127.0.0.1", "3307", "testdb", "testuser", "testpass"),
            "2" => ("postgres", "127.0.0.1", "5432", "testdb", "testuser", "testpass"),
            "3" => ("mssql", "127.0.0.1", "1433", "testdb", "sa", "YourStrong!Passw0rd"),
            "4" => ("mysql", "127.0.0.1", "3306", "testdb", "testuser", "testpass"),
            _ => throw new Exception("Provider unknown")
        };
    }
}
