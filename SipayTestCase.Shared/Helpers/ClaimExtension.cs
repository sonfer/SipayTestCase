using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SipayTestCase.Shared.Helpers;

public static class ClaimExtension
{
    private static ClaimsPrincipal GetClaimsPrincipal(this IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor?.HttpContext?.User;
    }

    /// <summary>
    /// Returns IsAuthenticated If user is have
    /// </summary>
    /// <returns>IsAuthenticated</returns>
    public static bool IsAuthenticated(this IHttpContextAccessor httpContextAccessor)
    {
        var identity = GetClaimsPrincipal(httpContextAccessor)?.Identity;

        return identity != null && GetClaimsPrincipal(httpContextAccessor) != null 
                                && GetClaimsPrincipal(httpContextAccessor)?.Identity != null 
                                && identity.IsAuthenticated;
    }
    /// <summary>
    /// Returns User Claims by key If user is have
    /// </summary>
    /// <returns>Value of Key</returns>
    public static string GetByKey(this IHttpContextAccessor httpContextAccessor, string key)
    {
        return IsAuthenticated(httpContextAccessor) 
            ? GetClaimsPrincipal(httpContextAccessor).Claims.FirstOrDefault(i => i.Type == key)?.Value 
            : null;
    }

    public static Guid? GetUserId(this IHttpContextAccessor httpContextAccessor)
    {
        var claimsUser = GetClaimsPrincipal(httpContextAccessor).FindFirst(ClaimTypes.Name)?.Value;
        var userId = claimsUser;
        return userId != null 
            ? Guid.Parse(userId) 
            : null;
    }
    
    
    
    
}