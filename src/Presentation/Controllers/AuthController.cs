using CipherLock.Application.DTO;
using CipherLock.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CipherLock.Presentation.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    IAuthService authService
) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDTO dto)
    {
        var token = await authService.Login(dto);
        return Ok(new { token });
    }
}
