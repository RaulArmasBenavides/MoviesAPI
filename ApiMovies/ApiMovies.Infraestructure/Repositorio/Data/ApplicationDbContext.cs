using ApiMovies.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Infraestructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUsuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<AppUsuario> AppUsuario { get; set; }
    }

    //public class SqlServerContext : IdentityDbContext<AppUsuario>
    //{
    //    public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options) { }

    //    public DbSet<Categoria> Categoria { get; set; }
    //    public DbSet<Pelicula> Pelicula { get; set; }
    //    public DbSet<Usuario> Usuario { get; set; }
    //    public DbSet<AppUsuario> AppUsuario { get; set; }
    //}

    public class OracleContext : IdentityDbContext<AppUsuario>
    {
        public OracleContext(DbContextOptions<OracleContext> options) : base(options) { }

 
    }

    public class PostgreSqlContext : IdentityDbContext<AppUsuario>
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options) { }

 
    }
}