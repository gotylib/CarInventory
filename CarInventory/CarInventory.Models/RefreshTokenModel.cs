namespace CarInventory.CarInventory.Models
{
    public class RefreshTokenModel
    {
        public string? Token { get; set; }
        public DateTime Expiration { get; set; } // Время истечения токена
    }

}
