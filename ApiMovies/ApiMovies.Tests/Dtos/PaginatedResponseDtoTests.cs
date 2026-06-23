using System;
using System.Collections.Generic;
using ApiMovies.Application.Dtos;
using Xunit;

namespace ApiMovies.Tests.Dtos
{
    public class PaginatedResponseDtoTests
    {
        [Fact]
        public void PaginatedResponseDto_CalculatesTotalPagesCorrectly()
        {
            // Arrange & Act
            var data = new List<string> { "item1", "item2" };
            var response = new PaginatedResponseDto<string>(data, totalCount: 25, offset: 0, limit: 10);

            // Assert
            Assert.Equal(3, response.TotalPages); // ceil(25/10) = 3
        }

        [Fact]
        public void PaginatedResponseDto_WithExactDivisor_CalculatesTotalPagesCorrectly()
        {
            // Arrange & Act
            var data = new List<string> { "item1", "item2" };
            var response = new PaginatedResponseDto<string>(data, totalCount: 20, offset: 0, limit: 10);

            // Assert
            Assert.Equal(2, response.TotalPages); // 20/10 = 2
        }

        [Fact]
        public void PaginatedResponseDto_WithZeroTotal_ReturnsTotalPagesZero()
        {
            // Arrange & Act
            var data = new List<string>();
            var response = new PaginatedResponseDto<string>(data, totalCount: 0, offset: 0, limit: 10);

            // Assert
            Assert.Equal(0, response.TotalPages);
        }

        [Fact]
        public void PaginatedResponseDto_StoresAllPropertiesCorrectly()
        {
            // Arrange
            var data = new List<int> { 1, 2, 3 };
            const int totalCount = 100;
            const int offset = 20;
            const int limit = 10;

            // Act
            var response = new PaginatedResponseDto<int>(data, totalCount, offset, limit);

            // Assert
            Assert.Equal(data, response.Data);
            Assert.Equal(totalCount, response.TotalCount);
            Assert.Equal(offset, response.Offset);
            Assert.Equal(limit, response.Limit);
            Assert.Equal(10, response.TotalPages); // ceil(100/10) = 10
        }
    }
}
