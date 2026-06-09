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
}
