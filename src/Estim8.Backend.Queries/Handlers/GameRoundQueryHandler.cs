using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Repositories;
using Estim8.Backend.Queries.Model;
using Estim8.Backend.Queries.Queries;

namespace Estim8.Backend.Queries.Handlers
{
    public class GameRoundQueryHandler : IQueryHandler<GetGameRoundById, GameRound>, IQueryHandler<GetCurrentGameRound, GameRound>
    {
        private readonly IGameRoundRepository _repo;

        public GameRoundQueryHandler(IGameRoundRepository repo)
        {
            _repo = repo;
        }
        
        public async Task<GameRound> Handle(GetGameRoundById request, CancellationToken cancellationToken)
        {
            var round = await _repo.GetById(request.GameId, request.RoundId);
            
            if (round == null)
                return null;

            return new GameRound
            {
                Id = round.Id,
                RoundVersion = round.RoundVersion,
                StartedTimestamp = round.StartedTimestamp,
                EndedTimestamp = round.EndedTimestamp,
                Subject = round.Subject
            };
        }

        public async Task<GameRound> Handle(GetCurrentGameRound request, CancellationToken cancellationToken)
        {
            var round = await _repo.GetCurrentRound(request.GameId);

            if (round == null)
                return null;
            
            return new GameRound
            {
                Id = round.Id,
                RoundVersion = round.RoundVersion,
                StartedTimestamp = round.StartedTimestamp,
                EndedTimestamp = round.EndedTimestamp,
                Subject = round.Subject
            };
        }
    }
}