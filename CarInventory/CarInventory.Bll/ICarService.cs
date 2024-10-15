using CarInventory.CarInventory.Dal.BaseObjects;

namespace CarInventory.CarInventory.Bll
{
    public interface ICarService
    {
        Task<List<Car>> GetCarsAsync();
        Task<Car> CreateCarAsync(Car car);
        Task UpdateCarAsync(int id, Car car);
        Task DeleteCarAsync(int id);
        Task UpdateCarQuantityAsync(int id, int quantity);
        Task SetCarAvailabilityAsync(int id, bool isAvailable);
    }

}
