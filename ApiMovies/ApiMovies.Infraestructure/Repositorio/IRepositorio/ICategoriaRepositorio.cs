using ApiMovies.Core.Entities;

namespace ApiMovies.Infraestructure.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio : IRepository<Categoria>
    {
        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int categorId);
        bool ExisteCategoria(string nombre);
        bool ExisteCategoria(int id);
        bool CrearCategoria(Categoria categoria);
        bool ActualizarCategoria(Categoria categoria);
        bool BorrarCategoria(Categoria categoria);
        bool Guardar();
    }
}
