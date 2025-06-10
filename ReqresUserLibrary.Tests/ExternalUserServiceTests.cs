using Microsoft.Extensions.Caching.Memory;
using Moq;
using ReqresUserLibrary.Clients;
using ReqresUserLibrary.Models;
using ReqresUserLibrary.Services;


namespace ReqresUserLibrary.Tests
{
    public class ExternalUserServiceTests
    {
        private readonly Mock<IReqresApiClient> _mockClient;
        private readonly IMemoryCache _cache;
        private readonly ExternalUserService _service;

        public ExternalUserServiceTests()
        {
            _mockClient = new Mock<IReqresApiClient>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _service = new ExternalUserService(_mockClient.Object, _cache);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            var user = new UserDto { Id = 1, Email = "test@example.com", First_Name = "John", Last_Name = "Doe" };
            _mockClient.Setup(x => x.GetUserByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _service.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result!.First_Name);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllPages()
        {
            // Arrange
            _mockClient.SetupSequence(x => x.GetUsersByPageAsync(It.IsAny<int>()))
                .ReturnsAsync(new PaginatedUsersDto
                {
                    Page = 1,
                    Total_Pages = 2,
                    Data = new List<UserDto> { new() { Id = 1, First_Name = "A" } }
                })
                .ReturnsAsync(new PaginatedUsersDto
                {
                    Page = 2,
                    Total_Pages = 2,
                    Data = new List<UserDto> { new() { Id = 2, First_Name = "B" } }
                });

            // Act
            var users = await _service.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, users.Count());
        }
    }
}