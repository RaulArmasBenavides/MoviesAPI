using ApiMovies.Application.Interfaces;
using ApiMovies.Infrastructure.Repositorio.WorkContainer;
using AutoMapper;
using ApiMovies.Core.Entities;
using ApiMovies.Core.IRepositorio;

namespace ApiMovies.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork contenedorTrabajo;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.contenedorTrabajo = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateMovieAsync(Movie pel)
        {
            this.contenedorTrabajo.Movies.Add(pel);
            //_contenedorTrabajo.Save();
            await this.contenedorTrabajo.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(Movie pel)
        {
            this.contenedorTrabajo.Movies.Update(pel);
            await this.contenedorTrabajo.SaveChangesAsync();
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            Movie pel = this.contenedorTrabajo.Movies.Get(id);

            if (pel != null)
            {
                this.contenedorTrabajo.Movies.Remove(pel);
                await this.contenedorTrabajo.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public IEnumerable<object> GetAllReque()
        {
            return this.contenedorTrabajo.Movies.GetMovies();
        }
        public Movie GetPelicula(int id) 
        { 
            return this.contenedorTrabajo.Movies.Get(id);
        }

        public bool ExistePelicula( int id)
        {
            return this.contenedorTrabajo.Movies.Exists(movie => movie.Id == id);
        }
    }
}
