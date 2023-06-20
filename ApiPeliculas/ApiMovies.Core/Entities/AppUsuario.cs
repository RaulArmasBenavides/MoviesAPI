using Microsoft.AspNetCore.Identity;

namespace ApiMovies.Core.Entities
{
    public class AppUsuario : IdentityUser
    {

        public string Nombre { get; set; }
    }
}
