using System;
using System.Collections.Generic;
using ApiMovies.Application.Dtos;
using ApiMovies.Application.Interfaces;
using ApiMovies.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ApiMovies.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CategoriesController(_mockCategoryService.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetCategories_WithDefaultParameters_CallsServiceWithCorrectFilter()
        {
            // Arrange
            var mockResult = new PaginatedResponseDto<CategoryDto>(
                new List<CategoryDto> { new CategoryDto { Id = 1, Nombre = "Test" } },
                totalCount: 1,
                offset: 0,
                limit: 10
            );

            _mockCategoryService
                .Setup(s => s.GetCategoriesPaginado(It.IsAny<CategoryFilterDto>()))
                .Returns(mockResult);

            // Act
            var result = _controller.GetCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PaginatedResponseDto<CategoryDto>>(okResult.Value);
            Assert.Single(returnValue.Data);
            Assert.Equal("Test", returnValue.Data[0].Nombre);
        }

        [Fact]
        public void GetCategories_WithSearchTerm_PassesSearchTermToService()
        {
            // Arrange
            var mockResult = new PaginatedResponseDto<CategoryDto>(
                new List<CategoryDto>(),
                totalCount: 0,
                offset: 0,
                limit: 10
            );

            CategoryFilterDto capturedFilter = null;
            _mockCategoryService
                .Setup(s => s.GetCategoriesPaginado(It.IsAny<CategoryFilterDto>()))
                .Callback<CategoryFilterDto>(f => capturedFilter = f)
                .Returns(mockResult);

            // Act
            _controller.GetCategories(searchTerm: "drama");

            // Assert
            Assert.NotNull(capturedFilter);
            Assert.Equal("drama", capturedFilter.SearchTerm);
        }

        [Fact]
        public void GetCategories_WithOrderBy_PassesOrderByToService()
        {
            // Arrange
            var mockResult = new PaginatedResponseDto<CategoryDto>(
                new List<CategoryDto>(),
                totalCount: 0,
                offset: 0,
                limit: 10
            );

            CategoryFilterDto capturedFilter = null;
            _mockCategoryService
                .Setup(s => s.GetCategoriesPaginado(It.IsAny<CategoryFilterDto>()))
                .Callback<CategoryFilterDto>(f => capturedFilter = f)
                .Returns(mockResult);

            // Act
            _controller.GetCategories(orderBy: "date");

            // Assert
            Assert.NotNull(capturedFilter);
            Assert.Equal("date", capturedFilter.OrderBy);
        }

        [Fact]
        public void GetCategories_WithCustomPagination_PassesOffsetAndLimitToService()
        {
            // Arrange
            var mockResult = new PaginatedResponseDto<CategoryDto>(
                new List<CategoryDto>(),
                totalCount: 0,
                offset: 10,
                limit: 5
            );

            CategoryFilterDto capturedFilter = null;
            _mockCategoryService
                .Setup(s => s.GetCategoriesPaginado(It.IsAny<CategoryFilterDto>()))
                .Callback<CategoryFilterDto>(f => capturedFilter = f)
                .Returns(mockResult);

            // Act
            _controller.GetCategories(offset: 10, limit: 5);

            // Assert
            Assert.NotNull(capturedFilter);
            Assert.Equal(10, capturedFilter.Offset);
            Assert.Equal(5, capturedFilter.Limit);
        }
    }
}
