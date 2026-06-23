using ApiMovies.Application.Dtos;
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

        private readonly IUnitOfWork contenedorTrabajo;
        private readonly IMapper _mapper;

        public CategorieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.contenedorTrabajo = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse> CreateCategoryAsync(Category category)
        {
            this.contenedorTrabajo.Categories.Add(category);
            await this.contenedorTrabajo.SaveChangesAsync();
            return new ApiResponse(200, "Category created");
        }

        public async Task DeleteCategoryAsync(int id)
        {
            this.contenedorTrabajo.Categories.Remove(id);
            await this.contenedorTrabajo.SaveChangesAsync();
        }

        public IEnumerable<Category> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllCategories()
        {
           return this.contenedorTrabajo.Categories.GetAll();
        }
         
        public Category GetCategoria(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCategoryAsync(Category cat)
        {
            this.contenedorTrabajo.Categories.Update(cat);
            await this.contenedorTrabajo.SaveChangesAsync();
        }

        public PaginatedResponseDto<CategoryDto> GetCategoriesPaginado(CategoryFilterDto filter)
        {
            var categorias = this.contenedorTrabajo.Categories.GetAll().AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                categorias = categorias.Where(c =>
                    c.Nombre.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var categoriasList = categorias.ToList();
            int totalCount = categoriasList.Count;

            categoriasList = filter.OrderBy?.ToLower() switch
            {
                "date" => categoriasList.OrderByDescending(c => c.FechaCreacion).ToList(),
                "name" or _ => categoriasList.OrderBy(c => c.Nombre).ToList()
            };

            var categoriasPaginadas = categoriasList
                .Skip(filter.Offset)
                .Take(filter.Limit)
                .ToList();

            var categoriasDto = categoriasPaginadas
                .Select(c => _mapper.Map<CategoryDto>(c))
                .ToList();

            return new PaginatedResponseDto<CategoryDto>(categoriasDto, totalCount, filter.Offset, filter.Limit);
        }
    }
}
