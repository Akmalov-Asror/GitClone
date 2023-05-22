using CloneRepo.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloneRepo.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Repository> Repositories { get; set; }
}