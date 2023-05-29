using CloneRepo.Data;
using CloneRepo.Extensions;
using CloneRepo.Repositories;
using CloneRepo.Services;
using Microsoft.EntityFrameworkCore;
using Octokit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureServices();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHostedService<ProjectFetcherService>();
builder.Services.AddScoped<RepositoryFetcher>();
builder.Services.AddCors(cors =>
{
    cors.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

if (app.Services.GetService<AppDbContext>() != null)
{
    var db = app.Services.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
