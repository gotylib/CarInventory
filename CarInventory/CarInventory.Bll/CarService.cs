using CarInventory.CarInventory.Dal.BaseObjects;
using CarInventory.CarInventory.Dal;
using Microsoft.EntityFrameworkCore;

namespace CarInventory.CarInventory.Bll
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car> CreateCarAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task UpdateCarAsync(int id, Car car)
        {
            if (id != car.Id)
                throw new ArgumentException("ID mismatch");

            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) throw new KeyNotFoundException("Car not found");

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCarQuantityAsync(int id, int quantity)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) throw new KeyNotFoundException("Car not found");

            car.Quantity = quantity;
            await _context.SaveChangesAsync();
        }

        public async Task SetCarAvailabilityAsync(int id, bool isAvailable)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) throw new KeyNotFoundException("Car not found");

            car.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();
        }
    }

}
