using PerformersSongs.Presentation.Pages;
using PerformersSongs.Presentation.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using Microsoft.Extensions.DependencyInjection;
using PerformersSongs.Application.DependencyInjection;
using PerformersSongs.Infrastructure.DependencyInjection;
using PerformersSongs.Infrastructure.Data;

namespace PerformersSongs.Presentation
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Регистрация Application (MediatR)
            builder.Services.AddApplication();

            // Путь к локальной базе данных SQLite
            string databasePath = Path.Combine(
                FileSystem.AppDataDirectory,
                "performers.db3");

            // Регистрация Infrastructure
            builder.Services.AddInfrastructure(databasePath);

            // Регистрация ViewModel и страниц
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<SongDetailsPage>();
            builder.Services.AddTransient<EditSongPage>();
            builder.Services.AddTransient<AddSongPage>();
            builder.Services.AddTransient<AddPerformerPage>();
            builder.Services.AddTransient<EditPerformerPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Инициализация базы данных
            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider
                    .GetRequiredService<DatabaseInitializer>();

                initializer.InitializeAsync().GetAwaiter().GetResult();
            }

            return app;
        }
    }
}
