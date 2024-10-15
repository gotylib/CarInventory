using CarInventory.CarInventory.Dal.BaseObjects;

namespace CarInventory.CarInventory.Bll
{
    public interface IForUserService
    {
        Task<List<Car>> GetAvailableCarsAsync();
    }
}
