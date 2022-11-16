// using Microsoft.AspNetCore.Mvc.Testing;
// using Xunit;
//
// namespace Test.WebApp.IntegrationTests;
//
// public class IntegrationTestAssociationController: IClassFixture<CustomWebApplicationFactory<Program>>
// {
//     private readonly HttpClient _client;
//     private readonly CustomWebApplicationFactory<Program> _factory;
//
//     public IntegrationTestAssociationController(CustomWebApplicationFactory<Program> factory) 
//     {
//
//         _factory = factory;
//         _client = factory.CreateClient(new WebApplicationFactoryClientOptions
//         {
//             AllowAutoRedirect = false
//         });
//     }
//     
//     [Fact]
//     public async Task Get_Index()
//     {
//         // Arrange
//
//         // Act
//         var response = await _client.GetAsync("/Admin/Association");
//
//         // Assert
//         response.EnsureSuccessStatusCode();
//     }
//     
//     
// }