using ApiMovies.Application.Dtos.Response;
using ApiMovies.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiMovies.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<APIResponse> CreateCategoryAsync(Categoria category);
        Task UpdateCategoryAsync(Categoria category);
        Task DeleteCategoryAsync(int id);
        Categoria GetCategoria(int id);
        IEnumerable<object> GetAllCategories();
        IEnumerable<Categoria> GetAll();
    }
}
