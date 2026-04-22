using TaskTracker.Models.Auth;

namespace TaskTracker.UI.Helpers
{
    public static class SessionHelper
    {
        public static void SetUserSession(HttpContext context, JwtInfo jwtInfo, string token)
        {
            context.Session.SetString("Token", token);
            context.Session.SetString("UserId", jwtInfo.UserId.ToString());
            context.Session.SetString("UserRole", jwtInfo.Role ?? "");
            context.Session.SetString("UserName", jwtInfo.UserName ?? "");
            context.Session.SetString("Name", $"{jwtInfo.FirstName} {jwtInfo.LastName}".Trim());
            context.Session.SetString("TokenExpiry", jwtInfo.Expiration.ToString("O"));
        }
    }

}
