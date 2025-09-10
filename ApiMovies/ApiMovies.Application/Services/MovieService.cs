using ApiMovies.Application.Interfaces;
using ApiMovies.Infrastructure.Repositorio.WorkContainer;
using AutoMapper;
using ApiMovies.Core.Entities;
using ApiMovies.Core.IRepositorio;

namespace ApiMovies.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _contenedorTrabajo;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _contenedorTrabajo = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateMovieAsync(Movie pel)
        {
             _contenedorTrabajo.Movies.Add(pel);
            //_contenedorTrabajo.Save();
            await _contenedorTrabajo.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(Movie pel)
        {
            _contenedorTrabajo.Movies.Update(pel);
            await _contenedorTrabajo.SaveChangesAsync();
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            Movie pel = _contenedorTrabajo.Movies.Get(id);

            if (pel != null)
            {
                _contenedorTrabajo.Movies.Remove(pel);
                await _contenedorTrabajo.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public IEnumerable<object> GetAllReque()
        {
            return _contenedorTrabajo.Movies.GetMovies();
        }


        public Movie GetPelicula(int id) 
        { 
            return _contenedorTrabajo.Movies.Get(id);
        }

        public bool ExistePelicula( int id)
        {
            return _contenedorTrabajo.Movies.Exists(movie => movie.Id == id);
        }


    }
}
