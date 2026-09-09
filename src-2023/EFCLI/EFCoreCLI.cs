using Microsoft.EntityFrameworkCore;


/// <summary>
/// The OR-Mapping. 
/// Needs DbContext with DbSet
/// </summary>
public class SongContext : DbContext
{
    public DbSet<Song> Songs => Set<Song>();
    public SongContext(DbContextOptions<SongContext> options) : base(options) { }
}


/// <summary>
/// CLI example of how to user EFCore as an ORM
/// Connects to database, adds to songs to table, and returns all entries
/// </summary>
/// <remark>
/// NEEDS DbContext-class and usage of DbSet.
/// Compare: https://learn.microsoft.com/ef/core/
/// 
/// EFCore can easily update and delete entries in database
/// </remarks>
public class EfCoreCLI
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Pick database provider: \n [1] MariaDB\n [2] PostgreSQL\n [3] MSSQL\n [4] MySQL\n");
        string choice = Console.ReadLine()!;
        var (provider, host, port, database, user, password) = ParseProvider(choice);

        SongContext db = ConnectToDB(provider, host, port, database, user, password);
        db.Database.EnsureCreated();
        db.Songs.ExecuteDelete();

        List<Song> songs = new List<Song>
            {
                new() { Name = "Twin Princes", Artist = "Yuka Kitamura", Album = "Dark Souls 3 (Original Game Soundtrack)", DurationInSeconds = 194 },
                new() { Name = "Darkeater Midir", Artist = "Yuka Kitamura", Album = "Dark Souls 3 (Original Game Soundtrack)", DurationInSeconds = 256 }
            };

        db.Songs.AddRange(songs);
        db.SaveChanges();
        Console.WriteLine($"{songs.Count} Songs added.");

        foreach (Song song in db.Songs.AsNoTracking())
        {
            Console.WriteLine($"[{song.Id}] {song.Artist} - {song.Name} ({song.Album}, {song.DurationInSeconds}s)");
        };
    }

    /// <summary>
    /// Baut den DbContext inkl. Provider-Auswahl auf.
    /// </summary>
    private static SongContext ConnectToDB(string provider, string host, string port, string database, string user, string password)
    {
        var options = new DbContextOptionsBuilder<SongContext>();

        switch (provider)
        {
            case "postgres":
                options.UseNpgsql($"Host={host};Port={port};Database={database};Username={user};Password={password}");
                break;
            case "mssql":
                options.UseSqlServer($"Server={host},{port};Database={database};User Id={user};Password={password};TrustServerCertificate=True");
                break;
            case "mysql":
            case "mariadb":
                string cs = $"Server={host};Port={port};Database={database};Uid={user};Pwd={password}";
                options.UseMySql(cs, ServerVersion.AutoDetect(cs));
                break;
        }

        return new SongContext(options.Options);
    }

    private static void UpdateEntry(SongContext db)
    {
        var first = db.Songs.FirstOrDefault();
        if (first != null)
        {
            first.Album = "Updated Album";
            db.SaveChanges();
            Console.WriteLine($"Song {first.Id} updated.");
        }
    }

    private static void DeleteEntry(SongContext db)
    {
        var toDelete = db.Songs.FirstOrDefault();
        if (toDelete != null)
        {
            db.Songs.Remove(toDelete);
            db.SaveChanges();
            Console.WriteLine($"Song {toDelete.Id} deleted.");
        }
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