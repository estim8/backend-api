using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;

namespace Estim8.Backend.Persistence.Repositories
{
    public interface IPlayerRepository
    {
        Task AddPlayer(Guid gameId, Guid playerId, string playerName, string gravatar);
        Task DeletePlayer(Guid gameId, Guid playerId);
        Task<Player> GetPlayer(Guid gameId, Guid playerId);
        Task<IEnumerable<Player>> GetAllPlayersInGame(Guid gameId);
    }
}