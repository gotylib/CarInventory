using CarInventory.CarInventory.Dal.BaseObjects;
using CarInventory.CarInventory.Dal;
using Microsoft.EntityFrameworkCore;

namespace CarInventory.CarInventory.Bll
{
    public class ForUserService : IForUserService
    {
        private readonly ApplicationDbContext _context;

        public ForUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetAvailableCarsAsync()
        {
            return await _context.Cars.Where(d => d.IsAvailable).ToListAsync();
        }
    }
}
