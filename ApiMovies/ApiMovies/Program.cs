using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using ApiMovies.Core.Entities;
using ApiMovies.Repositorio;
using ApiMovies.Infraestructure.Repositorio.IRepositorio;
using ApiMovies.Infraestructure.Data;
using ApiMovies.Infraestructure;
using ApiMovies.Application.Interfaces;
using ApiMovies.Application.Services;
using Serilog;
using ApiMovies.Middlewares;
using ApiMovies.CrossCutting.PeliculasMapper;
using ApiMovies.Extensions;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //Soporte para autenticación con .NET Identity
        builder.Services.AddIdentity<AppUsuario, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
        //Agregamos los repositorios
        builder.Services.AddScoped<IPeliculaService, PeliculaService>();
        builder.Services.AddScoped<IUsuarioService, UsuarioService>();
        builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
        builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
        builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
        builder.Services.AddPersistence(builder.Configuration);
        builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));
        var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");
        builder.Services.AddAutoMapper(typeof(PeliculasMapper));
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
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

