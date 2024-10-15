using CarInventory.CarInventory.Bll;
using CarInventory.CarInventory.Dal;
using CarInventory.CarInventory.Dal.BaseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarInventory.CarInventory.Api
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager,Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _carService.GetCarsAsync();
            return Ok(cars);
        }

        [HttpPost("CreateCar")]
        public async Task<IActionResult> CreateCar([FromBody] Car car)
        {
            var createdCar = await _carService.CreateCarAsync(car);
            return CreatedAtAction(nameof(GetCars), new { id = createdCar.Id }, createdCar);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] Car car)
        {
            try
            {
                await _carService.UpdateCarAsync(id, car);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                await _carService.DeleteCarAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/quantity")]
        public async Task<IActionResult> UpdateCarQuantity(int id, [FromBody] int quantity)
        {
            try
            {
                await _carService.UpdateCarQuantityAsync(id, quantity);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/availability")]
        public async Task<IActionResult> SetCarAvailability(int id, [FromBody] bool isAvailable)
        {
            try
            {
                await _carService.SetCarAvailabilityAsync(id, isAvailable);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }

}
