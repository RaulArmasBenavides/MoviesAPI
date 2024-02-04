using ApiMovies.Infraestructure.Repositorio;
using ApiMovies.Infraestructure.Repositorio.IRepositorio;
using ApiMovies.Infraestructure.Repositorio.WorkContainer;
using ApiMovies.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace ApiMovies.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
         IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConexionSQL"),
                  b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)), 
                  ServiceLifetime.Scoped);

            // Configuración para Oracle
            services.AddDbContext<OracleContext>(options =>
                options.UseOracle(
                    configuration.GetConnectionString("ConexionOracle"),
                    b => b.MigrationsAssembly(typeof(OracleContext).Assembly.FullName)),
                ServiceLifetime.Scoped);

            // Configuración para PostgreSQL
            //services.AddDbContext<PostgreSqlContext>(options =>
            //    options.UseNpgsql(
            //        configuration.GetConnectionString("ConexionPostgreSQL"),
            //        b => b.MigrationsAssembly(typeof(PostgreSqlContext).Assembly.FullName)),
            //    ServiceLifetime.Scoped);
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IWorkContainer, WorkContainer>();
            return services;
        }
    }
}