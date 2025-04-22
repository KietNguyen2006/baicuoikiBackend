namespace ToDoListAPI.Models.Auth
{
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Thêm dòng này để tránh lỗi CS1061
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

}
