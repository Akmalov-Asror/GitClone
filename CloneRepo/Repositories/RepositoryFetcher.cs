using CloneRepo.Data;
using CloneRepo.Services;
using Microsoft.EntityFrameworkCore;
using Octokit;

namespace CloneRepo.Repositories;

public class RepositoryFetcher
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;

    public RepositoryFetcher(DbContextOptions<AppDbContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }

    public async Task SaveRepository(Repository repository)
    {
        var dbContext = new AppDbContext(_dbContextOptions);
        
            dbContext.Repositories.Add(new Entities.Repository()
            {
                Name = repository.Name,
                Description = repository.Description ?? "a"
            });
            await dbContext.SaveChangesAsync();
        
        
    }

}