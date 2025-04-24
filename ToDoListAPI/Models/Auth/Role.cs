namespace ToDoListAPI.Models.Auth
{
    public class Role
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Quan hệ với UserRole
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // Helper method để kiểm tra xem role có hợp lệ không
        public bool IsValid()
        {
            return RoleConstants.IsValidRole(Name);
        }
    }
}
