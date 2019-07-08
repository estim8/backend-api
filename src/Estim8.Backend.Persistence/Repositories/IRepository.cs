using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;

namespace Estim8.Backend.Persistence.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<bool> Delete(Guid id);
        Task<TEntity> GetById(Guid id);
        Task Upsert(TEntity entity);
    }
}