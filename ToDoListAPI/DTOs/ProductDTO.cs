using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.DTOs
{
    public class ProductDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty; // Initialize with a default value

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    public class CreateProductDTO
    {
        public string Name { get; set; } = string.Empty; // Initialize with a default value
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }

    public class UpdateProductDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}