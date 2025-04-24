namespace ToDoListAPI.DTOs
{
    public class ProductDetailDTO
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Specifications { get; set; }
        public string? AdditionalInfo { get; set; }
        public int ProductId { get; set; }
    }

    public class CreateProductDetailDTO
    {
        public string? Description { get; set; }
        public string? Specifications { get; set; }
        public string? AdditionalInfo { get; set; }
        public int ProductId { get; set; }
    }

    public class UpdateProductDetailDTO
    {
        public string? Description { get; set; }
        public string? Specifications { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}