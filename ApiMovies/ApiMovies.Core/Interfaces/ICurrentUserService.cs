namespace ApiMovies.Core.Interfaces
{
    /// <summary>
    /// Exposes the name of the user performing the current request, used for auditing.
    /// </summary>
    public interface ICurrentUserService
    {
        string? UserName { get; }
    }
}
