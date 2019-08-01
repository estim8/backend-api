using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;

namespace Estim8.Backend.Persistence.Repositories
{
    public interface IPlayCardRepository
    {
        Task PlayCard(Guid gameId, Guid playerId, Guid roundId, PlayedCard card);
        Task CancelCard(Guid gameId, Guid playerId, Guid roundId);
        Task<IEnumerable<PlayedCard>> GetPlayedCardsInGame(Guid gameId);
        Task<IEnumerable<PlayedCard>> GetPlayedCardsInRound(Guid gameId, Guid roundId);
    }
}