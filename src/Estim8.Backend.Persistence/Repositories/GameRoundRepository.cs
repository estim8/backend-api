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
    public class GameRoundRepository : RedisRepository<GameRound>, IGameRoundRepository
    {
        public GameRoundRepository(IContext redisContext, ILoggerFactory loggerFactory) : base(redisContext, loggerFactory)
        {
        }

        public async  Task AddRound(Guid gameId, GameRound round)
        {
            await GetRounds(gameId).AddAsync(round);
        }

        public async Task<GameRound> GetById(Guid gameId, Guid roundId)
        {
            var rounds = await GetAllRounds(gameId);
            return rounds.SingleOrDefault(x => x.Id == roundId);
        }

        public async Task Replace(Guid gameId, Guid roundId, GameRound round)
        {
            //TODO: Make this all serverside chained ops
            var r = await GetById(gameId, roundId);

            var persistedIdx = await GetRounds(gameId).IndexOfAsync(r);
            await GetRounds(gameId).RemoveAtAsync(persistedIdx);
            await GetRounds(gameId).InsertAsync(persistedIdx, round);
        }

        public async Task<GameRound> GetCurrentRound(Guid gameId)
        {
            var q = await GetRounds(gameId).GetRangeAsync(-2);
            return q.LastOrDefault();
        }

        public async Task<IEnumerable<GameRound>> GetAllRounds(Guid gameId)
        {
            return await GetRounds(gameId).GetRangeAsync();
        }

        public async Task Delete(Guid gameId, Guid roundId)
        {
            var rounds = await GetAllRounds(gameId);
            await GetRounds(gameId).RemoveAsync(rounds.Single(x => x.Id == roundId), 1);
        }

        private IRedisList<GameRound> GetRounds(Guid gameId)
        {
            return Redis.Collections.GetRedisList<GameRound>($"game:id:{gameId}:rounds");
        }
    }
}