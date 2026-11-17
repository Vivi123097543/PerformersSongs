using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using PerformersSongs.Domain.Entities;

namespace PerformersSongs.Infrastructure.Data;

public class DatabaseInitializer
{
    private readonly AppDbContext _context;

    public DatabaseInitializer(AppDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        if (await _context.Performers.AnyAsync())
        {
            Debug.WriteLine("База уже содержит исполнителей.");
            return;
        }

        var performers = new List<Performer>
        {
            new Performer
            {
                Name = "Queen",
                Country = "Великобритания",
                Genre = "Рок",
                FoundationDate = new DateTime(1970, 1, 1),
                Songs = new List<Song>
                {
                    new Song { Title = "Bohemian Rhapsody", ChartPosition = 1, ReleaseYear = 1975 },
                    new Song { Title = "We Will Rock You", ChartPosition = 2, ReleaseYear = 1977 }
                }
            },
            new Performer
            {
                Name = "ABBA",
                Country = "Швеция",
                Genre = "Поп",
                FoundationDate = new DateTime(1972, 1, 1),
                Songs = new List<Song>
                {
                    new Song { Title = "Dancing Queen", ChartPosition = 1, ReleaseYear = 1976 },
                    new Song { Title = "Mamma Mia", ChartPosition = 2, ReleaseYear = 1975 }
                }
            },
            new Performer
            {
                Name = "The Beatles",
                Country = "Великобритания",
                Genre = "Рок",
                FoundationDate = new DateTime(1960, 1, 1),
                Songs = new List<Song>
                {
                    new Song { Title = "Yesterday", ChartPosition = 1, ReleaseYear = 1965 },
                    new Song { Title = "Hey Jude", ChartPosition = 2, ReleaseYear = 1968 }
                }
            },
            new Performer
            {
                Name = "Metallica",
                Country = "США",
                Genre = "Хеви-метал",
                FoundationDate = new DateTime(1981, 1, 1),
                Songs = new List<Song>
                {
                    new Song { Title = "Nothing Else Matters", ChartPosition = 1, ReleaseYear = 1991 },
                    new Song { Title = "Enter Sandman", ChartPosition = 2, ReleaseYear = 1991 }
                }
            },
            new Performer
            {
                Name = "Imagine Dragons",
                Country = "США",
                Genre = "Альтернативный рок",
                FoundationDate = new DateTime(2008, 1, 1),
                Songs = new List<Song>
                {
                    new Song { Title = "Radioactive", ChartPosition = 1, ReleaseYear = 2012 },
                    new Song { Title = "Believer", ChartPosition = 2, ReleaseYear = 2017 }
                }
            }
        };

        await _context.Performers.AddRangeAsync(performers);
        await _context.SaveChangesAsync();

        Debug.WriteLine($"Добавлено исполнителей: {performers.Count}");
    }
}

