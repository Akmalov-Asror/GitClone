using CloneRepo.Repositories;
using CloneRepo.Services;
using Hangfire;
using Hangfire.PostgreSql;

namespace CloneRepo.Extensions;

public class StartUp
{
    private readonly IConfiguration _configuration;

    public StartUp(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHangfire(config =>
        {
            config.UsePostgreSqlStorage(_configuration.GetConnectionString("Hangfire"));
        });

        services.AddHttpClient();

        services.AddScoped<GitHubService>();
        services.AddScoped<RepositoryFetcher>();

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs, IRecurringJobManager recurringJobs, RepositoryFetcher repositoryFetcher)
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