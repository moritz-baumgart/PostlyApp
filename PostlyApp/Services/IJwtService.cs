using System.IdentityModel.Tokens.Jwt;

namespace PostlyApp.Services
{
    public interface IJwtService
    {
        public JwtSecurityToken SetCurrentTokenFromString(string tokenString);
        public void DeleteCurrentToken();
        public event Action<JwtSecurityToken?> CurrentTokenChanged;
        public string? GetUserName();
    }
}
