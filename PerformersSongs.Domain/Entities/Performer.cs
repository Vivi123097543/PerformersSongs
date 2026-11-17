namespace PerformersSongs.Domain.Entities;

public class Performer
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public DateTime FoundationDate { get; set; }

    public string? PhotoPath { get; set; }

    // Список песен исполнителя
    public ICollection<Song> Songs { get; set; } = new List<Song>();
}
