using System.Security.Claims;
using CipherLock.Application.DTO;
using CipherLock.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CipherLock.Presentation.Controllers;

[ApiController]
[Route("credentials")]
public class CredentialController(
    ICredentialService credentialService
) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ResponseCredentialDTO>> AddAsync([FromBody] CreateCredentialDTO dto)
    {
        var userId = User.FindFirst(ClaimTypes.Name)?.Value;
        return Ok(await credentialService.AddAsync(int.Parse(userId!), dto));
    }

    [Authorize]
    [HttpPatch("{vaultId:int}/{credentialId:int}")]
    public async Task<ActionResult<ResponseCredentialDTO>> UpdateAsync(int vaultId, int credentialId, [FromBody] UpdateCredentialDTO dto)
    {
        var result = await credentialService.UpdateAsync(vaultId, credentialId, dto);
        return Ok(result);
    }
}
