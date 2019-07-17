using System;
using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Persistence.Model;
using Estim8.Backend.Persistence.Repositories;

namespace Estim8.Backend.Commands.Handlers
{
    public class GameHandler : ICommandHandler<CreateGame>
    {
        private readonly IRepository<Game> _gameRepo;

        public GameHandler(IRepository<Game> gameRepo)
        {
            _gameRepo = gameRepo;
        }

        public async Task<Response> Handle(CreateGame request, CancellationToken cancellationToken)
        {
            await _gameRepo.Upsert(new Game
            {
                Id = request.Id, 
                CreatedTimestamp = DateTimeOffset.Now, 
                PublicId = request.PublicId,
                CardSetId = request.CardsetId, 
                Secret = request.Secret,
                State = GameState.AwaitingPlayers,
            });

            return Response.Success;
        }
    }
}