using ApiMovies.Application.Dtos;
using ApiMovies.Application.Interfaces;
using ApiMovies.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{

    //[ApiController]
    //[Route("api/[controller]")]//Una opción
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _ctService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService ctService, IMapper mapper)
        {
            _ctService = ctService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias() 
        { 
            var listaCategorias = _ctService.GetAllCategories();
            //var listaCategoriasDto = new List<CategoryDto>();

            //foreach (var lista in listaCategorias)
            //{
            //    listaCategoriasDto.Add(_mapper.Map<CategoryDto>(lista));
            //}
            return Ok(listaCategorias);
        }

        [AllowAnonymous]
        [HttpGet("{categoriaId:int}", Name = "GetCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int categoriaId)
        {
            var  itemCategoria = _ctService.GetCategoria(categoriaId);

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
        public async Task<IActionResult> CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
        {

            //if (_ctService.ExisteCategoria(crearCategoriaDto.Name))
            //{
            //    ModelState.AddModelError("", "La categoría ya existe");
            //    return StatusCode(404, ModelState);
            //}

            var categoria = _mapper.Map<Category>(crearCategoriaDto);
            var responseDto  = await _ctService.CreateCategoryAsync(categoria);
            return Ok(responseDto);
            //return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
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

        //    var categoria = _ctRepo.GetCategoria(categoriaId);

        //    if (!_ctRepo.BorrarCategoria(categoria))
        //    {
        //        ModelState.AddModelError("", $"Algo salió mal borrando el registro{categoria.Name}");
        //        return StatusCode(500, ModelState);
        //    }
        //    return NoContent();
        //}
    }
}
