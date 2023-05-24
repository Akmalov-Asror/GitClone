using CloneRepo.Data;
using CloneRepo.DTOs;
using CloneRepo.Entities;
using CloneRepo.Repositories;
using CloneRepo.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Octokit;
using User = CloneRepo.Entities.User;

namespace CloneRepo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GithubRepositoryController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly RepositoryFetcher _repositoryFetcher;
    private readonly GitHubService _gitHubService;
    public GithubRepositoryController(AppDbContext context, RepositoryFetcher repositoryFetcher, GitHubService gitHubService)
    {
        _context = context;
        _repositoryFetcher = repositoryFetcher;
        _gitHubService = gitHubService;
    }

    [HttpGet]
    public IActionResult GetUsersAsync()
    {
        var user = _context.Users.ToList();
        var users = _context.Repositories.ToList();
        if (user == null)
            return BadRequest();

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync(UserDto userDto)
    {
        var user = new User();
        user.Name = userDto.Name;
        user.Email = userDto.Email;
        user.Password = userDto.Password;

        if (user is null)
            return BadRequest();

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok();
    }

}