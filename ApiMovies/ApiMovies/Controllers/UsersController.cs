using ApiMovies.Application.Dtos;
using ApiMovies.Application.Dtos.Import;
using ApiMovies.Application.Interfaces;
using ApiMovies.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService usService;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;

        public UsersController(IUserService usservice, IMapper mapper, IConfiguration config, IExcelService excelService)
        {
            usService = usservice;
            _mapper = mapper;
            _excelService = excelService;
        }

        [AllowAnonymous]
        [HttpGet("check-protocol")]
        public IActionResult CheckProtocol()
        {
            return Ok($"Protocol used: {this.HttpContext.Request.Protocol}");
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsers([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            var resultado = this.usService.GetUsuariosPaginado(offset, limit);
            return Ok(resultado);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{usuarioId:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUser(int usuarioId)
        {
            var itemUsuario = this.usService.GetUsuario(usuarioId.ToString());

            if (itemUsuario == null)
            {
                return NotFound();
            }

            var itemUsuarioDto = _mapper.Map<UserDto>(itemUsuario);
            return Ok(itemUsuarioDto);
        }

        [AllowAnonymous]
        [HttpPost("registro")]        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAsync([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {

            var rptaservice= await this.usService.Registro(usuarioRegistroDto);
            //bool validarNombreUsuarioUnico = _usRepo.IsUniqueUser(usuarioRegistroDto.UserName);
            //if (!validarNombreUsuarioUnico)
            //{
            //    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
            //    _respuestaApi.IsSuccess = false;
            //    _respuestaApi.ErrorMessages.Add("El nombre de usuario ya existe");
            //    return BadRequest(_respuestaApi);
            //}

            //var usuario = await _usRepo.Registro(usuarioRegistroDto);
            //if (usuario == null)
            //{
            //    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
            //    _respuestaApi.IsSuccess = false;
            //    _respuestaApi.ErrorMessages.Add("Error en el registro");
            //    return BadRequest(_respuestaApi);
            //}

            //_respuestaApi.StatusCode = HttpStatusCode.OK;
            //_respuestaApi.IsSuccess = true;         
            return Ok(rptaservice);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto usuarioLoginDto)
        {


            var rptaservice = await this.usService.Login(usuarioLoginDto);
            //var respuestaLogin = await _usRepo.Login(usuarioLoginDto);

            //if (respuestaLogin.User == null || string.IsNullOrEmpty(respuestaLogin.Token))
            //{
            //    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
            //    _respuestaApi.IsSuccess = false;
            //    _respuestaApi.ErrorMessages.Add("El nombre de usuario o password son incorrectos");
            //    return BadRequest(_respuestaApi);
            //}

            //_respuestaApi.StatusCode = HttpStatusCode.OK;
            //_respuestaApi.IsSuccess = true;
            //_respuestaApi.Result = respuestaLogin;
            return Ok(rptaservice);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("download-template")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult DownloadUserTemplate()
        {
            var columnNames = new[] { "NombreUsuario", "Nombre" };
            var fileBytes = _excelService.GenerateTemplate<ImportUserDto>(columnNames);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users_template.xlsx");
        }

        [Authorize(Roles = "admin")]
        [HttpPost("bulk-import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkImportUsers(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("El archivo no puede estar vacío");

            try
            {
                var users = _excelService.ImportFromExcel<ImportUserDto>(file.OpenReadStream(), file.FileName);
                int successCount = 0;
                var errors = new List<string>();

                foreach (var userImport in users)
                {
                    try
                    {
                        var registroDto = new UsuarioRegistroDto
                        {
                            NombreUsuario = userImport.NombreUsuario,
                            Nombre = userImport.Nombre,
                            Password = "DefaultPassword123!" // Usuarios deben cambiar al primer login
                        };

                        await this.usService.Registro(registroDto);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Error importando usuario '{userImport.NombreUsuario}': {ex.Message}");
                    }
                }

                var response = new
                {
                    message = "Importación completada",
                    successCount = successCount,
                    totalCount = users.Count,
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
