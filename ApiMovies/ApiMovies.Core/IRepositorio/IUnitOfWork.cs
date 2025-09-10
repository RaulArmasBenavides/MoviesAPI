using ApiMovies.Core.IRepositorio;

namespace ApiMovies.Core.IRepositorio
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categorias { get; }
        IMovieRepository Peliculas { get; }
        IUserRepository Usuarios { get; }
        void Save();
        Task<int> SaveChangesAsync();
    }
}
