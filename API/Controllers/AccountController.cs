using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extenstions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]  //api/account/register   here register is fixed path segemnt
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        using var hmac = new HMACSHA512();
        if (await EmailExists(registerDto.Email))
        {
            return BadRequest("Email Already Exist");
        }
        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };
        context.Add(user);
        await context.SaveChangesAsync();
        return user.ToDto(tokenService);
    }

    [HttpPost("login")]  //api/account/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.User.SingleOrDefaultAsync(x => x.Email == loginDto.Email);
        if (user == null) return Unauthorized("Invalid Email");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }
        
        return user.ToDto(tokenService);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteUser([FromBody] string id)
    {
        var user = await context.User.FindAsync(id);
        if (user == null) { return NotFound(); }
        context.Remove(user);
        await context.SaveChangesAsync();
        return NoContent();
    }

    public async Task<bool> EmailExists(string email)
    {
        return await context.User.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }

}
