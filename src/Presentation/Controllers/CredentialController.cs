using System.Security.Claims;
using CipherLock.Application.DTO;
using CipherLock.Application.Interfaces.Services;
using CipherLock.Presentation.Extensions;
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
        var userId = User.GetUserId();
        return Ok(await credentialService.AddAsync(userId, dto));
    }

    [Authorize]
    [HttpGet("{vaultId:int}/credentials")]
    public async Task<ActionResult<IEnumerable<ResponseCredentialDTO>>> GetAllByVaultAsync(int vaultId)
    {
        var userId = User.GetUserId();
        var result = await credentialService.GetAllByVaultAsync(vaultId, userId);

        return Ok(result);
    }

    [Authorize]
    [HttpPatch("{vaultId:int}/{credentialId:int}")]
    public async Task<ActionResult<ResponseCredentialDTO>> UpdateAsync(
        int vaultId,
        int credentialId,
        [FromBody] UpdateCredentialDTO dto
    )
    {
        var userId = User.GetUserId();
        var result = await credentialService.UpdateAsync(userId, vaultId, credentialId, dto);
        return Ok(result);
    }
}
