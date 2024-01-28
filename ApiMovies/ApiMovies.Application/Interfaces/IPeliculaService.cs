using ApiMovies.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiMovies.Application.Interfaces
{
    public interface IPeliculaService
    {
        Task CreateMovieAsync(Pelicula pel);
        Task UpdateMovieAsync(Pelicula pel);
        Task DeleteMovieAsync(int id);
        IEnumerable<object> GetAllReque();
        Pelicula GetPelicula(int id);
        bool ExistePelicula(int id);
    }
}
