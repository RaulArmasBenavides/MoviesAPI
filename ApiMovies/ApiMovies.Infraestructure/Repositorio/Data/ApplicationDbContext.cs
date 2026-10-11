using ApiMovies.Core.Entities;
using ApiMovies.Core.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ApiMovies.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUsuario>
    {
        private readonly ICurrentUserService? _currentUserService;

        // currentUserService is optional so EF design-time tools (dotnet ef) can create the context.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService? currentUserService = null) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Category> Categoria { get; set; }
        public DbSet<Movie> Pelicula { get; set; }
        //public DbSet<User> User { get; set; }
        public DbSet<AppUsuario> AppUsuario { get; set; }

        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInformation()
        {
            var utcNow = DateTime.UtcNow;
            var userName = _currentUserService?.UserName;

            foreach (EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = utcNow;
                        entry.Entity.CreatedBy = userName;
                        break;

                    case EntityState.Modified:
                        // Creation data must never be overwritten by an update, even if the incoming object lacks it.
                        entry.Property(e => e.CreatedAt).IsModified = false;
                        entry.Property(e => e.CreatedBy).IsModified = false;
                        entry.Entity.UpdatedAt = utcNow;
                        entry.Entity.UpdatedBy = userName;
                        break;
                }
            }
        }
    }

    //public class SqlServerContext : IdentityDbContext<AppUsuario>
    //{
    //    public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options) { }

    //    public DbSet<Category> Category { get; set; }
    //    public DbSet<Movie> Movie { get; set; }
    //    public DbSet<User> User { get; set; }
    //    public DbSet<AppUsuario> AppUsuario { get; set; }
    //}

    public class OracleDBContext :DbContext
    {
        public OracleDBContext(DbContextOptions<OracleDBContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>()
                .HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.categoriaId)
                .HasConstraintName("FK_Peli_Cat");
        }
        public DbSet<Category> Categoria { get; set; }
        public DbSet<Movie> Pelicula { get; set; }
    }

    public class PostgreSqlContext : IdentityDbContext<AppUsuario>
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options) { }
        public DbSet<Category> Categoria { get; set; }
        public DbSet<Movie> Pelicula { get; set; }
        public DbSet<User> Usuario { get; set; }
        public DbSet<AppUsuario> AppUsuario { get; set; }

    }
}