using LinqToDB.Mapping;

[Table("Durations")]
public class Duration
{
    [PrimaryKey, Identity] public int Id { get; set; }
    [Column("songID")] public int SongId { get; set; }
    [Column("duration")] public int Seconds { get; set; }
}