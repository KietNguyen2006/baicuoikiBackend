namespace ToDoListAPI.DTOs
{
    public class ProductCreateDTO
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        // Add more fields if needed (e.g., Description, etc.)
    }
}