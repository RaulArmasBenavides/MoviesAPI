using ApiMovies.Application.Interfaces;
using ApiMovies.Infraestructure.Repositorio.WorkContainer;
using ApiMovies.Infraestructure.Repositorio.IRepositorio;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiMovies.Core.Entities;

namespace ApiMovies.Application.Services
{
    public class PeliculaService : IPeliculaService
    {
        private readonly IWorkContainer _contenedorTrabajo;
        private readonly IMapper _mapper;

        public PeliculaService(IWorkContainer unitOfWork, IMapper mapper)
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

        public async Task DeleteMovieAsync(int id)
        {
            _contenedorTrabajo.Peliculas.Remove(id);
            await _contenedorTrabajo.SaveChangesAsync();
        }

        public IEnumerable<object> GetAllReque()
        {
            return _contenedorTrabajo.Peliculas.GetPeliculas();
        }


        public Pelicula GetPelicula(int id) 
        { 
            return _contenedorTrabajo.Peliculas.Get(id);
        }


    }
}
