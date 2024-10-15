using CarInventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CarInventory.CarInventory.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CarInventory.CarInventory.Bll;
using Microsoft.EntityFrameworkCore;
using CarInventory.CarInventory.Dal.BaseObjects;
using CarInventory.CarInventory.Api;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        return await _authService.Register(model);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        return await _authService.Login(model);
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
    {
        return await _authService.RefreshToken(model);
    }
}



