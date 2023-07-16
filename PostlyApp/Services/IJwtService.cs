using System.IdentityModel.Tokens.Jwt;

namespace PostlyApp.Services
{
    public interface IJwtService
    {
        /// <summary>
        /// Sets the current token by reading the given string value.
        /// </summary>
        /// <returns>The newly set <see cref="JwtSecurityToken"/></returns>
        public JwtSecurityToken SetCurrentTokenFromString(string tokenString);

        /// <summary>
        /// Sets the current token to null.
        /// </summary>
        public void DeleteCurrentToken();

        /// <summary>
        /// An event invoked everytime the current jwt token changes.
        /// </summary>
        public event Action<JwtSecurityToken?> CurrentTokenChanged;

        /// <summary>
        /// Retrieves the current username.
        /// </summary>
        /// <returns>The current username or null if the current token is null or there is no username claim.</returns>
        public string? GetUserName();
    }
}
