public class ProductDetail
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Specifications { get; set; }
    public string? AdditionalInfo { get; set; }
    
    // Quan hệ One-to-One với Product
    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }
}