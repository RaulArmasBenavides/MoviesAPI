using ApiMovies.Application.Dtos.Response;
using ApiMovies.Application.Interfaces;
using ApiMovies.Core.Entities;
using ApiMovies.Core.IRepositorio;
using ApiMovies.Infrastructure.Repositorio.WorkContainer;
using AutoMapper;

namespace ApiMovies.Application.Services
{
    public class CategorieService : ICategoryService
    {

        private readonly IUnitOfWork _contenedorTrabajo;
        private readonly IMapper _mapper;

        public CategorieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _contenedorTrabajo = unitOfWork;
            _mapper = mapper;
        }
        public async Task<APIResponse> CreateCategoryAsync(Category category)
        {
            _contenedorTrabajo.Categories.Add(category);
            await _contenedorTrabajo.SaveChangesAsync();
            return new APIResponse(200, "Category created");
        }

        public async Task DeleteCategoryAsync(int id)
        {
            _contenedorTrabajo.Categories.Remove(id);
            await _contenedorTrabajo.SaveChangesAsync();
        }

        public IEnumerable<Category> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllCategories()
        {
           return _contenedorTrabajo.Categories.GetAll();
        }
         
        public Category GetCategoria(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCategoryAsync(Category cat)
        {
            _contenedorTrabajo.Categories.Update(cat);
            await _contenedorTrabajo.SaveChangesAsync();
        }
    }
}
