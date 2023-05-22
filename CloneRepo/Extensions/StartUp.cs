using CloneRepo.Repositories;
using CloneRepo.Services;
using Hangfire;
using Hangfire.PostgreSql;
using System.Runtime.CompilerServices;

namespace CloneRepo.Extensions;

public static class StartUp
{
    /*private readonly IConfiguration _configuration;

    public StartUp(IConfiguration configuration)
    {
        _configuration = configuration;
    }*/

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHangfire(config =>
        {
            config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("Hangfire"));
        });

        builder.Services.AddHttpClient();

        builder.Services.AddScoped<GitHubService>();
        builder.Services.AddScoped<RepositoryFetcher>();

        builder.Services.AddControllers();
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs, IRecurringJobManager recurringJobs, RepositoryFetcher repositoryFetcher)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHangfireDashboard();

        backgroundJobs.Enqueue(() => repositoryFetcher.FetchRepositories());

        recurringJobs.AddOrUpdate(null,() => repositoryFetcher.FetchRepositories(), "*/10 * * * *");

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}