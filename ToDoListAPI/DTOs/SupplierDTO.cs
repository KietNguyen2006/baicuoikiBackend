namespace ToDoListAPI.DTOs
{
    public class SupplierDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ContactInfo { get; set; }
        public string? Address { get; set; }
    }

    public class CreateSupplierDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? ContactInfo { get; set; }
        public string? Address { get; set; }
    }

    public class UpdateSupplierDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? ContactInfo { get; set; }
        public string? Address { get; set; }
    }
}