using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using Serilog.Core;

namespace Estim8.Backend.Persistence.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
	    protected readonly ILogger Logger;
        protected readonly IMongoDatabase DefaultDatabase;
		protected readonly string CollectionName = $"entity-{typeof(TEntity).Name.ToLower()}";
		protected IMongoCollection<TEntity> DefaultCollection => DefaultDatabase.GetCollection<TEntity>(CollectionName);


		protected UpdateDefinitionBuilder<TEntity> Update => Builders<TEntity>.Update;
		protected SortDefinitionBuilder<TEntity> Sort => Builders<TEntity>.Sort;
		protected FilterDefinitionBuilder<TEntity> Filter => Builders<TEntity>.Filter;
		protected ProjectionDefinitionBuilder<TEntity> Projection => Builders<TEntity>.Projection;

		public Repository(IMongoClient client, IOptions<PersistenceConfiguration> config, ILogger logger)
		{
			Logger = logger;
			DefaultDatabase = client.GetDatabase(config.Value.DefaultDatabase);

			if (CollectionExists(DefaultDatabase, CollectionName)) return;
			
			DefaultDatabase.CreateCollection(CollectionName);
				
			var partition = new BsonDocument {
				{"shardCollection", $"{DefaultDatabase}.{CollectionName}"},
				{"key", new BsonDocument {{"_id", "hashed"}}}
			};
			var command = new BsonDocumentCommand<BsonDocument>(partition);
			DefaultDatabase.RunCommand(command);
		}


		public async Task<bool> Delete(Guid id)
		{
			Logger.Information("Trying to delete {Entity} with {Id}", typeof(TEntity).Name, id);
			return (await DefaultCollection.DeleteOneAsync(Filter.Eq(x => x.Id, id)))
				.IsAcknowledged;
		}

		public async Task<IEnumerable<TEntity>> GetAll(int skip = 0, int limit = 100)
		{
			return await DefaultCollection
				.Find(Filter.Empty)
				.Skip(skip)
				.Limit(limit)
				.ToListAsync();
		}

        public async Task<IEnumerable<TEntity>> GetPaged(int page, int pageSize)
        {
            return await GetAll(page * pageSize, pageSize);
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await DefaultCollection.Find(b => b.Id == id).SingleOrDefaultAsync();
        }

        public async Task<Guid> Insert(TEntity entity)
        {
            await DefaultCollection.InsertOneAsync(entity, new InsertOneOptions());
	        
	        Logger.Information("Saved {@Entity}", entity);
            return entity.Id;
        }

        private bool CollectionExists(IMongoDatabase db, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collections = db.ListCollections(new ListCollectionsOptions { Filter = filter });
            return collections.Any();
        }
    }
}