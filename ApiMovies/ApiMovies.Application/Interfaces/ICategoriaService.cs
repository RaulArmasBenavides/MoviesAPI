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
        Task CreateCategoryAsync(Categoria pel);
        Task UpdateCategoryAsync(Categoria pel);
        Task DeleteCategoryAsync(int id);
    }
}
