using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.MySql;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.DataProvider.SqlServer;
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

        List<Song> songs = new List<Song>
            {
                new() { Name = "Twin Princes", Artist = "Yuka Kitamura", Album = "Dark Souls 3 (Original Game Soundtrack)", DurationInSeconds = 194 },
                new() { Name = "Darkeater Midir", Artist = "Yuka Kitamura", Album = "Dark Souls 3 (Original Game Soundtrack)", DurationInSeconds = 256 }
            };

        foreach (Song s in songs)
        {
            db.Insert(s);
        }
        Console.WriteLine($"{songs.Count} Songs eingefügt.");

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
        
        switch(provider)
        {
            case "postgres":
                options = new DataOptions().UseConnectionString(
                    PostgreSQLTools.GetDataProvider(PostgreSQLVersion.v19),
                    $"Host={host};Port={port};Database={database};Username={user};Password={password}");
                break;

            case "mssql":
                options = new DataOptions().UseConnectionString(
                    SqlServerTools.GetDataProvider(SqlServerVersion.v2025),
                    $"Server={host},{port};Database={database};User Id={user};Password={password};TrustServerCertificate=True");
                break;

            case "mysql":
                options = new DataOptions().UseConnectionString(
                   MySqlTools.GetDataProvider(),
                   $"Server={host};Port={port};Database={database};Uid={user};Pwd={password}");
                break;

            case "mariadb":
                options = new DataOptions().UseConnectionString(
                    MySqlTools.GetDataProvider(),
                    $"Server={host};Port={port};Database={database};Uid={user};Pwd={password}");
                break;
        }

        MappingSchema mappingSchema = new MappingSchema();
        new FluentMappingBuilder(mappingSchema)
            .Entity<Song>()
                .HasTableName("Songs")
                .Property(s => s.Id).IsPrimaryKey().IsIdentity()
            .Build();

        var db = new DataConnection(options.UseMappingSchema(mappingSchema));

        CreateTable(db, provider);

        return db;
    }

    private static void CreateTable(DataConnection db, string provider)
    {
        string createTableSql = provider switch
        {
            "mssql" => "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Songs' AND xtype='U') CREATE TABLE Songs (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(255), Artist NVARCHAR(255), Album NVARCHAR(255), DurationInSeconds INT)",
            "postgres" => "CREATE TABLE IF NOT EXISTS Songs (Id SERIAL PRIMARY KEY, Name VARCHAR(255), Artist VARCHAR(255), Album VARCHAR(255), DurationInSeconds INT)",
            _ => "CREATE TABLE IF NOT EXISTS Songs (Id INT AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(255), Artist VARCHAR(255), Album VARCHAR(255), DurationInSeconds INT)"
        };
        db.Execute(createTableSql);

        db.Execute("DELETE FROM Songs");
    }

    private static (string provider, string host, string port, string database, string user, string password) ParseProvider(string choice)
    {
        return choice switch
        {
            "1" => ("mariadb", "127.0.0.1", "3307", "testdb", "testuser", "testpass"),
            "2" => ("postgres", "127.0.0.1", "5432", "testdb", "testuser", "testpass"),
            "3" => ("mssql", "127.0.0.1", "1433", "testdb", "testuser", "testpass"),
            "4" => ("mysql", "127.0.0.1", "3306", "testdb", "testuser", "testpass"),
            _ => throw new Exception("Provider unknown")
        };
    }
}