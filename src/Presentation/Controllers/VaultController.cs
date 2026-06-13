
using CipherLock.Application.DTO;
using CipherLock.Application.Interfaces.Services;
using CipherLock.Presentation.Extensions;
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
        var userId = User.GetUserId();
        var result = await vaultService.AddAsync(userId, dto);

        return CreatedAtRoute("GetVaultById", new { vaultId = result.Id}, result);
    }

    [Authorize]
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ResponseVaultDTO>>> SearchByNameAsync(
        [FromQuery] string name
    )
    {
        var userId = User.GetUserId();
        var result = await vaultService.SearchByNameAsync(userId, name);

        return Ok(result);
    }

    [Authorize]
    [HttpPatch("{vaultId:int}")]
    public async Task<ActionResult> UpdateAsync(int vaultId, [FromBody] UpdateVaultDTO dto)
    {
        var userId = User.GetUserId();

        return Ok(await vaultService.UpdateAsync(userId, vaultId, dto));
    }

    [Authorize]
    [HttpGet("{vaultId:int}", Name = "GetVaultById")]
    public async Task<ActionResult<ResponseVaultDetailDTO>> GetByIdWithCredentialsAsync(int vaultId)
    {
        var userId = User.GetUserId();
        return Ok(await vaultService.GetByIdWithCredentialsAsync(userId, vaultId));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResponseVaultDTO>>> GetAllAsync()
    {
        var userId = User.GetUserId();
        return Ok(await vaultService.GetAllAsync(userId));
    }

    [Authorize]
    [HttpDelete("{vaultId:int}")]
    public async Task<ActionResult> DeleteAsync(int vaultId)
    {
        var userId = User.GetUserId();
        await vaultService.DeleteAsync(userId, vaultId);

        return NoContent();
    }
}
