namespace ToDoListAPI.Models.Auth
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}