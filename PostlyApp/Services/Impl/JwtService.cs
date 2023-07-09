using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PostlyApp.Services.Impl
{
    internal class JwtService : IJwtService
    {
        private readonly JwtSecurityTokenHandler _jwtHandler;
        private JwtSecurityToken? _currentToken;
        public JwtSecurityToken? CurrentToken
        {
            get => _currentToken;
            set
            {
                if (_currentToken != value)
                {
                    _currentToken = value;
                    CurrentTokenChanged.Invoke(value);
                }
            }
        }
        public event Action<JwtSecurityToken?> CurrentTokenChanged;

        public JwtSecurityToken SetCurrentTokenFromString(string tokenString)
        {
            CurrentToken = _jwtHandler.ReadJwtToken(tokenString);
            return CurrentToken;
        }

        public void DeleteCurrentToken()
        {
            _currentToken = null;
        }

        public JwtService()
        {
            _jwtHandler = new();
        }

        public string? GetUserName()
        {
            if (_currentToken == null)
            {
                return null;
            }
            var usernameClaim = _currentToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier, null);
            if (usernameClaim != null)
            {
                return usernameClaim.Value;
            }
            else
            {
                return null;
            }
        }
    }
}
