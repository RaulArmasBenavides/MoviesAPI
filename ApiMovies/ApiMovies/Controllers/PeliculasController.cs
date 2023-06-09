﻿using ApiMovies.Application.Dtos;
using ApiMovies.Application.Interfaces;
using ApiMovies.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaService _pelService;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        public PeliculasController(IPeliculaService pelServ, IMapper mapper, Serilog.ILogger logger)
        {
            _pelService = pelServ;
            _mapper = mapper;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas()
        {
            _logger.Information("test");
            var listaPeliculas = _pelService.GetAllReque();

            var listaPeliculasDto = new List<PeliculaDto>();

            foreach (var lista in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(lista));
            }
            return Ok(listaPeliculasDto);
        }

        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = _pelService.GetPelicula(peliculaId);

            if (itemPelicula == null)
            {
                return NotFound();
            }
            var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);
            return Ok(itemPeliculaDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateMovie([FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (peliculaDto == null)
            {
                return BadRequest(ModelState);
            }

            //if (_pelService.ExistePelicula(peliculaDto.Nombre))
            //{
            //    ModelState.AddModelError("", "La película ya existe");
            //    return StatusCode(404, ModelState);
            //}
            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            //if (!_pelService.CreateMovieAsync(pelicula))
            //{
            //    ModelState.AddModelError("", $"Algo salió mal guardando el registro{pelicula.Nombre}");
            //    return StatusCode(500, ModelState);
            //}
            _pelService.CreateMovieAsync(pelicula);
            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{peliculaId:int}", Name = "UpdateMovie")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMovie(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            _pelService.UpdateMovieAsync(pelicula);
            return NoContent();
        }

        //[Authorize(Roles = "admin")]
        //[HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public IActionResult BorrarPelicula(int peliculaId)
        //{
        //    if (!_pelRepo.ExistePelicula(peliculaId))
        //    {
        //        return NotFound();
        //    }

        //    var pelicula = _pelRepo.GetPelicula(peliculaId);

        //    if (!_pelRepo.BorrarPelicula(pelicula))
        //    {
        //        ModelState.AddModelError("", $"Algo salió mal borrando el registro{pelicula.Nombre}");
        //        return StatusCode(500, ModelState);
        //    }
        //    return NoContent();
        //}

        //[AllowAnonymous]
        //[HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]      
        //public IActionResult GetPeliculasEnCategoria(int categoriaId)
        //{
        //    var listaPeliculas = _pelRepo.GetPeliculasEnCategoria(categoriaId);

        //    if (listaPeliculas == null)
        //    {
        //        return NotFound();
        //    }

        //    var itemPelicula = new List<PeliculaDto>();

        //    foreach (var item in listaPeliculas)
        //    {
        //        itemPelicula.Add(_mapper.Map<PeliculaDto>(item));
        //    }
        //    return Ok(itemPelicula);
        //}

        //[AllowAnonymous]
        //[HttpGet("Buscar")]
        //public IActionResult Buscar(string nombre)
        //{
        //    try
        //    {
        //        var resultado = _pelRepo.BuscarPelicula(nombre.Trim());
        //        if (resultado.Any())
        //        {
        //            return Ok(resultado);
        //        }

        //        return NotFound();
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
        //    }
        //}
    }
}
