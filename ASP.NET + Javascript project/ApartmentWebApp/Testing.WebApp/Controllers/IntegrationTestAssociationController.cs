using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Testing.WebApp.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Testing.WebApp.Controllers;

public class IntegrationTestAssociationController : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public IntegrationTestAssociationController(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false,
                HandleCookies = false
            }
        );
    }
    
    [Fact]
    public async Task Get_Association_Index_Returns_200_and_single_row()
    {
        //Arrange
          
        //Act
        var response = await _client.GetAsync("/Admin/Association");

        //Assert
        response.EnsureSuccessStatusCode();

        var responseContent = await HtmlHelpers.GetDocumentAsync(response);

        var tableRows = responseContent.QuerySelectorAll("id-table-associations-row");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }
}

