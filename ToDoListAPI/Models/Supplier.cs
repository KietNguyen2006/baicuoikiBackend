public class Supplier
{
    public int Id { get; set; }
    public required string Name { get; set; }
    // Add this line:
    public string? Description { get; set; }
    public string? ContactInfo { get; set; }
    public string? Address { get; set; }
    
    // Quan hệ Many-to-Many với Product
    public virtual ICollection<ProductSupplier>? ProductSuppliers { get; set; }
}