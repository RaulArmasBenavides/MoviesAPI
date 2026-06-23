using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CategorieServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategorieService _categorieService;

        public CategorieServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _categorieService = new CategorieService(
                _mockUnitOfWork.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public void GetCategoriesPaginado_WithSearchTerm_FiltersCorrectly()
        {
            // Arrange
            var categorias = new List<Category>
            {
                new Category { Id = 1, Nombre = "Acción", FechaCreacion = DateTime.Now },
                new Category { Id = 2, Nombre = "Drama", FechaCreacion = DateTime.Now },
                new Category { Id = 3, Nombre = "Comedia de Acción", FechaCreacion = DateTime.Now }
            };

            var filter = new CategoryFilterDto
            {
                SearchTerm = "acción",
                OrderBy = "name",
                Offset = 0,
                Limit = 10
            };

            _mockUnitOfWork
                .Setup(u => u.Categories.GetAll(It.IsAny<System.Linq.Expressions.Expression<System.Func<Category, bool>>>(), It.IsAny<System.Func<IQueryable<Category>, IOrderedQueryable<Category>>>(), It.IsAny<string>()))
                .Returns(categorias);

            _mockMapper
                .Setup(m => m.Map<CategoryDto>(It.IsAny<Category>()))
                .Returns((Category c) => new CategoryDto { Id = c.Id, Nombre = c.Nombre });

            // Act
            var result = _categorieService.GetCategoriesPaginado(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count); // "Acción" y "Comedia de Acción"
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public void GetCategoriesPaginado_OrderByDate_SortsCorrectly()
        {
            // Arrange
            var now = DateTime.Now;
            var categorias = new List<Category>
            {
                new Category { Id = 1, Nombre = "Acción", FechaCreacion = now.AddDays(-2) },
                new Category { Id = 2, Nombre = "Drama", FechaCreacion = now.AddDays(-1) },
                new Category { Id = 3, Nombre = "Comedia", FechaCreacion = now }
            };

            var filter = new CategoryFilterDto
            {
                SearchTerm = "",
                OrderBy = "date",
                Offset = 0,
                Limit = 10
            };

            _mockUnitOfWork
                .Setup(u => u.Categories.GetAll(It.IsAny<System.Linq.Expressions.Expression<System.Func<Category, bool>>>(), It.IsAny<System.Func<IQueryable<Category>, IOrderedQueryable<Category>>>(), It.IsAny<string>()))
                .Returns(categorias);

            _mockMapper
                .Setup(m => m.Map<CategoryDto>(It.IsAny<Category>()))
                .Returns((Category c) => new CategoryDto { Id = c.Id, Nombre = c.Nombre });

            // Act
            var result = _categorieService.GetCategoriesPaginado(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Data.Count);
            Assert.Equal("Comedia", result.Data[0].Nombre); // Más reciente primero
            Assert.Equal("Acción", result.Data[2].Nombre); // Más antiguo último
        }

        [Fact]
        public void GetCategoriesPaginado_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var categorias = new List<Category>();
            for (int i = 1; i <= 25; i++)
            {
                categorias.Add(new Category { Id = i, Nombre = $"Categoría {i}", FechaCreacion = DateTime.Now });
            }

            var filter = new CategoryFilterDto
            {
                SearchTerm = "",
                OrderBy = "name",
                Offset = 10,
                Limit = 10
            };

            _mockUnitOfWork
                .Setup(u => u.Categories.GetAll(It.IsAny<System.Linq.Expressions.Expression<System.Func<Category, bool>>>(), It.IsAny<System.Func<IQueryable<Category>, IOrderedQueryable<Category>>>(), It.IsAny<string>()))
                .Returns(categorias);

            _mockMapper
                .Setup(m => m.Map<CategoryDto>(It.IsAny<Category>()))
                .Returns((Category c) => new CategoryDto { Id = c.Id, Nombre = c.Nombre });

            // Act
            var result = _categorieService.GetCategoriesPaginado(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Data.Count);
            Assert.Equal(25, result.TotalCount);
            Assert.Equal(10, result.Offset);
            Assert.Equal(3, result.TotalPages); // ceil(25/10) = 3
        }

        [Fact]
        public void GetCategoriesPaginado_EmptyResult_ReturnsTotalCountZero()
        {
            // Arrange
            var categorias = new List<Category>();

            var filter = new CategoryFilterDto
            {
                SearchTerm = "NoExiste",
                OrderBy = "name",
                Offset = 0,
                Limit = 10
            };

            _mockUnitOfWork
                .Setup(u => u.Categories.GetAll(It.IsAny<System.Linq.Expressions.Expression<System.Func<Category, bool>>>(), It.IsAny<System.Func<IQueryable<Category>, IOrderedQueryable<Category>>>(), It.IsAny<string>()))
                .Returns(categorias);

            // Act
            var result = _categorieService.GetCategoriesPaginado(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Data);
            Assert.Equal(0, result.TotalCount);
            Assert.Equal(0, result.TotalPages);
        }
    }
}
