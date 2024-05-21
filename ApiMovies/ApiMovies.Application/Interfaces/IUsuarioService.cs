using ApiMovies.Application.Dtos;
using ApiMovies.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiMovies.Application.Interfaces
{
    public interface IUsuarioService
    {

        Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto);

        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);

        ICollection<AppUsuario> GetUsuarios();

        AppUsuario GetUsuario(string id);
    }
}
