﻿using ApiMovies.Application.Interfaces;
using ApiMovies.Infraestructure.Repositorio.WorkContainer;
using ApiMovies.Infraestructure.Repositorio.IRepositorio;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IEnumerable<object> GetAllReque()
        {
            return _contenedorTrabajo.Peliculas.GetPeliculas();
        }
    }
}
