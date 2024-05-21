using ApiMovies.Application.Interfaces;
using ApiMovies.Core.Entities;
using ApiMovies.Core.IRepositorio;
using ApiMovies.Infraestructure.Repositorio.WorkContainer;
using AutoMapper;

namespace ApiMovies.Application.Services
{
    public class CategoriaSerivce : ICategoriaService
    {

        private readonly IUnitOfWork _contenedorTrabajo;
        private readonly IMapper _mapper;

        public CategoriaSerivce(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _contenedorTrabajo = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateCategoryAsync(Categoria category)
        {
            _contenedorTrabajo.Categorias.Add(category);
            await _contenedorTrabajo.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            _contenedorTrabajo.Categorias.Remove(id);
            await _contenedorTrabajo.SaveChangesAsync();
        }

        public IEnumerable<Categoria> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public Categoria GetCategoria(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCategoryAsync(Categoria cat)
        {
            _contenedorTrabajo.Categorias.Update(cat);
            await _contenedorTrabajo.SaveChangesAsync();
        }
    }
}
