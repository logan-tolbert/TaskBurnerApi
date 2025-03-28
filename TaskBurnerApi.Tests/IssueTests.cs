using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using TaskBurnerAPI.Enums;
using TaskBurnerAPI.Models;

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

        [Fact]
        public async Task GetIssueById_ReturnsNotFoundResult()
        {
            // Act
            var response = await _client.GetAsync("/issues/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task CreateIssue_ReturnsCreatedResult()
        {
            // Act
            var response = await _client.PostAsJsonAsync("/issues", 
                new { 
                        Title = "Test Issue",
                        Body = "This is a test issue"
                    });

            // Assert
            response.EnsureSuccessStatusCode();
        }


        [Fact]
        public async Task CreateIssue_ReturnsCorrectId()
        {
            // Act
            var response = await _client.PostAsJsonAsync("/issues",
           new
            {
               Title = "Id Test",
               Body = "This is a test the id is incremented"
            });
            response.EnsureSuccessStatusCode();
            var issue = await response.Content.ReadFromJsonAsync<Issue>();
            Assert.NotNull(issue);
            Assert.Equal("Id Test", issue.Title);
            Assert.Equal(3, issue.Id);
        }


        [Fact]
        public async Task CreateIssue_ReturnsBackLogStatus()
        {
            // Act
            var response = await _client.PostAsJsonAsync("/issues",
                new
                {
                    Title = "Status Test",
                    Body = "This is an issue status test"
                });

            // Assert
            response.EnsureSuccessStatusCode();
            var issue = await response.Content.ReadFromJsonAsync<Issue>();
            Assert.NotNull(issue);
            Assert.Equal("Status Test", issue.Title);
            Assert.Equal(Status.Backlog, issue.Status);
        }

        [Fact]
        public async Task CreateIssue_ReturnsBadRequestResult()
        {
            // Act
            var response = await _client.PostAsJsonAsync("/issues", new { });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task UpdateIssueStatus_ReturnsOkResult()
        {
            // Arrange
            var issueId = 1;
            var updateData = new Issue
            {
                Id = issueId,
                Title = "Existing Issue",
                Body = "This is an existing issue",
                Status = Status.InProgress
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/issues/{issueId}/status", updateData);

            // Assert
            Assert.True(response.IsSuccessStatusCode, "Expected a successful status code.");
            response.EnsureSuccessStatusCode();
        }


        [Fact]
        public async Task UpdateIssueStatus_ReturnsNotFoundResult()
        {
            // Arrange
            var issueId = 99;
            var updateData = new Issue
            {
                Id = issueId,
                Title = "Existing Issue",
                Body = "This is an existing issue",
                Status = Status.InProgress
            };
            // Act
            var response = await _client.PutAsJsonAsync($"/issues/{issueId}/status", updateData);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // -- Delete --  
        [Fact]
        public async Task DeleteIssue_ReturnsOkResult()
        {
            // Arrange  
            var issueId = 1;

            // Act  
            var response = await _client.DeleteAsync($"/issues/{issueId}");

            // Assert  
            Assert.True(response.IsSuccessStatusCode, "Expected a successful status code.");
            response.EnsureSuccessStatusCode();

        }
        [Fact]
        public async Task DeleteIssue_ReturnsNotFoundResult()
        {
            // Arrange  
            var issueId = 99;

            // Act  
            var response = await _client.DeleteAsync($"/issues/{issueId}");

            // Assert  
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}
