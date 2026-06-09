using System.Security.Claims;
using CipherLock.Application.Services;
using CipherLock.Domain.Interfaces;
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
        var id = User.FindFirst(ClaimTypes.Name)?.Value;

        var result = await vaultService.AddAsync(int.Parse(id!), dto);

        return CreatedAtAction(nameof(GetByIdWithCredentialsAsync), new { vaultId = result.Id}, result);
    }

    [Authorize]
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ResponseVaultDTO>>> SearchByNameAsync(
        [FromQuery] string name
    )
    {
        var userId = User.FindFirst(ClaimTypes.Name)?.Value;
        var result = await vaultService.SearchByNameAsync(int.Parse(userId!), name);

        return Ok(result);
    }

    [Authorize]
    [HttpPatch("{vaultId:int}")]
    public async Task<ActionResult> UpdateAsync(int vaultId, [FromBody] UpdateVaultDTO dto)
    {
        var userId = User.FindFirst(ClaimTypes.Name)?.Value;

        return Ok(await vaultService.UpdateAsync(int.Parse(userId!), vaultId, dto));
    }

    [Authorize]
    [HttpGet("{vaultId:int}")]
    public async Task<ActionResult<ResponseVaultDetailDTO>> GetByIdWithCredentialsAsync(int vaultId)
    {
        var userId = User.FindFirst(ClaimTypes.Name)?.Value;
        return Ok(await vaultService.GetByIdWithCredentialsAsync(int.Parse(userId!), vaultId));
    }
}
