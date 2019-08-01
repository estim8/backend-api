using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;

namespace Estim8.Backend.Persistence.Repositories
{
    public interface IRoundRepository
    {
        Task AddRound(Guid gameId, Round round);
        Task<Round> GetById(Guid gameId, Guid roundId);
        Task<Round> GetCurrentRound(Guid gameId);
        Task UpdateRoundTimestamp(Guid gameId, Guid roundId, DateTimeOffset endedTimestamp);
        Task<IEnumerable<Round>> GetAllRounds(Guid gameId);
        Task Delete(Guid gameId, Guid roundId);
    }
}