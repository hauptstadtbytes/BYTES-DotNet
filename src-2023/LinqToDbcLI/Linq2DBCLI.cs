using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

/// <summary>
/// CLI example of how to user LINQtoDB as an ORM
/// </summary>
/// <remark>
/// Needs different packages to connect to different DB providers
/// using: https://linq2db.github.io/
///</remark>
public class LinqToDbCli
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Pick database provider: \n [1] MariaDB\n [2] PostgreSQL\n [3] MSSQL\n [4] MySQL\n");
        string choice = Console.ReadLine()!;
        var (provider, host, port, database, user, password) = ParseProvider(choice);
        
        DataConnection db = ConnectToDB(provider, host, port, database, user, password);
        db.GetTable<Song>().Truncate();

        List<Song> songs = new List<Song>
            {
                new() { Name = "Twin Princes", Artist = "Yuka Kitamura", Album = "Dark Souls 3 (Original Game Soundtrack)", DurationInSeconds = 194 },
                new() { Name = "Darkeater Midir", Artist = "Yuka Kitamura", Album = "Dark Souls 3 (Original Game Soundtrack)", DurationInSeconds = 256 },
                new() { Name = "Ornstein & Smough", Artist = "Motoi Sakuraba", Album = "Dark Souls (Original Game Soundtrack)", DurationInSeconds = 170 },
                new() { Name = "Caligo, Miasma of Night", Artist = "FromSoftware", Album = "Elden Ring: NIGHTREIGN (Original Game Soundtrack)", DurationInSeconds = 422 }
            };

        foreach (Song s in songs)
        {
            db.Insert(s);
        }
        Console.WriteLine($"{songs.Count} Songs added.");

        foreach (Song song in db.GetTable<Song>())
        {
            Console.WriteLine($"[{song.Id}] {song.Artist} - {song.Name} ({song.Album}, {song.DurationInSeconds}s)");
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
            _ => throw new Exception("Provider unknown")
        };
    }
}