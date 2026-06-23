using ApiMovies.Application.Dtos;
using ApiMovies.Application.Interfaces;
using ApiMovies.Core.Entities;
using ApiMovies.Core.IRepositorio;
using ApiMovies.CrossCutting;
 
using AutoMapper;

namespace ApiMovies.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork contenedorTrabajo;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.contenedorTrabajo = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateMovieAsync(Movie pel)
        {
            this.contenedorTrabajo.Movies.Add(pel);
            await this.contenedorTrabajo.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(Movie pel)
        {
            this.contenedorTrabajo.Movies.Update(pel);
            await this.contenedorTrabajo.SaveChangesAsync();
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            Movie pel = this.contenedorTrabajo.Movies.Get(id);

            if (pel != null)
            {
                this.contenedorTrabajo.Movies.Remove(pel);
                await this.contenedorTrabajo.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public IEnumerable<Movie> GetAllReque()
        {
            return this.contenedorTrabajo.Movies.GetMovies();
        }

        public ApiResponse<PagedResult<MovieDto>> GetAllReque(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            try
            {
                var skip = (page - 1) * pageSize;
                var result = this.contenedorTrabajo.Movies.GetMovies(skip, pageSize);  

                // Mapeo manual Movie -> MovieDto
                var dtoItems = result.Items.Select(m => new MovieDto
                {
                    // Ajusta estos campos a tu Movie/MovieDto reales
                    Id = m.Id,
                    Nombre = m.Nombre,
                    // Descripcion = m.Descripcion,
                    // FechaEstreno = m.FechaEstreno,
                    // ...
                }).ToList();

                var dtoPaged = new PagedResult<MovieDto>
                {
                    Items = dtoItems,
                    TotalRows = result.TotalRows,
                };

                return new ApiResponse<PagedResult<MovieDto>>
                {
                    Success = true,
                    Data = dtoPaged,
                    Message = "OK",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PagedResult<MovieDto>>
                {
                    Success = false,
                    Data = null,
                    Message = $"Error al obtener películas: {ex.Message}",
                };
            }
        }


        public Movie GetPelicula(int id) 
        { 
            return this.contenedorTrabajo.Movies.Get(id);
        }

        public bool ExistePelicula( int id)
        {
            return this.contenedorTrabajo.Movies.Exists(movie => movie.Id == id);
        }

        public PaginatedResponseDto<MovieDto> GetMoviesPaginado(MovieFilterDto filter)
        {
            var movies = this.contenedorTrabajo.Movies.GetMovies().AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                movies = movies.Where(m =>
                    m.Nombre.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(filter.Clasificacion))
            {
                movies = movies.Where(m =>
                    m.Clasificacion.ToString().Equals(filter.Clasificacion, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(filter.Estado))
            {
                movies = movies.Where(m =>
                    m.Estado.Contains(filter.Estado, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.CategoriaId.HasValue)
            {
                movies = movies.Where(m => m.categoriaId == filter.CategoriaId.Value);
            }

            var moviesList = movies.ToList();
            int totalCount = moviesList.Count;

            moviesList = filter.OrderBy?.ToLower() switch
            {
                "date" => moviesList.OrderByDescending(m => m.FechaCreacion).ToList(),
                "name" or _ => moviesList.OrderBy(m => m.Nombre).ToList()
            };

            var moviesPaginadas = moviesList
                .Skip(filter.Offset)
                .Take(filter.Limit)
                .ToList();

            var moviesDto = moviesPaginadas
                .Select(m => _mapper.Map<MovieDto>(m))
                .ToList();

            return new PaginatedResponseDto<MovieDto>(moviesDto, totalCount, filter.Offset, filter.Limit);
        }
    }
}
