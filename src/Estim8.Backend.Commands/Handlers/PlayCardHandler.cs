using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Commands.Exceptions;
using Estim8.Backend.Persistence.Model;
using Estim8.Backend.Persistence.Repositories;

namespace Estim8.Backend.Commands.Handlers
{
    public class PlayCardHandler : ICommandHandler<PlayCard>, ICommandHandler<CancelCard>
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayCardRepository _playCardRepository;
        private readonly IRoundRepository _roundRepository;

        public PlayCardHandler(IGameRepository gameRepository, IPlayerRepository playerRepository, IPlayCardRepository playCardRepository, IRoundRepository roundRepository)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _playCardRepository = playCardRepository;
            _roundRepository = roundRepository;
        }
        
        public async Task<Response> Handle(PlayCard request, CancellationToken cancellationToken)
        {
            var playerInGame = await _playerRepository.GetPlayer(request.GameId, request.PlayerId);
            if(playerInGame == null)
                throw new DomainException(ErrorCode.NotAuthorized);

            var validValues = new []{0, 0.5, 1, 2, 3, 5, 8, 13, 20, 50, 100};
            var validTypes = new[] {"Infinite", "Break", "Number"};
            
            if(!validTypes.Contains(request.Type))
                throw new DomainException(ErrorCode.InputNotAllowed, $"Card type is not in allowed range. Must be in [{string.Join(", ", validTypes)}]");
            
            if(request.Value.HasValue && !validValues.Contains(request.Value.Value))
                throw new DomainException(ErrorCode.InputNotAllowed, $"Card value is not in allowed range. Must be in [{string.Join(", ", validValues)}]");
            
            var currentRound = await _roundRepository.GetCurrentRound(request.GameId);

            await _playCardRepository.PlayCard(request.GameId, request.PlayerId, currentRound.Id, new PlayedCard
            {
                GameId = request.GameId,
                PlayerId = request.PlayerId,
                GameRoundId = currentRound.Id,
                CardType = request.Type,
                CardValue = request.Value, 
                PlayedTimestamp = DateTimeOffset.Now
            });
            
            return Response.Success;
        }

        public async Task<Response> Handle(CancelCard request, CancellationToken cancellationToken)
        {
            var playerInGame = await _playerRepository.GetPlayer(request.GameId, request.PlayerId);
            if(playerInGame == null)
                throw new DomainException(ErrorCode.NotAuthorized);

            var currentRound = await _roundRepository.GetCurrentRound(request.GameId);
            
            await _playCardRepository.CancelCard(request.GameId, request.PlayerId, currentRound.Id);
            
            return Response.Success;
        }
    }
}