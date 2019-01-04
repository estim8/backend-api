using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;

namespace Estim8.Backend.Persistence.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task<bool> Delete(Guid id);
        Task<IEnumerable<TEntity>> GetAll(int skip = 0, int limit = 100);
        Task<IEnumerable<TEntity>> GetPaged(int page, int pageSize);
        Task<TEntity> GetById(Guid id);
        Task<Guid> Insert(TEntity entity);
    }
}