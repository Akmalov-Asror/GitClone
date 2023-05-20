using System.Security.Cryptography.X509Certificates;
using CloneRepo.Data;
using CloneRepo.DTOs;
using CloneRepo.Entities;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CloneRepo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GithubRepositoryController : ControllerBase
{
    public readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public GithubRepositoryController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        var user = _context.Users.ToList();
        if (user == null)
        {
            return BadRequest();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync(UserDto userDto)
    {
        var user = new User()
        {
            Name = userDto.Name,
            Email = userDto.Email
        };
        var userCreateResult = await _userManager.CreateAsync(user, userDto.Password);

        if (!userCreateResult.Succeeded)
            return BadRequest();

        return Ok(userCreateResult);
    }
}