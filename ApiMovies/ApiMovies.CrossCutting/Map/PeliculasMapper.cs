
using ApiMovies.Application.Dtos;
using ApiMovies.Core.Entities;
using AutoMapper;

namespace ApiMovies.CrossCutting.PeliculasMapper
{
    public class PeliculasMapper : Profile
    {
        public PeliculasMapper()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CrearCategoriaDto>().ReverseMap();
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<AppUsuario, UserDto>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDatosDto>().ReverseMap();
        }
    }
}
