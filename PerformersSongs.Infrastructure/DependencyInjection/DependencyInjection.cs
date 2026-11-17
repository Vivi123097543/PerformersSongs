using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PerformersSongs.Application.Interfaces;
using PerformersSongs.Infrastructure.Data;
using PerformersSongs.Infrastructure.Repositories;
using PerformersSongs.Infrastructure.UnitOfWork;

namespace PerformersSongs.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string databasePath)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={databasePath}"));

        services.AddScoped<IPerformerRepository, PerformerRepository>();
        services.AddScoped<ISongRepository, SongRepository>();
        services.AddScoped<IUnitOfWork, 
             PerformersSongs.Infrastructure.UnitOfWork.UnitOfWork>();
       
        services.AddScoped<DatabaseInitializer>();
       
        return services;
    }
}
