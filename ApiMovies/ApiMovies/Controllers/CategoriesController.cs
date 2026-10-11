using ApiMovies.Application.Dtos;
using ApiMovies.Application.Dtos.Import;
using ApiMovies.Application.Interfaces;
using ApiMovies.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService ctService;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;

        public CategoriesController(ICategoryService mctService, IMapper mapper, IExcelService excelService)
        {
            ctService = mctService;
            _mapper = mapper;
            _excelService = excelService;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategories(
            [FromQuery] string searchTerm = "",
            [FromQuery] string orderBy = "name",
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 10)
        {
            var filter = new CategoryFilterDto
            {
                SearchTerm = searchTerm,
                OrderBy = orderBy,
                Offset = offset,
                Limit = limit
            };

            var resultado = this.ctService.GetCategoriesPaginado(filter);
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpGet("{categoriaId:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory(int categoriaId)
        {
            var  itemCategoria = this.ctService.GetCategoria(categoriaId);

            if (itemCategoria == null)
            {
                return NotFound();
            }

            var itemCategoriaDto = _mapper.Map<CategoryDto>(itemCategoria);           
            return Ok(itemCategoriaDto);
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearCategoria([FromBody] CreateCategoryDto crearCategoriaDto)
        {

            //if (_ctService.ExisteCategoria(crearCategoriaDto.Name))
            //{
            //    ModelState.AddModelError("", "La categoría ya existe");
            //    return StatusCode(404, ModelState);
            //}

            var categoria = _mapper.Map<Category>(crearCategoriaDto);
            var responseDto  = await this.ctService.CreateCategoryAsync(categoria);
            return Ok(responseDto);
            //return CreatedAtRoute("GetCategory", new { categoriaId = categoria.Id }, categoria);
        }

        //[Authorize(Roles = "admin")]
        //[HttpPatch("{categoriaId:int}", Name = "ActualizarPatchCategoria")]
        //[ProducesResponseType(201, Type = typeof(CategoryDto))]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]       
        //public async Task<IActionResult> ActualizarPatchCategoria(int categoriaId, [FromBody] CategoryDto categoriaDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (categoriaDto == null || categoriaId != categoriaDto.Id)
        //    {
        //        return BadRequest(ModelState);
        //    }         

        //    var categoria = _mapper.Map<Category>(categoriaDto);
        //    await _ctService.UpdateCategoryAsync(categoria);
        //    //if (!)
        //    //{
        //    //    ModelState.AddModelError("", $"Algo salió mal actualizando el registro{categoria.Name}");
        //    //    return StatusCode(500, ModelState);
        //    //}
        //    return NoContent();
        //}

        //[Authorize(Roles = "admin")]
        //[HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public IActionResult BorrarCategoria(int categoriaId)
        //{
        //    if (!_ctRepo.ExisteCategoria(categoriaId))
        //    {
        //        return NotFound();
        //    }

        //    var categoria = _ctRepo.GetCategory(categoriaId);

        //    if (!_ctRepo.BorrarCategoria(categoria))
        //    {
        //        ModelState.AddModelError("", $"Algo salió mal borrando el registro{categoria.Name}");
        //        return StatusCode(500, ModelState);
        //    }
        //    return NoContent();
        //}

        [Authorize(Roles = "admin")]
        [HttpGet("download-template")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult DownloadCategoryTemplate()
        {
            var columnNames = new[] { "Nombre" };
            var fileBytes = _excelService.GenerateTemplate<ImportCategoryDto>(columnNames);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "categories_template.xlsx");
        }

        [Authorize(Roles = "admin")]
        [HttpPost("bulk-import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkImportCategories(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("El archivo no puede estar vacío");

            try
            {
                var categories = _excelService.ImportFromExcel<ImportCategoryDto>(file.OpenReadStream(), file.FileName);
                int successCount = 0;
                var errors = new List<string>();

                foreach (var categoryImport in categories)
                {
                    try
                    {
                        var category = new Category
                        {
                            Nombre = categoryImport.Nombre
                        };

                        await this.ctService.CreateCategoryAsync(category);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Error importando '{categoryImport.Nombre}': {ex.Message}");
                    }
                }

                var response = new
                {
                    message = "Importación completada",
                    successCount = successCount,
                    totalCount = categories.Count,
                    errors = errors
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
