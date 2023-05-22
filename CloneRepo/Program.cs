using CloneRepo.Data;
using CloneRepo.Entities;
using CloneRepo.Extensions;
using CloneRepo.Repositories;
using CloneRepo.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;


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


builder.Services.AddHangfire(config => config
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<RepositoryFetcher>();
builder.Services.AddScoped<RepositoryFetcherJob>();
builder.Services.AddScoped<GitHubService>();
builder.Services.AddHangfireServer();   

var app = builder.Build();

app.ConfigureHangfire(app.Environment, app.Services.GetRequiredService<IBackgroundJobClient>(), app.Services.GetRequiredService<IRecurringJobManager>(), app.Services.GetRequiredService<GitHubService>());

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
