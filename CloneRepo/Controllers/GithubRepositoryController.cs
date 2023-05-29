using CloneRepo.Data;
using CloneRepo.DTOs;
using CloneRepo.Entities;
using CloneRepo.Repositories;
using CloneRepo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Octokit;

namespace CloneRepo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GithubRepositoryController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly RepositoryFetcher _repositoryFetcher;
    public GithubRepositoryController(AppDbContext context, RepositoryFetcher repositoryFetcher)
    {
        _context = context;
        _repositoryFetcher = repositoryFetcher;
    }

    [HttpGet("gitRepositories")]
    public async Task<IActionResult> GetUsersAsync()
    {
        var users = await _context.Repositories.ToListAsync();
        
        return Ok(users);
    }

}