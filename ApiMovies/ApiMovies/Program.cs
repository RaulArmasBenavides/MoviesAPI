using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using ApiMovies.Core.Entities;
using ApiMovies.Repositorio;
using ApiMovies.Infrastructure.Data;
using ApiMovies.Infrastructure;
using ApiMovies.Application.Interfaces;
using ApiMovies.Application.Services;
using Serilog;
using ApiMovies.Middlewares;
using ApiMovies.CrossCutting.PeliculasMapper;
using ApiMovies.Extensions;
using ApiMovies.Core.IRepositorio;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //Soporte para autenticación con .NET Identity
        builder.Services.AddIdentity<AppUsuario, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);
        builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));
        builder.Services.AddAutoMapper(typeof(PeliculasMapper));
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerDocumentation();
        //Soporte para CORS
        //Se pueden habilitar: 1-Un dominio, 2-multiples dominios,
        //3-cualquier dominio (Tener en cuenta seguridad)
        //Usamos de ejemplo el dominio: http://localhost:3223, se debe cambiar por el correcto
        //Se usa (*) para todos los dominios
        builder.Services.AddCors(p => p.AddPolicy("PolicyCors", build =>
        {
            build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        }));

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerGen();
        }
        app.UseHttpsRedirection();
        app.UseSerilogRequestLogging();
        app.UseMiddleware<ExceptionMiddleware>();
        //Soporte para CORS
        app.UseCors("PolicyCors");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    } 

}

