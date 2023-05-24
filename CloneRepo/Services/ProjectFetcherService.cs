﻿using CloneRepo.Repositories;
using Hangfire;
using Octokit;

namespace CloneRepo.Services;

public class ProjectFetcherService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    public string githubUsername = "Akmalov-Asror";
    public ProjectFetcherService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);

            using (var scope = _serviceProvider.CreateScope())
            {
                var githubClient = scope.ServiceProvider.GetRequiredService<GitHubClient>();
                var repositoryFetcher = scope.ServiceProvider.GetRequiredService<RepositoryFetcher>();

                var repositories = await githubClient.Repository.GetAllForUser(githubUsername);

                /*var jobId = BackgroundJob.Schedule(
                    () => 
                    TimeSpan.FromDays(7));*/

                foreach (var repository in repositories)
                {
                    await repositoryFetcher.SaveRepository(repository);
                }
            }
        }
    }
}
