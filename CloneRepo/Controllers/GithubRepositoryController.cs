using CloneRepo.Data;
using CloneRepo.DTOs;
using CloneRepo.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloneRepo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GithubRepositoryController : ControllerBase
{
    public readonly AppDbContext _context;

    public GithubRepositoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetUsersAsync()
    {
        var user = _context.Users.ToList();

        if (user == null)
            return BadRequest();

        return Ok(user);
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