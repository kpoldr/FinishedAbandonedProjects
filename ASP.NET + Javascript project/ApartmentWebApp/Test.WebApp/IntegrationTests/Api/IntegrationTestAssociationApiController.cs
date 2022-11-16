using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Test.WebApp.IntegrationTests.Api;

public class IntegrationTestAssociationApiController: IClassFixture<CustomWebApplicationFactory<Program>>
{
    
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;


    public IntegrationTestAssociationApiController(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }
        );
    }
    
    [Fact]
    public async Task Get_FooBars_API_Returns_Unauthorized()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/api/v1.0/Association");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        
    }

    
    [Fact]
    public async Task Get_FooBars_API_Returns_Single_Element()
    {
        // Arrange
        var registerDto = new Register()
        {
            Email = "test1@test.test",
            FirstName = "test",
            LastName = "test",
            Password = "Test1.test"
        };
        var jsonStr = System.Text.Json.JsonSerializer.Serialize(registerDto);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/v1/identity/account/register", data);

        response.EnsureSuccessStatusCode();

        var requestContent = await response.Content.ReadAsStringAsync();

        var resultJwt = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(
            requestContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}
        );


        var apiRequest = new HttpRequestMessage();
        apiRequest.Method = HttpMethod.Get;
        apiRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", resultJwt!.Token);
        apiRequest.RequestUri = new Uri("/api/v1.0/Association");


        // Act
        var apiResponse = await _client.SendAsync(apiRequest);

        // Assert
        apiResponse.EnsureSuccessStatusCode();
        
        var content = await apiResponse.Content.ReadAsStringAsync();
        var resultData = System.Text.Json.JsonSerializer.Deserialize<List<Association>>(content);
        Assert.NotNull(resultData);
        Assert.Single(resultData);
    }

    
    
    
}