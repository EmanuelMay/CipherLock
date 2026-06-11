using System.Security.Claims;
using CipherLock.Application.DTO;
using CipherLock.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CipherLock.Presentation.Controllers;

[ApiController]
[Route("users")]
public class UserController(
    IUserService userService
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ResponseUserDTO>> AddAsync([FromBody] CreateUserDTO dto)
    {
        var result = await userService.AddAsync(dto);

        return CreatedAtRoute("GetUserById", null, result);
    }

    [Authorize]
    [HttpGet("me", Name = "GetUserById")]
    public async Task<ActionResult<ResponseUserDTO>> GetByIdAsync()
    {
        var id = User.FindFirstValue(ClaimTypes.Name);

        return Ok(await userService.GetByIdAsync(int.Parse(id!)));
    }
    
    [Authorize]
    [HttpDelete("me")]
    public async Task<ActionResult> DeleteAsync()
    {
        var id = User.FindFirstValue(ClaimTypes.Name);
        await userService.DeleteAsync(int.Parse(id!));
    
        return NoContent();
    }
    
    [Authorize]
    [HttpPatch("me")]
    public async Task<ActionResult<ResponseUserDTO>> UpdateAsync([FromBody] UpdateUserDTO dto)
    {
        var id = User.FindFirstValue(ClaimTypes.Name);

        return Ok(await userService.UpdateAsync(int.Parse(id!), dto));
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordDTO dto)
    {
        await userService.ForgotPasswordAsync(dto);

        return Ok();
    }

    [HttpPatch("reset-password")]
    public async Task<ActionResult> ResetPasswordAsync([FromBody] ResetPasswordDTO dto)
    {
        await userService.ResetPasswordAsync(dto);

        return Ok();
    }

    [HttpPost("welcome-confirm")]
    public async Task<ActionResult> WelcomeConfirmAsync([FromBody] WelcomeConfirmDTO dto)
    {
        await userService.WelcomeConfirmAsync(dto);
        return Ok();
    }
}
