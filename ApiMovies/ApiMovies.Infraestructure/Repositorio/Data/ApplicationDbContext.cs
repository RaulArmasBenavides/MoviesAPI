using ApiMovies.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUsuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }
        //public DbSet<Usuario> Usuario { get; set; }
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

    public class OracleDBContext :DbContext
    {
        public OracleDBContext(DbContextOptions<OracleDBContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pelicula>()
                .HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.categoriaId)
                .HasConstraintName("FK_Peli_Cat");
        }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }
    }

    public class PostgreSqlContext : IdentityDbContext<AppUsuario>
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options) { }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<AppUsuario> AppUsuario { get; set; }

    }
}