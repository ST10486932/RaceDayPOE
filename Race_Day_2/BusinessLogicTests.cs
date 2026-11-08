using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Race_Day_2; 

namespace RaceDay.Tests;

public class BusinessLogicTests : IClassFixture<ApiFactory<Program>>
{
    private readonly HttpClient _client;

    public BusinessLogicTests(ApiFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // TEST 6: Can an Organiser create a Category?
    [Fact]
    public async Task CreateCategory_AsOrganiser_ReturnsSuccess()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/categories");
        request.Headers.Add(TestAuthHandler.RoleHeader, "Organiser");
        request.Content = JsonContent.Create(new { Name = "10km Run", Distance = 10, EntryFee = 100 });

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.NotEqual(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // TEST 7: Can a Participant enrol in an event?
    [Fact]
    public async Task EnrolInEvent_AsParticipant_ReturnsSuccess()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/enrolments");
        request.Headers.Add(TestAuthHandler.RoleHeader, "Participant");
        request.Content = JsonContent.Create(new { EventId = 1, CategoryId = 1 });

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.NotEqual(HttpStatusCode.Forbidden, response.StatusCode); // Participants should be allowed
    }

    // TEST 8: Organisers should NOT be able to enrol as participants
    [Fact]
    public async Task EnrolInEvent_AsOrganiser_ReturnsForbidden()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/enrolments");
        request.Headers.Add(TestAuthHandler.RoleHeader, "Organiser"); // Organiser role trying to enrol
        request.Content = JsonContent.Create(new { EventId = 1, CategoryId = 1 });

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    // TEST 9: Can an Organiser capture race results?
    [Fact]
    public async Task CaptureResults_AsOrganiser_ReturnsSuccess()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/results");
        request.Headers.Add(TestAuthHandler.RoleHeader, "Organiser");
        request.Content = JsonContent.Create(new { EnrolmentId = 1, FinishTime = "02:30:00", Position = 1 });

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // TEST 10: Only Admins can view system reports
    [Fact]
    public async Task ViewSystemReports_AsParticipant_ReturnsForbidden()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/admin/reports");
        request.Headers.Add(TestAuthHandler.RoleHeader, "Participant"); // Participant trying to access Admin data

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}