using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using App.Contracts.BLL;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit.Abstractions;

namespace Testing.WebApp.ApiControlles;

public class IntegrationTestAssociationApiController : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Mock<IAppBLL> _bllMock;
    

    public IntegrationTestAssociationApiController(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }
        );
        
        _bllMock = new Mock<IAppBLL>();
        
    
    }
    
    [Fact]
    public async Task Get_FooBars_API_Returns_Unauthorized()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/api/v1/Association/");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);


        /*
             response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();

    var resultData = System.Text.Json.JsonSerializer.Deserialize<List<FooBar>>(content);

    Assert.NotNull(resultData);
    Assert.Single(resultData);
    
    
    
    
    
    var responseContent = await HtmlHelpers.GetDocumentAsync(response);

    var tableRows = responseContent.QuerySelectorAll(".id-table-foobars-row");

    
    _testOutputHelper.WriteLine(responseContent.Source.Text);
    
    */
    }

    [Fact]
    public async Task Get_FooBars_API_Returns_Single_Element()
    {
        // Arrange
        var registerDto = new Register()
        {
            Email = "test@test.test",
            Password = "Test1.test",
            FirstName = "TestName",
            LastName = "LastTest"
        };
        var jsonStr = System.Text.Json.JsonSerializer.Serialize(registerDto);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/v1/identity/Account/Register/", data);

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
        apiRequest.RequestUri = new Uri("/api/v1/FooBars");


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