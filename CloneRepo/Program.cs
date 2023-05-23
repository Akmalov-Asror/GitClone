using CloneRepo.Data;
using CloneRepo.Extensions;
using CloneRepo.Repositories;
using CloneRepo.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Octokit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//todo Extension method is here
builder.ConfigureServices();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<GitHubClient>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var client = new GitHubClient(new ProductHeaderValue("Auto.Test.Bot"));
    return client;
});

builder.Services.AddHangfire(config => config
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHostedService<ProjectFetcherService>();
builder.Services.AddScoped<RepositoryFetcher>();

builder.Services.AddHangfireServer();   

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.MapHangfireDashboard();

app.Run();
BackgroundJob.Enqueue(() => Console.WriteLine("Hello World"));
using (var server = new BackgroundJobServer())
{
    Console.ReadLine();
}
