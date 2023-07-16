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

        /// <summary>
        /// An event invoked everytime the current jwt token changes.
        /// </summary>
        public event Action<JwtSecurityToken?> CurrentTokenChanged;

        /// <summary>
        /// Sets the current token by reading the given string value.
        /// </summary>
        /// <returns>The newly set <see cref="JwtSecurityToken"/></returns>
        public JwtSecurityToken SetCurrentTokenFromString(string tokenString)
        {
            CurrentToken = _jwtHandler.ReadJwtToken(tokenString);
            return CurrentToken;
        }

        /// <summary>
        /// Sets the current token to null.
        /// </summary>
        public void DeleteCurrentToken()
        {
            _currentToken = null;
        }

        public JwtService()
        {
            _jwtHandler = new();
        }

        /// <summary>
        /// Retrieves the current username.
        /// </summary>
        /// <returns>The current username or null if the current token is null or there is no username claim.</returns>
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
