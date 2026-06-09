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
    public async Task<ActionResult<ResponseVaultDTO>> Add(
        [FromBody] CreateVaultDTO dto
    )
    {
        var id = User.FindFirst(ClaimTypes.Name)?.Value;

        return Ok(await vaultService.Add(int.Parse(id!), dto));
    }

    [Authorize]
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ResponseVaultDTO>>> SearchByName(
        [FromQuery] string name
    )
    {
        var userId = User.FindFirst(ClaimTypes.Name)?.Value;
        var result = await vaultService.SearchByName(int.Parse(userId!), name);

        return Ok(result);
    }

    [Authorize]
    [HttpPatch("{vaultId:int}")]
    public async Task<ActionResult> Update(int vaultId, [FromBody] UpdateVaultDTO dto)
    {
        var userId = User.FindFirst(ClaimTypes.Name)?.Value;

        return Ok(await vaultService.Update(int.Parse(userId!), vaultId, dto));
    }

    [Authorize]
    [HttpGet("{vaultId:int}")]
    public async Task<ActionResult<ResponseVaultDetailDTO>> GetByIdWithCredentials(int vaultId)
    {
        var userId = User.FindFirst(ClaimTypes.Name)?.Value;
        return Ok(await vaultService.GetByIdWithCredentials(int.Parse(userId!), vaultId));
    }
}
