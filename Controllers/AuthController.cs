using Application.Dtos.UserDtos;
using Application.Interfaces.Services;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{

    private readonly IUserService _userService;

    private readonly ILogger<AuthController> _logger;   

    public AuthController(ILogger<AuthController> logger, IUserService userService)
    {

        _logger = logger;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RequestUserDto user)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

        var Id =  _userService.CreateUserAsync(user);


        return Ok(); 
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var user = await _userService.GetUserByEmailAsync(req.Email);
        if (user == null) return Unauthorized();

        if(user?.Data?.Email != null)
        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.Data.Email))
            return Unauthorized();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-key-123"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(claims: new[] {
            new Claim(ClaimTypes.NameIdentifier, user?.Data?.Id.ToString())
        }, signingCredentials: creds, expires: DateTime.Now.AddHours(1));

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

public record LoginRequest(string Email, string Password);
