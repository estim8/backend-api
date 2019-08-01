using System;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;

namespace Estim8.Backend.Persistence.Repositories
{
    public interface IGameRepository : IRepository<Game>
    {
        Task<bool> SetGameState(Guid gameId, GameState newState);
        Task<bool> SetGameTimestamp(Guid gameId, DateTimeOffset endedTimestamp);
    }
}