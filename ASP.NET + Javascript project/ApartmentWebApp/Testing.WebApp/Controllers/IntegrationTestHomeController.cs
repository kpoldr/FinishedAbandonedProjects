using Microsoft.AspNetCore.Mvc.Testing;

namespace Testing.WebApp.Controllers;

public class IntegrationTestHomeController : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public IntegrationTestHomeController(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(
            new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            }

        );
    }


    
    [Fact]
    public async Task Get_Index()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/");

        // Assert
        response.EnsureSuccessStatusCode();
    }

}