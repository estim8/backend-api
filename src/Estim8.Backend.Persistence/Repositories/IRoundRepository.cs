using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;

namespace Estim8.Backend.Persistence.Repositories
{
    public interface IRoundRepository
    {
        Task AddRound(Guid gameId, Round round);
        Task Replace(Guid gameId, Guid roundId, Round round);
        Task<Round> GetById(Guid gameId, Guid roundId);
        Task<Round> GetCurrentRound(Guid gameId);
        Task<IEnumerable<Round>> GetAllRounds(Guid gameId);
        Task Delete(Guid gameId, Guid roundId);
    }
}