using CarInventory.CarInventory.Dal.BaseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarInventory.CarInventory.Api
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
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
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var result = await _userManager.CreateAsync(user, user.Password);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("DeleteeUser")]
        public async Task<IActionResult> DeleteUser([FromBody] User user)
        {
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }

            return BadRequest(result.Errors);
        }


        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("JWT is valid!");
        }

    }

}
