using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;
using System.Runtime.CompilerServices;
using System.Xml;

/// <summary>
/// CLI example of how to user LINQtoDB as an ORM
/// </summary>
/// <remark>
/// Needs different packages to connect to different DB providers
/// using: https://linq2db.github.io/
///</remark>
public class LinqToDbCli
{
    private static string GetSourceDirectory([CallerFilePath] string sourceFile = "")
        => Path.GetDirectoryName(sourceFile)!;

    public static void Main(string[] args)
    {
        Console.WriteLine("Pick database provider: \n [1] MariaDB\n [2] PostgreSQL\n [3] MSSQL\n [4] MySQL\n [5] SQLite");
        string choice = Console.ReadLine()!;
        var (provider, host, port, database, user, password) = ParseProvider(choice);
        
        DataConnection db = ConnectToDB(provider, host, port, database, user, password);
        db.GetTable<Song>().Truncate();
        
        List<Song> songs = new List<Song>
            {
                new() { Name = "Twin Princes", Artist = "Yuka Kitamura", Album = "Dark Souls 3 (Original Game Soundtrack)" },
                new() { Name = "Darkeater Midir", Artist = "Yuka Kitamura", Album = "Dark Souls 3 (Original Game Soundtrack)" },
                new() { Name = "Ornstein & Smough", Artist = "Motoi Sakuraba", Album = "Dark Souls (Original Game Soundtrack)" },
                new() { Name = "Caligo, Miasma of Night", Artist = "FromSoftware", Album = "Elden Ring: NIGHTREIGN (Original Game Soundtrack)" }
            };
        /*
        foreach (Song s in songs)
        {
            db.Insert(s);
        }
        Console.WriteLine($"{songs.Count} Songs added.");
        */

        // Bulk insert
        db.GetTable<Song>().BulkCopy(songs);

        foreach (Song song in db.GetTable<Song>())
        {
            Console.WriteLine($"[{song.Id}] {song.Artist} - {song.Name} ({song.Album})");
        }

        // create other table
        db.CreateTable<Duration>(tableOptions: TableOptions.CreateIfNotExists);

        // fill table
        List<Duration> durations = new()
        {
            new() { SongId = 1, Seconds = 194 },
            new() { SongId = 2, Seconds = 256 },
            new() { SongId = 3, Seconds = 170 },
            new() { SongId = 4, Seconds = 422 }
        };
        db.GetTable<Duration>().Truncate();
        db.GetTable<Duration>().BulkCopy(durations);

        // create a view per SQL
        db.Execute(@"
            CREATE VIEW IF NOT EXISTS `SongWithDuration` AS
            SELECT
                s.Id,
                s.Name,
                s.Artist,
                s.Album,
                d.duration
            FROM Songs s
            LEFT JOIN Durations d ON s.Id = d.songID;
        ");

        // Get Data from the view
        Console.WriteLine("\nGet the Data from the View\n");

        var view = db.GetTable<SongWithDuration>();

        foreach (var v in view)
        {
            Console.WriteLine($"{v.Id}: {v.Artist} - {v.Name} ({v.Album}, {v.Duration})");
        }
    }

    /// <summary>
    /// Create the connection to the specified database
    /// Also optionally create the table "Songs" if it doesnt exist yet
    /// </summary>
    private static DataConnection ConnectToDB(string provider, string host, string port, string database, string user, string password)
    {
        DataOptions options = null;

        switch (provider)
        {
            case "postgres":
                options = new DataOptions().UsePostgreSQL($"Host={host};Port={port};Database={database};Username={user};Password={password}");
                break;
            case "mssql":
                options = new DataOptions().UseSqlServer($"Server={host},{port};Database={database};User Id={user};Password={password};TrustServerCertificate=True");
                break;
            case "mysql":
                options = new DataOptions().UseMySql($"Server={host};Port={port};Database={database};Uid={user};Pwd={password}");
                break;
            case "mariadb":
                options = new DataOptions().UseMySql($"Server={host};Port={port};Database={database};Uid={user};Pwd={password}");
                break;
            case "sqlite":
                string dbPath = Path.Combine(GetSourceDirectory(), database); ;
                options = new DataOptions().UseSQLite($"Data Source={dbPath}", SQLiteProvider.Microsoft);
                break;
        }

        MappingSchema mappingSchema = new MappingSchema();
        new FluentMappingBuilder(mappingSchema).Entity<Song>().HasTableName("Songs")
            .Property(s => s.Id).IsPrimaryKey().IsIdentity().Build();

        DataConnection db = new DataConnection(options.UseMappingSchema(mappingSchema));

        db.CreateTable<Song>(tableOptions: TableOptions.CreateIfNotExists);
        return db;
    }

    private static (string provider, string host, string port, string database, string user, string password) ParseProvider(string choice)
    {
        return choice switch
        {
            "1" => ("mariadb", "127.0.0.1", "3307", "testdb", "testuser", "testpass"),
            "2" => ("postgres", "127.0.0.1", "5432", "testdb", "testuser", "testpass"),
            "3" => ("mssql", "127.0.0.1", "1433", "testdb", "sa", "YourStrong!Passw0rd"),
            "4" => ("mysql", "127.0.0.1", "3306", "testdb", "testuser", "testpass"),
            "5" => ("sqlite", "", "", "testdb.sqlite", "", ""),
            _ => throw new Exception("Provider unknown")
        };
    }
}