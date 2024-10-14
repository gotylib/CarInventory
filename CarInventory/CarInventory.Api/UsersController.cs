﻿using CarInventory.CarInventory.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarInventory.CarInventory.Api
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user, user.Password);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("DeleteeUser")]
        public async Task<IActionResult> DeleteUser([FromBody] ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }

            return BadRequest(result.Errors);
        }


    }

}
