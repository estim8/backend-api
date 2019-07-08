using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;

namespace Estim8.Backend.Persistence.Repositories
{
    public interface IGameRoundRepository
    {
        Task AddRound(Guid gameId, GameRound round);
        Task Replace(Guid gameId, Guid roundId, GameRound round);
        Task<GameRound> GetById(Guid gameId, Guid roundId);
        Task<GameRound> GetCurrentRound(Guid gameId);
        Task<IEnumerable<GameRound>> GetAllRounds(Guid gameId);
        Task Delete(Guid gameId, Guid roundId);
    }
}