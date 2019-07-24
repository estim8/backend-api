using System;
using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Repositories;
using Estim8.Backend.Queries.Model;

namespace Estim8.Backend.Queries.Handlers
{
    public class GameQueryHandler : 
        IQueryHandler<GetGameById, Game>
    {
        private readonly IRepository<Persistence.Model.Game> _repo;

        public GameQueryHandler(IRepository<Persistence.Model.Game> repo)
        {
            _repo = repo;
        }

        public async Task<Game> Handle(GetGameById request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetById(request.Id);

            if (entity == null)
                return null;
            
            return new Game
            {
                Id = entity.Id,
                DealerId = entity.DealerId,
                CreatedTimestamp = entity.CreatedTimestamp,
                State = Enum.Parse<GameState>(entity.State.ToString())
            };        
        }
    }
}