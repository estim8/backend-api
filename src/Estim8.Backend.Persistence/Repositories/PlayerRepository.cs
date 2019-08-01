using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CachingFramework.Redis.Contracts;
using Estim8.Backend.Persistence.Model;
using Microsoft.Extensions.Logging;

namespace Estim8.Backend.Persistence.Repositories
{
    public class PlayerRepository : RedisRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(IContext redisContext, ISerializer serializer, ILoggerFactory loggerFactory) : base(redisContext, serializer, loggerFactory)
        {
            
        }
        
        public async Task AddPlayer(Guid gameId, Guid playerId)
        {
            await Redis.Cache.SetHashedAsync(ToHashKey(gameId), ToKey(playerId), new Player{Id = playerId});
        }

        public async Task DeletePlayer(Guid gameId, Guid playerId)
        {
            await Redis.Cache.RemoveHashedAsync(ToHashKey(gameId), ToKey(playerId));
        }

        public async Task<Player> GetPlayer(Guid gameId, Guid playerId)
        {
            return await Redis.Cache.GetHashedAsync<Player>(ToHashKey(gameId), ToHashKey(playerId));
        }

        public async Task<IEnumerable<Player>> GetAllPlayersInGame(Guid gameId)
        {
            var result = await Redis.Cache.GetHashedAllAsync<Player>(ToHashKey(gameId));
            return result.Select(x => x.Value);
        }

        private string ToHashKey(Guid gameId)
        {
            return $"game:id:{gameId}:players:hash";
        }
    }
}