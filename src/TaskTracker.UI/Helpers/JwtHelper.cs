using TaskTracker.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TaskTracker.UI.Helpers
{
    public static class JwtHelper
    {
        public static JwtInfo DecodeJwt(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                Guid.TryParse(
                    jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value,
                    out Guid userId
                );
                return new JwtInfo
                {
                    UserId = userId,
                    Role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                    UserName = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value,
                    FirstName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FirstName")?.Value,
                    LastName = jwtToken.Claims.FirstOrDefault(c => c.Type == "LastName")?.Value,
                    Expiration = jwtToken.ValidTo
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
