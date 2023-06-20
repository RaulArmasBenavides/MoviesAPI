using ApiMovies.Core.Entities;
using ApiPeliculas.Infraestructure.Data;
using ApiPeliculas.Infraestructure.Repositorio.IRepositorio;
using ApiPeliculas.Repositorio;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiMovies.Infraestructure.Repositorio.WorkContainer
{
    public class WorkContainer : IWorkContainer
    {
        private readonly ApplicationDbContext _db;
        public WorkContainer() {
            _db = db;
            Categorias = new CategoriaRepositorio(_db);
            Peliculas = new PeliculaRepositorio(_db);
            Usuarios = new UsuarioRepositorio(_db);
        }
        public ICategoriaRepositorio Categorias { get; private set; }
        public IPeliculaRepositorio Peliculas { get; private set; }
        public IUsuarioRepositorio Usuarios { get; private set; }

        void IWorkContainer.Save()
        {
            throw new NotImplementedException();
        }

        Task<int> IWorkContainer.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
