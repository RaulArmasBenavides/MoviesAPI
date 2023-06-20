using ApiMovies.Application.Dtos;
using ApiMovies.Core.Entities;

namespace ApiPeliculas.Infraestructure.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<AppUsuario> GetUsuarios();
        AppUsuario GetUsuario(string usuarioId);
        bool IsUniqueUser(string usuario);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        //Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);

        Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto);
    }
}
