namespace CarInventory.CarInventory.Dal.BaseObjects
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public bool IsAvailable { get; set; }
        public int Quantity { get; set; }
    }

}
