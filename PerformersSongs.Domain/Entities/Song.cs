namespace PerformersSongs.Domain.Entities;

public class Song
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    // Позиция в музыкальном чарте
    public int ChartPosition { get; set; }

    public int ReleaseYear { get; set; }

    public TimeSpan Duration { get; set; }

    // Внешний ключ исполнителя
    public int PerformerId { get; set; }

    // Навигационное свойство
    public Performer Performer { get; set; } = null!;
}
