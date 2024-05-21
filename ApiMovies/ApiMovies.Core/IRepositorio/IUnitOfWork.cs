using ApiMovies.Core.IRepositorio;

namespace ApiMovies.Core.IRepositorio
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoriaRepositorio Categorias { get; }
        IPeliculaRepositorio Peliculas { get; }
        IUsuarioRepositorio Usuarios { get; }
        void Save();
        Task<int> SaveChangesAsync();
    }
}
