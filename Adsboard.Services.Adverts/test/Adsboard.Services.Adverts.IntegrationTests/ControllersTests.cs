using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;
using Adsboard.Services.Adverts.Domain;
using System;
using Adsboard.Services.Adverts.IntegrationTests.Fixtures;
using Newtonsoft.Json;
using Adsboard.Services.Adverts.Dto;
using Adsboard.Services.Adverts.IntegrationTests.Config;

namespace Adsboard.Services.Adverts.IntegrationTests
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
        [InlineData("adverts")]
        public async Task Given_Endpoints_Should_Return_Success_Http_Status_Code(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);
            response.IsSuccessStatusCode.ShouldBeTrue();
        }

        [Fact]
        public async Task Adverts_Details_Endpoint_Should_Return_Correct_Model()
        {
            string advertDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum bibendum purus et libero vulputate elementum. Quisque hendrerit risus turpis, vitae tristique urna feugiat nec.";
            
            var user = new User(Guid.NewGuid(), "test@test.com");
            var category = new Category(Guid.NewGuid(), user.Id);
            var advert = new Advert(Guid.NewGuid(), "Test Advert Name", advertDescription, user.Id, 
                category.Id, null);

            await _dbFixture.InsertAsync(advert);

            var response = await _client.GetAsync($"adverts/{advert.Id}");
            var contentString = await response.Content.ReadAsStringAsync();
            var advertDetails = JsonConvert.DeserializeObject<AdvertDto>(contentString);

            advertDetails.ShouldNotBeNull();
            advertDetails.Id.ShouldBe(advert.Id);
            advertDetails.Title.ShouldBe(advert.Title);
            advertDetails.Description.ShouldBe(advert.Description);
        }
    }
}
