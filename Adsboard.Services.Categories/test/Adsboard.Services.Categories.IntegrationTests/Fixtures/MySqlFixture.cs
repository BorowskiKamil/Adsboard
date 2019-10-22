using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Adsboard.Common.Domain;
using Adsboard.Services.Categories.Data;

namespace Adsboard.Services.Categories.IntegrationTests.Fixtures
{
    public class MySqlFixture : IDisposable
    {
        private readonly DbContextOptions<ApplicationContext> _contextOptions;
        private bool _disposed = false;


        public MySqlFixture()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseMySql("Server=localhost;Port=3307;Database=adsboard_categories_test;Uid=root;Pwd=root;");
            _contextOptions = optionsBuilder.Options;

            EnsureDeleted();
        }

        private void EnsureDeleted()
        {
            using (var context = new ApplicationContext(_contextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }

        public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            using (var context = new ApplicationContext(_contextOptions))
            {
                context.Add(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<TEntity> GetEntity<TEntity>(Guid id) where TEntity : Entity
        {
            using (var context = new ApplicationContext(_contextOptions))
            {
                return await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task GetEntityTask<TEntity>(Guid expectedId, TaskCompletionSource<TEntity> receivedTask) where TEntity : Entity
        {
            if (expectedId == null)
            {
                throw new ArgumentNullException(nameof(expectedId));
            }

            using (var context = new ApplicationContext(_contextOptions))
            {
                var entity = await context.Set<TEntity>().FirstOrDefaultAsync(d => d.Id == expectedId);
                
                if (entity is null)
                {
                    receivedTask.TrySetCanceled();
                    return;
                }
                
                receivedTask.TrySetResult(entity);
            }
        }

        
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            
            if (disposing)
            {
                EnsureDeleted();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}