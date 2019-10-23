using System;
using System.IO;
using System.Threading.Tasks;
using Adsboard.Services.Adverts.Domain;
using Adsboard.Services.Adverts.IntegrationTests.Config;
using Adsboard.Services.Adverts.IntegrationTests.Fixtures;
using Adsboard.Services.Adverts.Messages.Commands;
using Adsboard.Services.Adverts.Messages.Events;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Shouldly;
using Xunit;

namespace Adsboard.Services.Adverts.IntegrationTests
{
    public class AdvertCommandsTest : IClassFixture<MySqlFixture>, IClassFixture<RabbitMqFixture>,
        IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly MySqlFixture _dbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;

        public AdvertCommandsTest(MySqlFixture mySqlFixture, RabbitMqFixture rabbitFixture,
            WebApplicationFactory<Startup> factory)
        {
            _dbFixture = mySqlFixture;
            _rabbitMqFixture = rabbitFixture;
            var client = AppConfig.AddConfiguration(factory).CreateClient();
        }

        [Fact]
        public async Task Create_Advert_Command_Should_Create_Entity()
        {
            var user = new User(Guid.NewGuid(), "test@test.com");
            await _dbFixture.InsertAsync(user);

            var category = new Category(Guid.NewGuid(), user.Id);
            await _dbFixture.InsertAsync(category);

            string advertDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum bibendum purus et libero vulputate elementum.";
            var command = new CreateAdvert(Guid.NewGuid(), "Test title", advertDescription, user.Id, category.Id, null);
            var creationTask = await _rabbitMqFixture.SubscribeAndGetAsync<AdvertCreated, Advert>(_dbFixture.GetEntityTask, command.Id);
            await _rabbitMqFixture.PublishAsync(command);
            
            var createdEntity = await creationTask.Task;
            
            createdEntity.ShouldNotBeNull();
            createdEntity.Id.ShouldBe(command.Id);
            createdEntity.Title.ShouldBe(command.Title);
            createdEntity.Description.ShouldBe(command.Description);
            createdEntity.Image.ShouldBe(command.Image);
            createdEntity.Creator.ShouldBe(user.Id);
            createdEntity.Category.ShouldBe(category.Id);
        }

        [Fact]
        public async Task Archive_Advert_Command_Should_Archive_Entity()
        {
            var user = new User(Guid.NewGuid(), "test@test.com");
            await _dbFixture.InsertAsync(user);

            var category = new Category(Guid.NewGuid(), user.Id);
            await _dbFixture.InsertAsync(category);

            string advertDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum bibendum purus et libero vulputate elementum.";
            var advert = new Advert(Guid.NewGuid(), "Test Advert Name", advertDescription, user.Id, 
                category.Id, null);
            await _dbFixture.InsertAsync(advert);

            var command = new ArchiveAdvert(advert.Id, advert.Creator);
            var archiveTask = await _rabbitMqFixture.SubscribeAndGetAsync<AdvertArchived, Advert>(_dbFixture.GetEntityTask, command.Id);
            await _rabbitMqFixture.PublishAsync(command);

            var archivedEntity = await archiveTask.Task;

            archivedEntity.ShouldNotBeNull();
            archivedEntity.Id.ShouldBe(command.Id);
            archivedEntity.ArchivedAt.ShouldNotBeNull();
        }

        [Fact]
        public async Task Update_Advert_Command_Should_Update_Entity_Title_And_Description()
        {
            var user = new User(Guid.NewGuid(), "test@test.com");
            await _dbFixture.InsertAsync(user);

            var category = new Category(Guid.NewGuid(), user.Id);
            await _dbFixture.InsertAsync(category);

            string advertDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum bibendum purus et libero vulputate elementum. Quisque hendrerit risus turpis, vitae tristique urna feugiat nec.";
            var advert = new Advert(Guid.NewGuid(), "Test Advert Name", advertDescription, user.Id, 
                category.Id, null);
            await _dbFixture.InsertAsync(advert);

            string newDescription = "Vestibulum bibendum purus et libero vulputate elementum. Quisque hendrerit risus turpis, vitae tristique urna feugiat nec.";
            var command  = new UpdateAdvert(advert.Id, "Updated Ad Title", newDescription, advert.Creator);

            var updateTask = await _rabbitMqFixture.SubscribeAndGetAsync<AdvertUpdated, Advert>(_dbFixture.GetEntityTask, command.Id);
            await _rabbitMqFixture.PublishAsync(command);

            var updatedEntity = await updateTask.Task;
            
            updatedEntity.ShouldNotBeNull();
            updatedEntity.Title.ShouldBe(command.Title);
            updatedEntity.Description.ShouldBe(command.Description);
        }
    }
}