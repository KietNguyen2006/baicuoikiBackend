using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Services;
using ToDoListAPI.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var response = await _authService.Login(loginDto);
        if (response == null)
        {
            return Unauthorized();
        }

        return Ok(response);
    }
}
