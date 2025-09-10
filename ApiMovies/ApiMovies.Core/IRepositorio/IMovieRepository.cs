using ApiMovies.Core.Entities;

namespace ApiMovies.Core.IRepositorio
{
    public interface IMovieRepository : IRepository<Movie>
    {
        ICollection<Movie> GetMovies();
        Movie GetMovie(int peliculaId);
        ICollection<Movie> GetPeliculasEnCategoria(int catId);
        ICollection<Movie> SearchMovie(string nombre);
        bool Save();
    }
}
