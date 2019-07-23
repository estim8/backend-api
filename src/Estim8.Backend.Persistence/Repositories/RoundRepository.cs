using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CachingFramework.Redis.Contracts;
using CachingFramework.Redis.Contracts.RedisObjects;
using Estim8.Backend.Persistence.Model;
using Microsoft.Extensions.Logging;

namespace Estim8.Backend.Persistence.Repositories
{
    public class RoundRepository : RedisRepository<Round>, IRoundRepository
    {
        public RoundRepository(IContext redisContext, ISerializer serializer, ILoggerFactory loggerFactory) : base(redisContext, serializer, loggerFactory)
        {
        }

        public async Task AddRound(Guid gameId, Round round)
        {
            await GetRounds(gameId).AddAsync(round);
        }

        public async Task<Round> GetById(Guid gameId, Guid roundId)
        {
            var rounds = await GetAllRounds(gameId);
            return rounds.SingleOrDefault(x => x.Id == roundId);
        }

        public async Task Replace(Guid gameId, Guid roundId, Round round)
        {
            //TODO: Make this all serverside chained ops
            var r = await GetById(gameId, roundId);

            var persistedIdx = await GetRounds(gameId).IndexOfAsync(r);
            await GetRounds(gameId).RemoveAtAsync(persistedIdx);
            await GetRounds(gameId).InsertAsync(persistedIdx, round);
        }

        public async Task<Round> GetCurrentRound(Guid gameId)
        {
            var q = await GetRounds(gameId).GetRangeAsync(-1);
            return q.LastOrDefault();
        }

        public async Task<IEnumerable<Round>> GetAllRounds(Guid gameId)
        {
            var rounds = GetRounds(gameId);
            return await rounds.GetRangeAsync();
        }

        public async Task Delete(Guid gameId, Guid roundId)
        {
            var rounds = await GetAllRounds(gameId);
            await GetRounds(gameId).RemoveAsync(rounds.Single(x => x.Id == roundId), 1);
        }

        private IRedisList<Round> GetRounds(Guid gameId)
        {
            return Redis.Collections.GetRedisList<Round>($"game:id:{gameId}:rounds");
        }
    }
}