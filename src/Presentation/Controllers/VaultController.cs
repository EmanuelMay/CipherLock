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
    public async Task<ActionResult<ResponseVaultDTO>> Add([FromBody] CreateVaultDTO dto)
    {
        var id = User.FindFirst(ClaimTypes.Name)?.Value;

        return Ok(await vaultService.Add(int.Parse(id!), dto));
    }
}
