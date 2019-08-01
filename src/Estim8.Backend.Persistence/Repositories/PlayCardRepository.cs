using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CachingFramework.Redis.Contracts;
using Estim8.Backend.Persistence.Model;
using Microsoft.Extensions.Logging;

namespace Estim8.Backend.Persistence.Repositories
{
    public class PlayCardRepository : RedisRepository<PlayedCard>, IPlayCardRepository
    {
        public PlayCardRepository(IContext redisContext, ISerializer serializer, ILoggerFactory loggerFactory) : base(redisContext, serializer, loggerFactory)
        {
        }

        public async Task PlayCard(Guid gameId, Guid playerId, Guid roundId, PlayedCard card)
        {
            await Redis.Cache.SetHashedAsync(ToHashKey(gameId), ToKey(roundId, playerId), card, when: When.NotExists);
        }

        public async Task CancelCard(Guid gameId, Guid playerId, Guid roundId)
        {
            await Redis.Cache.RemoveHashedAsync(ToHashKey(gameId), ToKey(roundId, playerId));
        }

        public Task<IEnumerable<PlayedCard>> GetPlayedCardsInRound(Guid gameId, Guid roundId)
        {
            return Task.FromResult(Redis.Cache.ScanHashed<PlayedCard>(ToHashKey(gameId), $"round:id:{roundId}:*", 100).Select(x => x.Value));
        }
        
        public async Task<IEnumerable<PlayedCard>> GetPlayedCardsInGame(Guid gameId)
        {
            return (await Redis.Cache.GetHashedAllAsync<PlayedCard>(ToHashKey(gameId))).Values;
        }

        private string ToHashKey(Guid gameId)
        {
            return $"game:id:{gameId}:cards:hash";
        }

        private string ToKey(Guid roundId, Guid playerId)
        {
            return $"round:id:{roundId}:player:id:{playerId}";
        }
    }
}