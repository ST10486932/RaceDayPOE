using Race_Day_2;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RaceDay.Tests;

public class AuthorizationTests : IClassFixture<ApiFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthorizationTests(ApiFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Unauthenticated_request_is_rejected_with_401()
    {
        // Act
        var response = await _client.GetAsync("/api/events"); 

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}