using System;
using System.Linq;
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
        private readonly IPlayerRepository _playerRepo;

        public GameQueryHandler(IRepository<Persistence.Model.Game> repo, IPlayerRepository playerRepo)
        {
            _repo = repo;
            _playerRepo = playerRepo;
        }

        public async Task<Game> Handle(GetGameById request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetById(request.Id);

            if (entity == null)
                return null;

            var players = (await _playerRepo.GetAllPlayersInGame(request.Id)).Select(x => new Player
            {
                Id = x.Id,
                Name = x.PlayerName,
                Email = x.Email
            });
            
            return new Game
            {
                Id = entity.Id,
                DealerId = entity.DealerId,
                CreatedTimestamp = entity.CreatedTimestamp,
                State = Enum.Parse<GameState>(entity.State.ToString()),
                Players = players
            };        
        }
    }
}