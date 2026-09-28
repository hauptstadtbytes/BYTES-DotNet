using LinqToDB.Mapping;

[Table("SongWithDuration")]
public class SongWithDuration
{
    [Column("Id")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; } = null!;

    [Column("Artist")]
    public string Artist { get; set; } = null!;

    [Column("Album")]
    public string Album { get; set; } = null!;

    [Column("duration")]
    public int Duration { get; set; }

}