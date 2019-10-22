using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;
using System;
using Newtonsoft.Json;
using Adsboard.Services.Adverts.IntegrationTests.Config;
using Adsboard.Services.Categories.IntegrationTests.Fixtures;
using Adsboard.Services.Categories.Domain;
using Adsboard.Services.Categories.Dto;

namespace Adsboard.Services.Categories.IntegrationTests
{
    public class ControllersTests : IClassFixture<MySqlFixture>,
        IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly MySqlFixture _dbFixture;
        private readonly HttpClient _client;


        public ControllersTests(WebApplicationFactory<Startup> factory, MySqlFixture dbFixture)
        {
            _client = AppConfig.AddConfiguration(factory).CreateClient();
            _dbFixture = dbFixture;
        }

        [Theory]
        [InlineData("")]
        [InlineData("ping")]
        [InlineData("categories")]
        public async Task Given_Endpoints_Should_Return_Success_Http_Status_Code(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);
            response.IsSuccessStatusCode.ShouldBeTrue();
        }

        [Fact]
        public async Task Adverts_Details_Endpoint_Should_Return_Correct_Model()
        {            
            var category = new Category(Guid.NewGuid(), "Category 1", Guid.NewGuid());

            await _dbFixture.InsertAsync(category);

            var response = await _client.GetAsync($"categories/{category.Id}");
            var contentString = await response.Content.ReadAsStringAsync();
            var categoryDetails = JsonConvert.DeserializeObject<CategoryDto>(contentString);

            categoryDetails.ShouldNotBeNull();
            categoryDetails.Id.ShouldBe(category.Id);
            categoryDetails.Name.ShouldBe(category.Name);
        }
    }
}
