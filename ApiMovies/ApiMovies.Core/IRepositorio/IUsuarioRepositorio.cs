using ApiMovies.Core.Entities;

namespace ApiMovies.Core.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<AppUsuario> GetUsuarios();
        AppUsuario GetUsuario(string usuarioId);

        AppUsuario GetUsuarioByUserName(string userName);
        bool IsUniqueUser(string usuario);
   
        //Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        ////Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);

        //Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto);
    }
}
