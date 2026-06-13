using System.Security.Claims;

namespace CipherLock.Presentation.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.Name)
            ?? throw new UnauthorizedAccessException("user id claim not found");
        return int.Parse(value);
    }
}
