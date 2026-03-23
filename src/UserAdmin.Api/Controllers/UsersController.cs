using Application.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    private readonly ILogger<AuthController> _logger;

    public UserController(IUserService userService, ILogger<AuthController> logger)
    {
      
        _userService = userService;
        _logger = logger;
    }

    [Authorize]
    [HttpGet("UserDetails")]
    public async Task<IActionResult> UserDetails()
    {
        var user = await _userService.GetUserByEmailAsync(ClaimTypes.NameIdentifier);
       
        return Ok(new { user?.Data?.FirstName, user?.Data?.LastName, user?.Data?.Email });
    }
}
