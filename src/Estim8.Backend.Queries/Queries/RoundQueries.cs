using System;
using Estim8.Backend.Queries.Model;
using MediatR;

namespace Estim8.Backend.Queries.Queries
{
    public class GetGameRoundById : IRequest<GameRound>
    {
        public GetGameRoundById(Guid gameId, Guid roundId, int roundVersion = 0)
        {
            GameId = gameId;
            RoundId = roundId;
            RoundVersion = roundVersion;
        }

        public Guid GameId { get; set; }
        public Guid RoundId { get; set; }
        public int RoundVersion { get; set; }
    }

    public class GetCurrentGameRound : IRequest<GameRound>
    {
        public GetCurrentGameRound(Guid gameId)
        {
            GameId = gameId;
        }

        public Guid GameId { get; set; }
    }
}