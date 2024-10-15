using CarInventory.CarInventory.Bll;
using CarInventory.CarInventory.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarInventory.CarInventory.Api
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class ForUsersController : ControllerBase
    {
        private readonly IForUserService _userService;

        public ForUsersController(IForUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetAvailableCars()
        {
            var cars = await _userService.GetAvailableCarsAsync();
            return Ok(cars);
        }
    }

}
