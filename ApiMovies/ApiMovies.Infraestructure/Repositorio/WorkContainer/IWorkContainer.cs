using ApiMovies.Core.IRepositorio;

namespace ApiMovies.Infraestructure.Repositorio.WorkContainer
{
    public interface IWorkContainer : IDisposable
    {
        ICategoriaRepositorio Categorias { get; }
        IPeliculaRepositorio Peliculas { get; }
        IUsuarioRepositorio Usuarios { get; }
        void Save();
        Task<int> SaveChangesAsync();
    }
}
