using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CachingFramework.Redis.Contracts;
using CachingFramework.Redis.Contracts.RedisObjects;
using Estim8.Backend.Persistence.Model;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Estim8.Backend.Persistence.Repositories
{
    public class RoundRepository : RedisRepository<Round>, IRoundRepository
    {
        public RoundRepository(IContext redisContext, ISerializer serializer, ILoggerFactory loggerFactory) : base(redisContext, serializer, loggerFactory)
        {
        }

        public async Task AddRound(Guid gameId, Round round)
        {
            await Redis.Cache.SetHashedAsync(ToHashKey(gameId), ToKey(round.Id), round);
        }
        
        public async Task<Round> GetById(Guid gameId, Guid roundId)
        {
            return await Redis.Cache.GetHashedAsync<Round>(ToHashKey(gameId), ToKey(roundId));
        }

        public async Task<Round> GetCurrentRound(Guid gameId)
        {
            var allRounds = await Redis.Cache.GetHashedAllAsync<Round>(ToHashKey(gameId));
            return allRounds.Values.OrderByDescending(x => x.StartedTimestamp).FirstOrDefault(x => x.EndedTimestamp.HasValue);
        }

        public async Task UpdateRoundTimestamp(Guid gameId, Guid roundId, DateTimeOffset endedTimestamp)
        {
            var trans = Database.CreateTransaction();
            var round = await Redis.Cache.GetHashedAsync<Round>(ToHashKey(gameId), ToKey(roundId));
            
            trans.AddCondition(Condition.HashEqual(ToHashKey(gameId), ToKey(roundId), Serializer.Serialize(round)));

            round.EndedTimestamp = endedTimestamp;
            
#pragma warning disable 4014
            trans.HashSetAsync(ToHashKey(gameId), ToKey(roundId), Serializer.Serialize(round));
#pragma warning restore 4014
            
            await trans.ExecuteAsync();
        }

        public async Task<IEnumerable<Round>> GetAllRounds(Guid gameId)
        {
            return (await Redis.Cache.GetHashedAllAsync<Round>(ToHashKey(gameId))).Values;
        }

        public async Task Delete(Guid gameId, Guid roundId)
        {
            await Redis.Cache.RemoveHashedAsync(ToHashKey(gameId), ToKey(roundId));
        }

        private string ToHashKey(Guid gameId)
        {
            return $"game:id:{gameId}:rounds:hash";
        }
    }
}