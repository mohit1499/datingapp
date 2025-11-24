using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static string GetMemberId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Member Id not found");
    }
}
