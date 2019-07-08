using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;
using Estim8.Backend.Persistence.Repositories;
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
            return await _repo.GetById(request.GameId, request.RoundId);
        }

        public async Task<GameRound> Handle(GetCurrentGameRound request, CancellationToken cancellationToken)
        {
            return await _repo.GetCurrentRound(request.GameId);
        }
    }
}