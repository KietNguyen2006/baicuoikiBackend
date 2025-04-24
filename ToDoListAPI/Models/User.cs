using System;
using ToDoListAPI.Models.Auth;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Email { get; set; }
    public string? FullName { get; set; }
    
    // Quan hệ với UserRole
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
    // Helper method để kiểm tra role
    public bool HasRole(string roleName)
    {
        return UserRoles.Any(ur => ur.Role.Name == roleName);
    }
}