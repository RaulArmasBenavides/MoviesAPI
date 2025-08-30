using Microsoft.AspNetCore.Identity;
using ApiMovies.Core.Entities;
using ApiMovies.Infrastructure.Data;
using ApiMovies.Infrastructure;
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
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);
        builder.Services.AddCustomHealthChecks();
        builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));
        builder.Services.AddAutoMapper(typeof(PeliculasMapper));
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerDocumentation();
        builder.Services.ConfigureCors();
        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerGen();
        }
        app.UseHttpsRedirection();
        app.UseSerilogRequestLogging();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseCustomCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHealthChecks("/health");
        app.Run();
    } 

}

