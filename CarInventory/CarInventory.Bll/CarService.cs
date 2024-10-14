using CarInventory.CarInventory.Dal;

namespace CarInventory.CarInventory.Bll
{
    public class CarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void MarkAsUnavailable(int carId)
        {
            var car = _context.Cars.Find(carId);
            if (car != null)
            {
                car.IsAvailable = false;
                _context.SaveChanges();
            }
        }

        // Другие методы для управления автомобилями...
    }

}
