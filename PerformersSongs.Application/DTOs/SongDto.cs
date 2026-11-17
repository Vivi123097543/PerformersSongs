namespace PerformersSongs.Application.DTOs;

public class SongDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int ChartPosition { get; set; }

    public int ReleaseYear { get; set; }

    public TimeSpan Duration { get; set; }

    public int PerformerId { get; set; }

    public bool IsBelowTopTen => ChartPosition > 10;
}
