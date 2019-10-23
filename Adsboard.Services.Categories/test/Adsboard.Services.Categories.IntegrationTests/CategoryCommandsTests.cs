using System;
using System.Threading.Tasks;
using Adsboard.Services.Categories.Domain;
using Adsboard.Services.Categories.IntegrationTests.Config;
using Adsboard.Services.Categories.IntegrationTests.Fixtures;
using Adsboard.Services.Categories.Messages.Commands;
using Adsboard.Services.Categories.Messages.Events;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;

namespace Adsboard.Services.Categories.IntegrationTests
{
    public class CategoryCommandsTest : IClassFixture<MySqlFixture>, IClassFixture<RabbitMqFixture>,
        IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly MySqlFixture _dbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;

        public CategoryCommandsTest(MySqlFixture mySqlFixture, RabbitMqFixture rabbitFixture,
            WebApplicationFactory<Startup> factory)
        {
            _dbFixture = mySqlFixture;
            _rabbitMqFixture = rabbitFixture;
            var client = AppConfig.AddConfiguration(factory).CreateClient();
        }

        [Fact]
        public async Task Create_Category_Command_Should_Create_Entity()
        {
            var user = new User(Guid.NewGuid(), "test@test.com");
            await _dbFixture.InsertAsync(user);

            var command = new CreateCategory(Guid.NewGuid(), "Category 1", user.Id);
            var creationTask = await _rabbitMqFixture.SubscribeAndGetAsync<CategoryCreated, Category>(_dbFixture.GetEntityTask, command.Id);
            await _rabbitMqFixture.PublishAsync(command);
            
            var createdEntity = await creationTask.Task;
            
            createdEntity.Id.ShouldBe(command.Id);
            createdEntity.Name.ShouldBe(command.Name);
            createdEntity.Creator.ShouldBe(command.UserId);
            createdEntity.CreatedAt.ShouldNotBeNull();
        }

        [Fact]
        public async Task Remove_Category_Command_Should_Remove_Entity()
        {
            var user = new User(Guid.NewGuid(), "test@test.com");
            await _dbFixture.InsertAsync(user);

            var category = new Category(Guid.NewGuid(), "Category 1", user.Id);
            await _dbFixture.InsertAsync(category);

            var command = new RemoveCategory(category.Id, category.Creator);
            var archiveTask = await _rabbitMqFixture.SubscribeAndGetAsync<CategoryRemoved, Category>(_dbFixture.GetEntityTask, command.Id);
            await _rabbitMqFixture.PublishAsync(command);

            var removedCategory = await archiveTask.Task;

            removedCategory.ShouldBeNull();
        }

        [Fact]
        public async Task Update_Category_Command_Should_Update_Entity_Name()
        {
            var user = new User(Guid.NewGuid(), "test@test.com");
            await _dbFixture.InsertAsync(user);

            var category = new Category(Guid.NewGuid(), "New Category", user.Id);
            await _dbFixture.InsertAsync(category);

            var command  = new UpdateCategory(category.Id, category.Creator, "Updated Category Name");

            var updateTask = await _rabbitMqFixture.SubscribeAndGetAsync<CategoryUpdated, Category>(_dbFixture.GetEntityTask, command.Id);
            await _rabbitMqFixture.PublishAsync(command);

            var updatedEntity = await updateTask.Task;
            
            updatedEntity.ShouldNotBeNull();
            updatedEntity.Name.ShouldBe(command.Name);
        }
    }
}