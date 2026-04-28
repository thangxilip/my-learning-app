using System.Security.Claims;
using Flashcard.Application.Common.Exceptions;

namespace Flashcard.API.Auth;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetRequiredUserId(this ClaimsPrincipal user)
    {
        var rawValue = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
        if (Guid.TryParse(rawValue, out var userId))
        {
            return userId;
        }

        throw new UnauthorizedException("Missing or invalid user id claim. Expected NameIdentifier/sub (GUID).");
    }
}
