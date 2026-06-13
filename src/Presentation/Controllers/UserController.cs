using System.Security.Claims;
using CipherLock.Application.DTO;
using CipherLock.Application.Interfaces.Services;
using CipherLock.Presentation.Extensions;
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
        await userService.AddAsync(dto);

        return Ok("If this email is not registered, you will receive a confirmation email shortly.");
    }

    [Authorize]
    [HttpGet("me", Name = "GetUserById")]
    public async Task<ActionResult<ResponseUserDTO>> GetByIdAsync()
    {
        var userId = User.GetUserId();

        return Ok(await userService.GetByIdAsync(userId));
    }
    
    [Authorize]
    [HttpDelete("me")]
    public async Task<ActionResult> DeleteAsync()
    {
        var userId = User.GetUserId();
        await userService.DeleteAsync(userId);
    
        return NoContent();
    }
    
    [Authorize]
    [HttpPatch("me")]
    public async Task<ActionResult<ResponseUserDTO>> UpdateAsync([FromBody] UpdateUserDTO dto)
    {
        var userId = User.GetUserId();

        return Ok(await userService.UpdateAsync(userId, dto));
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
