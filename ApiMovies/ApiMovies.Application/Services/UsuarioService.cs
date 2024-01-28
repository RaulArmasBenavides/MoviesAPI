using ApiMovies.Application.Dtos;
using ApiMovies.Application.Interfaces;
using ApiMovies.Core.Entities;
using ApiMovies.Infraestructure.Repositorio.WorkContainer;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiMovies.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsuario> _userManager;
        private readonly IWorkContainer _contenedorTrabajo;
        private IConfiguration _config;

        private readonly RoleManager<IdentityRole> _roleManager;
        public UsuarioService(IWorkContainer unitOfWork, IConfiguration config, UserManager<AppUsuario> userManager, IMapper mapper , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;

            _mapper = mapper;
            _contenedorTrabajo = unitOfWork;
            _userManager = userManager;
            _config = config;
            _roleManager = roleManager;
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto, string SecretKey)
        {
            var usuario = _contenedorTrabajo.Usuarios.GetUsuarioByUserName(usuarioLoginDto.NombreUsuario.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(usuario, usuarioLoginDto.Password);
            //Validamos si el usuario no existe con la combinación de usuario y contraseña correcta
            if (usuario == null || !isValid )
            {
                //return null;
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }
            //Aquí existe el usuario entonces podemos procesar el login
            var roles = await _userManager.GetRolesAsync(usuario);
            var manejadorToken = new JwtSecurityTokenHandler();

           
            var key = Encoding.ASCII.GetBytes(SecretKey); 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, usuario.UserName.ToString()),
                    new(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = manejadorToken.CreateToken(tokenDescriptor);
            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario),

            };
            return usuarioLoginRespuestaDto;
        }
        public async Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            AppUsuario usuario = new AppUsuario()
            {
                UserName = usuarioRegistroDto.NombreUsuario,
                Email = usuarioRegistroDto.NombreUsuario,
                NormalizedEmail = usuarioRegistroDto.NombreUsuario.ToUpper(),
                Nombre = usuarioRegistroDto.Nombre
            };
            var result = await _userManager.CreateAsync(usuario, usuarioRegistroDto.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));
                }
                await _userManager.AddToRoleAsync(usuario, "admin");
                var usuarioRetornado = _contenedorTrabajo.Usuarios.GetUsuarioByUserName(usuarioRegistroDto.NombreUsuario);
                return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);
            }
            return new UsuarioDatosDto();
        }

        public  ICollection<AppUsuario> GetUsuarios()
        {
            return _contenedorTrabajo.Usuarios.GetUsuarios();
        }

        public AppUsuario GetUsuario(string id)
        {
            return _contenedorTrabajo.Usuarios.GetUsuario(id);
        }
    }
}