using System;
using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Commands.Exceptions;
using Estim8.Backend.Commands.Services;
using Estim8.Backend.Persistence.Model;
using Estim8.Backend.Persistence.Repositories;

namespace Estim8.Backend.Commands.Handlers
{
    public class GameHandler : ICommandHandler<CreateGame, SerializedSecurityToken>, ICommandHandler<StartGame>, ICommandHandler<EndGame>
    {
        private readonly IGameRepository _gameRepo;
        private readonly ISecurityTokenService _sts;

        public GameHandler(IGameRepository gameRepo, ISecurityTokenService sts)
        {
            _gameRepo = gameRepo;
            _sts = sts;
        }

        public async Task<Response<SerializedSecurityToken>> Handle(CreateGame request, CancellationToken cancellationToken)
        {
            await _gameRepo.Upsert(new Game
            {
                Id = request.Id, 
                CreatedTimestamp = DateTimeOffset.Now, 
                PublicId = request.PublicId,
                Secret = request.Secret,
                DealerId = request.PlayerId,
                State = GameState.AwaitingPlayers,
            });
            
            var token = _sts.IssueToken(request.Id, request.PlayerId, new []{PlayerRoles.Dealer.ToString()});

            return Response.FromResult(token);
        }

        public async Task<Response> Handle(StartGame request, CancellationToken cancellationToken)
        {
            var game = await _gameRepo.GetById(request.GameId);
            
            if(game == null)
                throw new DomainException(ErrorCode.GameNotFound, $"Game with id {request.GameId} was not found. Cannot start game.");
            
            await _gameRepo.SetGameState(request.GameId, GameState.Playing);
            
            return Response.Success;
        }

        public async Task<Response> Handle(EndGame request, CancellationToken cancellationToken)
        {
            var game = await _gameRepo.GetById(request.GameId);
            
            if(game == null)
                throw new DomainException(ErrorCode.GameNotFound,$"Game with id {request.GameId} was not found. Cannot start game.");
            
            var t_op = await _gameRepo.SetGameTimestamp(request.GameId, DateTimeOffset.Now);
            var s_op = await _gameRepo.SetGameState(request.GameId, GameState.Ended);
            
            return t_op && s_op ? Response.Success : Response.Failed(errorMessage:"Could not end game completely");
        }
    }
}