using CipherLock.Application.DTO;
using CipherLock.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CipherLock.Presentation.Controllers;

[ApiController]
[Route("users")]
public class UserController(
    UserService userService
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UserResponseDTO>> Add([FromBody] CreateUserDTO dto)
    {
        var result = await userService.Add(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id}, result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponseDTO>> GetById(int id)
        => Ok(await userService.GetById(id));

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAll()
        => Ok(await userService.GetAll());
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
        => NoContent();
}
