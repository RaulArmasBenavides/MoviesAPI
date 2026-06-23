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
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _userService = new UserService(
                _mockUnitOfWork.Object,
                null,
                null,
                _mockMapper.Object,
                null
            );
        }

        [Fact]
        public void GetUsuariosPaginado_WithValidData_ReturnsPaginatedUsers()
        {
            // Arrange
            var usuarios = new List<AppUsuario>
            {
                new AppUsuario { Id = "1", UserName = "user1", NormalizedEmail = "USER1@TEST.COM", Name = "User One" },
                new AppUsuario { Id = "2", UserName = "user2", NormalizedEmail = "USER2@TEST.COM", Name = "User Two" },
                new AppUsuario { Id = "3", UserName = "user3", NormalizedEmail = "USER3@TEST.COM", Name = "User Three" }
            };

            var userDtos = new List<UserDto>
            {
                new UserDto { Id = "1", UserName = "user1", Nombre = "User One" },
                new UserDto { Id = "2", UserName = "user2", Nombre = "User Two" }
            };

            _mockUnitOfWork
                .Setup(u => u.Users.GetUsuarios())
                .Returns(usuarios);

            _mockMapper
                .Setup(m => m.Map<UserDto>(It.IsAny<AppUsuario>()))
                .Returns((AppUsuario u) => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Nombre = u.Name
                });

            // Act
            var result = _userService.GetUsuariosPaginado(offset: 0, limit: 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(0, result.Offset);
            Assert.Equal(2, result.Limit);
            Assert.Equal(2, result.TotalPages); // ceil(3/2) = 2
        }

        [Fact]
        public void GetUsuariosPaginado_WithOffset_ReturnsCorrectPage()
        {
            // Arrange
            var usuarios = new List<AppUsuario>
            {
                new AppUsuario { Id = "1", UserName = "user1", NormalizedEmail = "USER1@TEST.COM", Name = "User One" },
                new AppUsuario { Id = "2", UserName = "user2", NormalizedEmail = "USER2@TEST.COM", Name = "User Two" },
                new AppUsuario { Id = "3", UserName = "user3", NormalizedEmail = "USER3@TEST.COM", Name = "User Three" }
            };

            _mockUnitOfWork
                .Setup(u => u.Users.GetUsuarios())
                .Returns(usuarios);

            _mockMapper
                .Setup(m => m.Map<UserDto>(It.IsAny<AppUsuario>()))
                .Returns((AppUsuario u) => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Nombre = u.Name
                });

            // Act
            var result = _userService.GetUsuariosPaginado(offset: 2, limit: 2);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Data);
            Assert.Equal("3", result.Data[0].Id);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(2, result.Offset);
        }

        [Fact]
        public void GetUsuariosPaginado_WithNoData_ReturnsEmptyList()
        {
            // Arrange
            var usuarios = new List<AppUsuario>();

            _mockUnitOfWork
                .Setup(u => u.Users.GetUsuarios())
                .Returns(usuarios);

            // Act
            var result = _userService.GetUsuariosPaginado(offset: 0, limit: 10);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Data);
            Assert.Equal(0, result.TotalCount);
            Assert.Equal(0, result.TotalPages);
        }
    }
}
