using ApiMovies.Core.Entities;
using System.Reflection.Metadata;

namespace ApiMovies.Infraestructure.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio : IRepository<Pelicula>
    {
        ICollection<Pelicula> GetPeliculas();
        Pelicula GetPelicula(int peliculaId);
        ICollection<Pelicula> GetPeliculasEnCategoria(int catId);
        ICollection<Pelicula> BuscarPelicula(string nombre);
        bool Guardar();
    }
}
