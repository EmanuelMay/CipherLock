using System.Security.Claims;
using CipherLock.Application.DTO;
using CipherLock.Domain.Interfaces;
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
    public async Task<ActionResult<ResponseUserDTO>> Add([FromBody] CreateUserDTO dto)
    {
        var result = await userService.Add(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id}, result);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ResponseUserDTO>> GetById()
    {
        var id = User.FindFirst(ClaimTypes.Name)?.Value;

        return Ok(await userService.GetById(int.Parse(id!)));
    }
    
    [Authorize]
    [HttpDelete("me")]
    public async Task<ActionResult> Delete()
    {
        var id = User.FindFirst(ClaimTypes.Name)?.Value;

        await userService.Delete(int.Parse(id!));
        return NoContent();
    }
    
    [Authorize]
    [HttpPatch("me")]
    public async Task<ActionResult<ResponseUserDTO>> Update([FromBody] UpdateUserDTO dto)
    {
        var id = User.FindFirst(ClaimTypes.Name)?.Value;

        return Ok(await userService.Update(int.Parse(id!), dto));
    }
}
