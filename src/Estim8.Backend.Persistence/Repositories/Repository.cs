using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using Cosmonaut.Response;
using Estim8.Backend.Persistence.Model;
using Serilog;

namespace Estim8.Backend.Persistence.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
	    protected readonly ICosmosStore<TEntity> Store;
	    protected readonly ILogger Logger;
        
	    public Repository(ICosmosStore<TEntity> store, ILogger logger)
	    {
		    Store = store;
		    Logger = logger;
	    }


		public async Task<bool> Delete(Guid id)
		{
			Logger.Information("Trying to delete {Entity} with {Id}", typeof(TEntity).Name, id);

			var r = await Store.RemoveByIdAsync(id.ToString());
			return r.IsSuccess;
		}

		public async Task<IEnumerable<TEntity>> GetPaged(int page, int pageSize)
		{
			return await Store.Query().WithPagination(page * pageSize, pageSize).ToListAsync();
		}
		
		public async Task<ResultPage<TEntity>> GetPaged(string pagingToken, int pageSize)
		{
			var page = await Store.Query().WithPagination(pagingToken, pageSize).ToPagedListAsync();
			
			return new ResultPage<TEntity>(page.Results, page.HasNextPage, page.NextPageToken);
		}

        public async Task<TEntity> GetById(Guid id)
        {
	        return await Store.FindAsync(id.ToString());
        }

        public async Task<bool> Upsert(TEntity entity)
        {
	        var r = await Store.UpsertAsync(entity);
	        
	        Logger.Information("Upserted {@Entity}", entity);
	        return r.IsSuccess;
        }
    }
}