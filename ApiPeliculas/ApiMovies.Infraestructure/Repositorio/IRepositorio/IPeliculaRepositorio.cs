using ApiMovies.Core.Entities;
using System.Reflection.Metadata;

namespace ApiMovies.Infraestructure.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio : IRepository<Pelicula>
    {
        ICollection<Pelicula> GetPeliculas();
        Pelicula GetPelicula(int peliculaId);
        bool ExistePelicula(string nombre);
        bool ExistePelicula(int id);
        bool CrearPelicula(Pelicula pelicula);
        bool BorrarPelicula(Pelicula pelicula);

        //Métodos para buscar pelicualas en categoría y buscar película por nombre
        ICollection<Pelicula> GetPeliculasEnCategoria(int catId);
        ICollection<Pelicula> BuscarPelicula(string nombre);

        bool Guardar();
    }
}
