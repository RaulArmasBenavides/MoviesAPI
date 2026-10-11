using ApiMovies.Core.Interfaces;

namespace ApiMovies.Services
{
    /// <summary>
    /// Reads the authenticated user name from the current HTTP request (JWT "name" claim).
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}
