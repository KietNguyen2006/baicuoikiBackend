public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    // Quan hệ One-to-Many với Category
    public int CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    
    // Quan hệ One-to-One với ProductDetail
    public virtual ProductDetail? ProductDetail { get; set; }
    
    // Quan hệ Many-to-Many với Supplier
    public virtual ICollection<ProductSupplier>? ProductSuppliers { get; set; }
}