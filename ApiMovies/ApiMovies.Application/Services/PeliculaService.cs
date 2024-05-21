using ApiMovies.Application.Interfaces;
using ApiMovies.Infraestructure.Repositorio.WorkContainer;
using AutoMapper;
using ApiMovies.Core.Entities;
using ApiMovies.Core.IRepositorio;

namespace ApiMovies.Application.Services
{
    public class PeliculaService : IPeliculaService
    {
        private readonly IUnitOfWork _contenedorTrabajo;
        private readonly IMapper _mapper;

        public PeliculaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _contenedorTrabajo = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateMovieAsync(Pelicula pel)
        {
             _contenedorTrabajo.Peliculas.Add(pel);
            //_contenedorTrabajo.Save();
            await _contenedorTrabajo.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(Pelicula pel)
        {
            _contenedorTrabajo.Peliculas.Update(pel);
            await _contenedorTrabajo.SaveChangesAsync();
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            Pelicula pel = _contenedorTrabajo.Peliculas.Get(id);

            if (pel != null)
            {
                _contenedorTrabajo.Peliculas.Remove(pel);
                await _contenedorTrabajo.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public IEnumerable<object> GetAllReque()
        {
            return _contenedorTrabajo.Peliculas.GetPeliculas();
        }


        public Pelicula GetPelicula(int id) 
        { 
            return _contenedorTrabajo.Peliculas.Get(id);
        }

        public bool ExistePelicula( int id)
        {
            return _contenedorTrabajo.Peliculas.Exists(movie => movie.Id == id);
        }


    }
}
