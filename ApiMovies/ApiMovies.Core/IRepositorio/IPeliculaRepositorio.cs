using ApiMovies.Core.Entities;

namespace ApiMovies.Core.IRepositorio
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
