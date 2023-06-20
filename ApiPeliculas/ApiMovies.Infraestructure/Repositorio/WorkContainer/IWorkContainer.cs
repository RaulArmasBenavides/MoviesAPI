using ApiMovies.Core.Entities;
using ApiPeliculas.Infraestructure.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiMovies.Infraestructure.Repositorio.WorkContainer
{
    public interface IWorkContainer
    {
        ICategoriaRepositorio Categorias { get; }

        IPeliculaRepositorio Peliculas { get; }

        IUsuarioRepositorio Usuarios { get; }

        void Save();

        Task<int> SaveChangesAsync();
    }
}
