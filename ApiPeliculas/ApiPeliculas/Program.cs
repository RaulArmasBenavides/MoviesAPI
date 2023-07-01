using ApiMovies.PeliculasMapper;
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

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //Configuramos la conexion a sql server
        //builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
        //{
        //    opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"));
        //});

        //Soporte para autenticación con .NET Identity
        builder.Services.AddIdentity<AppUsuario, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

        //Agregamos los repositorios
        builder.Services.AddScoped<IPeliculaService, PeliculaService>();
        builder.Services.AddScoped<IUsuarioService, UsuarioService>();
        builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
        builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
        builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
        builder.Services.AddPersistence(builder.Configuration);
        var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");

        //Agregar el AutoMapper
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


        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                "Autenticación JWT usando el esquema Bearer. \r\n\r\n " +
                "Ingresa la palabra 'Bearer' seguida de un [espacio] y despues su token en el campo de abajo \r\n\r\n" +
                "Ejemplo: \"Bearer tkdknkdllskd\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
        });


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
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        //Soporte para CORS
        app.UseCors("PolicyCors");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    } 

}

