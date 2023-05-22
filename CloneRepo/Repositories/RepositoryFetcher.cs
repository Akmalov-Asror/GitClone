using CloneRepo.Data;
using Octokit;

namespace CloneRepo.Repositories;

public class RepositoryFetcher
{
    private readonly AppDbContext _dbContext;
    private readonly GitHubClient _gitHubClient;
    public RepositoryFetcher(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _gitHubClient = new GitHubClient(new ProductHeaderValue("https://github.com/Akmalov-Asror"));
    }

    public async Task FetchRepositories(string owner, int count)
    {
        var repositories = await FetchFromGitHub(owner, count);

        foreach (var repository in repositories)
        {
            _dbContext.Repositories.Add(repository);
        }
        await _dbContext.SaveChangesAsync();
    }

    private async Task<List<Entities.Repository>> FetchFromGitHub(string owner, int count)
    {
        var options = new ApiOptions
        {
            PageSize = count,
            PageCount = 10
        };

        var repositories = await _gitHubClient.Repository.GetAllForUser(owner, options);

        return repositories.Select(r => new Entities.Repository
        {
            Name = r.Name,
            Description = r.Description,
        }).ToList();
    }
}