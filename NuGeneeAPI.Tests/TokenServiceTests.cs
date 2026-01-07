using Moq;
using Microsoft.Extensions.Configuration;
using NuGeneeAPI.Services;
using NuGeneeAPI.Models.Entities;
using Xunit;

namespace NuGeneeAPI.Tests
{
    public class TokenServiceTests
    {
        [Fact]
        public void CreateToken_ReturnsString()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", "SuperSecretKeyForNuGeneeAcademy2024ByAntigravity"},
                {"Jwt:Issuer", "NuGeneeAPI"},
                {"Jwt:Audience", "NuGeneeAcademy"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            var tokenService = new TokenService(configuration);
            var admin = new Admin { Id = 1, Email = "test@test.com", Name = "Test Admin", Role = "Admin" };

            // Act
            var token = tokenService.CreateToken(admin);

            // Assert
            Assert.NotNull(token);
            Assert.False(string.IsNullOrWhiteSpace(token));
        }
    }
}
