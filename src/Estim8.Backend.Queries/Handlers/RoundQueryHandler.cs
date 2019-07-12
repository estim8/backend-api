using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Repositories;
using Estim8.Backend.Queries.Model;
using Estim8.Backend.Queries.Queries;

namespace Estim8.Backend.Queries.Handlers
{
    public class RoundQueryHandler : IQueryHandler<GetRoundById, Round>, IQueryHandler<GetCurrentRound, Round>
    {
        private readonly IRoundRepository _repo;

        public RoundQueryHandler(IRoundRepository repo)
        {
            _repo = repo;
        }
        
        public async Task<Round> Handle(GetRoundById request, CancellationToken cancellationToken)
        {
            var round = await _repo.GetById(request.GameId, request.RoundId);
            
            if (round == null)
                return null;

            return new Round
            {
                Id = round.Id,
                GameId = request.GameId,
                RoundVersion = round.RoundVersion,
                StartedTimestamp = round.StartedTimestamp,
                EndedTimestamp = round.EndedTimestamp,
                Subject = round.Subject
            };
        }

        public async Task<Round> Handle(GetCurrentRound request, CancellationToken cancellationToken)
        {
            var round = await _repo.GetCurrentRound(request.GameId);

            if (round == null)
                return null;
            
            return new Round
            {
                Id = round.Id,
                GameId = request.GameId,
                RoundVersion = round.RoundVersion,
                StartedTimestamp = round.StartedTimestamp,
                EndedTimestamp = round.EndedTimestamp,
                Subject = round.Subject
            };
        }
    }
}