using CipherLock.Application.DTO;
using CipherLock.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CipherLock.Presentation.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    IAuthService authService
) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult> LoginAsync([FromBody] LoginDTO dto)
    {
        var token = await authService.LoginAsync(dto);
        return Ok(new { token });
    }
}
