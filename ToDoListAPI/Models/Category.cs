public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    
    // Quan hệ One-to-Many với Product
    public virtual ICollection<Product>? Products { get; set; }
}