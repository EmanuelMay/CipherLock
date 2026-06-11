using System.Security.Claims;
using CipherLock.Application.DTO;
using CipherLock.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CipherLock.Presentation.Controllers;

[ApiController]
[Route("vaults")]
public class VaultController(
    IVaultService vaultService
) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ResponseVaultDTO>> AddAsync(
        [FromBody] CreateVaultDTO dto
    )
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var result = await vaultService.AddAsync(int.Parse(userId!), dto);

        return CreatedAtRoute("GetVaultById", new { vaultId = result.Id}, result);
    }

    [Authorize]
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ResponseVaultDTO>>> SearchByNameAsync(
        [FromQuery] string name
    )
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var result = await vaultService.SearchByNameAsync(int.Parse(userId!), name);

        return Ok(result);
    }

    [Authorize]
    [HttpPatch("{vaultId:int}")]
    public async Task<ActionResult> UpdateAsync(int vaultId, [FromBody] UpdateVaultDTO dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);

        return Ok(await vaultService.UpdateAsync(int.Parse(userId!), vaultId, dto));
    }

    [Authorize]
    [HttpGet("{vaultId:int}", Name = "GetVaultById")]
    public async Task<ActionResult<ResponseVaultDetailDTO>> GetByIdWithCredentialsAsync(int vaultId)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await vaultService.GetByIdWithCredentialsAsync(int.Parse(userId!), vaultId));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResponseVaultDTO>>> GetAllAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await vaultService.GetAllAsync(int.Parse(userId!)));
    }

    [Authorize]
    [HttpDelete("{vaultId:int}")]
    public async Task<ActionResult> DeleteAsync(int vaultId)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        await vaultService.DeleteAsync(int.Parse(userId!), vaultId);

        return NoContent();
    }
}
