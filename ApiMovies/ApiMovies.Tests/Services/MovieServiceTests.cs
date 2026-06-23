using System;
using System.Collections.Generic;
using ApiMovies.Application.Dtos;
using ApiMovies.Application.Interfaces;
using ApiMovies.Application.Services;
using ApiMovies.Core.Entities;
using ApiMovies.Core.IRepositorio;
using AutoMapper;
using Moq;
using Xunit;

namespace ApiMovies.Tests.Services
{
    public class MovieServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _movieService = new MovieService(
                _mockUnitOfWork.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public void GetMoviesPaginado_WithSearchTerm_FiltersCorrectly()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Nombre = "Avatar", Descripcion = "Sci-Fi", Estado = "Activa", categoriaId = 1, Clasificacion = Movie.TipoClasificacion.Dieciocho, FechaCreacion = DateTime.Now },
                new Movie { Id = 2, Nombre = "Titanic", Descripcion = "Drama", Estado = "Activa", categoriaId = 2, Clasificacion = Movie.TipoClasificacion.Dieciseis, FechaCreacion = DateTime.Now },
                new Movie { Id = 3, Nombre = "Avatar 2", Descripcion = "Sci-Fi Sequel", Estado = "Activa", categoriaId = 1, Clasificacion = Movie.TipoClasificacion.Dieciocho, FechaCreacion = DateTime.Now }
            };

            var filter = new MovieFilterDto
            {
                SearchTerm = "avatar",
                Offset = 0,
                Limit = 10
            };

            _mockUnitOfWork
                .Setup(u => u.Movies.GetMovies())
                .Returns(movies);

            _mockMapper
                .Setup(m => m.Map<MovieDto>(It.IsAny<Movie>()))
                .Returns((Movie m) => new MovieDto { Id = m.Id, Nombre = m.Nombre });

            // Act
            var result = _movieService.GetMoviesPaginado(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count); // Avatar y Avatar 2
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public void GetMoviesPaginado_FilterByClasificacion_ReturnsFiltered()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Nombre = "Avatar", Descripcion = "Sci-Fi", Estado = "Activa", categoriaId = 1, Clasificacion = Movie.TipoClasificacion.Dieciocho, FechaCreacion = DateTime.Now },
                new Movie { Id = 2, Nombre = "Titanic", Descripcion = "Drama", Estado = "Activa", categoriaId = 2, Clasificacion = Movie.TipoClasificacion.Dieciseis, FechaCreacion = DateTime.Now },
            };

            var filter = new MovieFilterDto
            {
                Clasificacion = "Dieciocho",
                Offset = 0,
                Limit = 10
            };

            _mockUnitOfWork
                .Setup(u => u.Movies.GetMovies())
                .Returns(movies);

            _mockMapper
                .Setup(m => m.Map<MovieDto>(It.IsAny<Movie>()))
                .Returns((Movie m) => new MovieDto { Id = m.Id, Nombre = m.Nombre });

            // Act
            var result = _movieService.GetMoviesPaginado(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Data);
            Assert.Equal("Avatar", result.Data[0].Nombre);
        }

        [Fact]
        public void GetMoviesPaginado_FilterByCategoria_ReturnsFiltered()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Nombre = "Avatar", Descripcion = "Sci-Fi", Estado = "Activa", categoriaId = 1, Clasificacion = Movie.TipoClasificacion.Dieciocho, FechaCreacion = DateTime.Now },
                new Movie { Id = 2, Nombre = "Titanic", Descripcion = "Drama", Estado = "Activa", categoriaId = 2, Clasificacion = Movie.TipoClasificacion.Dieciseis, FechaCreacion = DateTime.Now },
                new Movie { Id = 3, Nombre = "The Martian", Descripcion = "Sci-Fi", Estado = "Activa", categoriaId = 1, Clasificacion = Movie.TipoClasificacion.Trece, FechaCreacion = DateTime.Now }
            };

            var filter = new MovieFilterDto
            {
                CategoriaId = 1,
                Offset = 0,
                Limit = 10
            };

            _mockUnitOfWork
                .Setup(u => u.Movies.GetMovies())
                .Returns(movies);

            _mockMapper
                .Setup(m => m.Map<MovieDto>(It.IsAny<Movie>()))
                .Returns((Movie m) => new MovieDto { Id = m.Id, Nombre = m.Nombre, categoriaId = m.categoriaId });

            // Act
            var result = _movieService.GetMoviesPaginado(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count);
            Assert.All(result.Data, m => Assert.Equal(1, m.categoriaId));
        }

        [Fact]
        public void GetMoviesPaginado_OrderByDate_SortsCorrectly()
        {
            // Arrange
            var now = DateTime.Now;
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Nombre = "Old Movie", Descripcion = "Old", Estado = "Activa", categoriaId = 1, Clasificacion = Movie.TipoClasificacion.Siete, FechaCreacion = now.AddDays(-10) },
                new Movie { Id = 2, Nombre = "Recent Movie", Descripcion = "Recent", Estado = "Activa", categoriaId = 1, Clasificacion = Movie.TipoClasificacion.Siete, FechaCreacion = now },
                new Movie { Id = 3, Nombre = "Middle Movie", Descripcion = "Middle", Estado = "Activa", categoriaId = 1, Clasificacion = Movie.TipoClasificacion.Siete, FechaCreacion = now.AddDays(-5) }
            };

            var filter = new MovieFilterDto
            {
                OrderBy = "date",
                Offset = 0,
                Limit = 10
            };

            _mockUnitOfWork
                .Setup(u => u.Movies.GetMovies())
                .Returns(movies);

            _mockMapper
                .Setup(m => m.Map<MovieDto>(It.IsAny<Movie>()))
                .Returns((Movie m) => new MovieDto { Id = m.Id, Nombre = m.Nombre });

            // Act
            var result = _movieService.GetMoviesPaginado(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Data.Count);
            Assert.Equal("Recent Movie", result.Data[0].Nombre); // Más reciente primero
            Assert.Equal("Old Movie", result.Data[2].Nombre); // Más antiguo último
        }

        [Fact]
        public void GetMoviesPaginado_CombinedFilters_AppliesAll()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Nombre = "Avatar 1", Descripcion = "Sci-Fi", Estado = "Activa", categoriaId = 1, Clasificacion = Movie.TipoClasificacion.Dieciocho, FechaCreacion = DateTime.Now.AddDays(-5) },
                new Movie { Id = 2, Nombre = "Avatar 2", Descripcion = "Sci-Fi", Estado = "Activa", categoriaId = 1, Clasificacion = Movie.TipoClasificacion.Dieciocho, FechaCreacion = DateTime.Now },
                new Movie { Id = 3, Nombre = "Titanic", Descripcion = "Drama", Estado = "Activa", categoriaId = 2, Clasificacion = Movie.TipoClasificacion.Dieciocho, FechaCreacion = DateTime.Now }
            };

            var filter = new MovieFilterDto
            {
                SearchTerm = "avatar",
                CategoriaId = 1,
                Clasificacion = "Dieciocho",
                OrderBy = "date",
                Offset = 0,
                Limit = 10
            };

            _mockUnitOfWork
                .Setup(u => u.Movies.GetMovies())
                .Returns(movies);

            _mockMapper
                .Setup(m => m.Map<MovieDto>(It.IsAny<Movie>()))
                .Returns((Movie m) => new MovieDto { Id = m.Id, Nombre = m.Nombre });

            // Act
            var result = _movieService.GetMoviesPaginado(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal("Avatar 2", result.Data[0].Nombre); // Ordenado por fecha, más reciente primero
            Assert.Equal("Avatar 1", result.Data[1].Nombre);
        }
    }
}
