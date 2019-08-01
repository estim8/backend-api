using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Commands.Exceptions;
using Estim8.Backend.Commands.Services;
using Estim8.Backend.Persistence.Repositories;

namespace Estim8.Backend.Commands.Handlers
{
    public class PlayerHandler : ICommandHandler<AddPlayer, SerializedSecurityToken>, ICommandHandler<RemovePlayer>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameRepository _gameRepository;
        private readonly ISecurityTokenService _sts;

        public PlayerHandler(IPlayerRepository playerRepository, IGameRepository gameRepository, ISecurityTokenService sts)
        {
            _playerRepository = playerRepository;
            _gameRepository = gameRepository;
            _sts = sts;
        }
        
        public async Task<Response<SerializedSecurityToken>> Handle(AddPlayer request, CancellationToken cancellationToken)
        {
            var game = await _gameRepository.GetById(request.GameId);
            
            if(game.Secret != request.GameSecret)
                throw new DomainException(ErrorCode.NotAuthorized, "Game secret does not match");
            
            await _playerRepository.AddPlayer(request.GameId, request.PlayerId);
            
            var token = _sts.IssueToken(request.GameId, request.PlayerId, new []{PlayerRoles.Player.ToString()});

            return Response.FromResult(token);
        }

        public async Task<Response> Handle(RemovePlayer request, CancellationToken cancellationToken)
        {
            var game = await _gameRepository.GetById(request.GameId);
            
            if(game == null)
                throw new DomainException(ErrorCode.GameNotFound);

            await _playerRepository.DeletePlayer(request.GameId, request.PlayerId);
            
            return Response.Success;
        }
    }
}