using CloneRepo.Data;
using CloneRepo.Services;
using Octokit;

namespace CloneRepo.Repositories;

public class RepositoryFetcher
{
    private readonly GitHubService _gitHubService;
    private readonly AppDbContext _dbContext;

    public RepositoryFetcher(GitHubService gitHubService, AppDbContext dbContext)
    {
        _gitHubService = gitHubService;
        _dbContext = dbContext;
    }

    public async Task FetchRepositories()
    {
        var owner = "YourGitHubUsername";
        var count = 10;

        var repositories = await _gitHubService.GetRepositories(owner, count);

        foreach (var repository in repositories)
        {
            _dbContext.Repositories.Add(repository);
        }

        await _dbContext.SaveChangesAsync();
    }
}