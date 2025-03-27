using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace TaskBurnerAPI.Tests
{
    public class IssueTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task GetAllIssues_ReturnsOkResult()
        {
            // Act
            var response = await _client.GetAsync("/issues");

            // Assert
            Assert.True(response.IsSuccessStatusCode, "Expected a successful status code.");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetIssueById_ReturnsOkResult()
        {
            // Act
            var response = await _client.GetAsync("/issues/1");

            // Assert
            Assert.True(response.IsSuccessStatusCode, $"Expected a successful status code.");
            response.EnsureSuccessStatusCode();
        }
    }
}
