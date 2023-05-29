using CloneRepo.Repositories;
using CloneRepo.Services;
using Hangfire;
using Hangfire.PostgreSql;
using System.Runtime.CompilerServices;

namespace CloneRepo.Extensions;

public static class StartUp
{
    private static readonly RepositoryFetcher _repositoryFetcher;

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHangfire(config =>
        {
            config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("Hangfire"));
        });

        builder.Services.AddHttpClient();

        builder.Services.AddScoped<RepositoryFetcher>();

        builder.Services.AddControllers();

    }
    
}