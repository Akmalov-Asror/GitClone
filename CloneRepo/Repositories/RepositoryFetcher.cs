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
        using (var dbContext = new AppDbContext(_dbContextOptions))
        {
            // Repository ma'lumotlarini Postgres ma'lumotlar bazasiga saqlash
            // dbContext orqali ma'lumotlar bazasiga qo'shimcha ma'lumotlarni saqlash
            //dbContext.Repositories.Add(repository);

            await dbContext.SaveChangesAsync();
        }
    }
}