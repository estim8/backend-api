using System;
using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Commands.Exceptions;
using Estim8.Backend.Persistence.Model;
using Estim8.Backend.Persistence.Repositories;

namespace Estim8.Backend.Commands.Handlers
{
    public class RoundHandler : ICommandHandler<AddGameRound>
    {
        private readonly IRoundRepository _roundRepo;
        private readonly IRepository<Game> _gameRepo;

        public RoundHandler(IRoundRepository roundRepo, IRepository<Game> gameRepo)
        {
            _roundRepo = roundRepo;
            _gameRepo = gameRepo;
        }
        
        public async Task<Response> Handle(AddGameRound request, CancellationToken cancellationToken)
        {
            var game = await _gameRepo.GetById(request.GameId);
            
            if(game == null)
                throw new DomainException(ErrorCode.GameNotFound, $"A game with id '{request.GameId}' was not found");

            var current = await _roundRepo.GetCurrentRound(request.GameId);
            await _roundRepo.UpdateRoundTimestamp(request.GameId, current.Id, DateTimeOffset.Now);
            
            await _roundRepo.AddRound(request.GameId, new Round
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