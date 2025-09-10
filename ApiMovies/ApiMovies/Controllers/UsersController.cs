﻿using ApiMovies.Application.Dtos;
using ApiMovies.Application.Interfaces;
using ApiMovies.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService usService;
        protected RespuestAPI _respuestaApi;
        private readonly IMapper _mapper;

        public UsersController(IUserService usservice, IMapper mapper,IConfiguration config)
        {
            usService = usservice;
            this._respuestaApi = new();
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("check-protocol")]
        public IActionResult CheckProtocol()
        {
            return Ok($"Protocol used: {this.HttpContext.Request.Protocol}");
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = usService.GetUsuarios();

            var listaUsuariosDto = new List<UserDto>();

            foreach (var lista in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UserDto>(lista));
            }
            return Ok(listaUsuariosDto);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsuario(int usuarioId)
        {
            var itemUsuario = usService.GetUsuario(usuarioId.ToString());

            if (itemUsuario == null)
            {
                return NotFound();
            }

            var itemUsuarioDto = _mapper.Map<UserDto>(itemUsuario);
            return Ok(itemUsuarioDto);
        }

        [AllowAnonymous]
        [HttpPost("registro")]        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {

            var rptaservice= await usService.Registro(usuarioRegistroDto);
            //bool validarNombreUsuarioUnico = _usRepo.IsUniqueUser(usuarioRegistroDto.UserName);
            //if (!validarNombreUsuarioUnico)
            //{
            //    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
            //    _respuestaApi.IsSuccess = false;
            //    _respuestaApi.ErrorMessages.Add("El nombre de usuario ya existe");
            //    return BadRequest(_respuestaApi);
            //}

            //var usuario = await _usRepo.Registro(usuarioRegistroDto);
            //if (usuario == null)
            //{
            //    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
            //    _respuestaApi.IsSuccess = false;
            //    _respuestaApi.ErrorMessages.Add("Error en el registro");
            //    return BadRequest(_respuestaApi);
            //}

            //_respuestaApi.StatusCode = HttpStatusCode.OK;
            //_respuestaApi.IsSuccess = true;         
            return Ok(rptaservice);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto usuarioLoginDto)
        {

            
            var rptaservice = await usService.Login(usuarioLoginDto);
            //var respuestaLogin = await _usRepo.Login(usuarioLoginDto);

            //if (respuestaLogin.User == null || string.IsNullOrEmpty(respuestaLogin.Token))
            //{
            //    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
            //    _respuestaApi.IsSuccess = false;
            //    _respuestaApi.ErrorMessages.Add("El nombre de usuario o password son incorrectos");
            //    return BadRequest(_respuestaApi);
            //}           

            //_respuestaApi.StatusCode = HttpStatusCode.OK;
            //_respuestaApi.IsSuccess = true;
            //_respuestaApi.Result = respuestaLogin;
            return Ok(rptaservice);
        }

    }
}
