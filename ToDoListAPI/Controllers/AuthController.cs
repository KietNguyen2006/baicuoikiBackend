using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Services; // Đảm bảo bạn dùng đúng namespace của JwtService và các dịch vụ liên quan
using Microsoft.EntityFrameworkCore; // Để sử dụng phương thức Include và các chức năng liên quan đến EF
using BCrypt.Net;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly ApplicationDbContext _context;

        public AuthController(JwtService jwtService, ApplicationDbContext context)
        {
            _jwtService = jwtService;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Kiểm tra người dùng trong cơ sở dữ liệu
            var user = _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Tài khoản hoặc mật khẩu sai.");
            }

            // Lấy Role của User và tạo Token
            var role = user.UserRoles.FirstOrDefault()?.Role.Name ?? "User"; // Lấy Role của User
            var token = _jwtService.GenerateToken(user.Username, role); // Tạo token với Role

            return Ok(new { token });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
