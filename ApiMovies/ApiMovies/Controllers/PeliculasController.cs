﻿using ApiMovies.Application.Dtos;
using ApiMovies.Application.Interfaces;
using ApiMovies.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaService _pelService;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        public PeliculasController(IPeliculaService pelServ, 
                                   IMapper mapper, 
                                   Serilog.ILogger logger,
                                   IWebHostEnvironment environment)
        {
            _pelService = pelServ;
            _mapper = mapper;
            _logger = logger;
            _environment = environment;
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
        public async Task<IActionResult> CreateMovie([FromBody] PeliculaDto peliculaDto)
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
            await _pelService.CreateMovieAsync(pelicula);
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

        [Authorize(Roles = "admin")]
        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BorrarPeliculaAsync(int peliculaId)
        {
            if (!_pelService.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            //var pelicula = _pelService.GetPelicula(peliculaId);
             var res =await _pelService.DeleteMovieAsync(peliculaId);
            if (!res)
            {
                ModelState.AddModelError("", $"Algo salió mal borrando la película");
               return StatusCode(500, ModelState);
            }
            return NoContent();
        }

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


        //[AllowAnonymous]
        //[HttpPost("UploadImageReq")]
        //public async Task<ActionResult> UploadImageReq()
        //{
        //    bool res = false;
        //    var uploadedfiles = Request.Form.Files;
        //    foreach (IFormFile item in uploadedfiles)
        //    {
        //        string Filename = item.FileName;
        //        int idreq = Convert.ToInt32(Request.Form["nroreq"]);
        //        string xtension = string.Empty;
        //        xtension = FileHelper.GetExtension(item.FileName);

        //        Guid newguid = Guid.NewGuid();
        //        Filename = newguid.ToString().ToLower();
        //        string Filepath = this._environment.WebRootPath + $"\\uploads\\reqs\\{idreq}";
        //        if (!Directory.Exists(Filepath))
        //        {
        //            Directory.CreateDirectory(Filepath);
        //        }
        //        string imagepath = Filepath + "\\" + Filename + ".jpeg";
        //        if (System.IO.File.Exists(imagepath))
        //        {
        //            System.IO.File.Delete(imagepath);
        //        }
        //        using FileStream stream = System.IO.File.Create(imagepath);
        //        ReqActivo _req = _RequeService.GetReque(idreq);
        //        _req.ImageGUID = idreq.ToString();
        //        await this._RequeService.UpdateRequeAdminAsync(_req);
        //        await item.CopyToAsync(stream);
        //        res = true;
        //    }
        //    return Ok(res);
        //}
    }
}
