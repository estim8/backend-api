using System;
using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Persistence.Model;
using Estim8.Backend.Persistence.Repositories;

namespace Estim8.Backend.Commands.Handlers
{
    public class GameRoundHandler : ICommandHandler<AddGameRound>
    {
        private readonly IGameRoundRepository _roundRepo;
        private readonly IRepository<Game> _gameRepo;

        public GameRoundHandler(IGameRoundRepository roundRepo, IRepository<Game> gameRepo)
        {
            _roundRepo = roundRepo;
            _gameRepo = gameRepo;
        }
        
        public async Task<Response> Handle(AddGameRound request, CancellationToken cancellationToken)
        {
            var game = await _gameRepo.GetById(request.GameId);
            
            if(game == null)
                throw new NullReferenceException($"A game with id '{request.GameId}' was not found");

            var current = await _roundRepo.GetCurrentRound(request.GameId);
            current.EndedTimestamp = DateTimeOffset.Now;
            await _roundRepo.Replace(request.GameId, current.Id, current);
            
            await _roundRepo.AddRound(request.GameId, new GameRound
            {
                Id = request.Id,
                StartedTimestamp = DateTimeOffset.Now,
                Subject = request.Subject,
                RoundVersion = request.RoundVersion
            });
            
            return Response.Success;
        }
    }
}