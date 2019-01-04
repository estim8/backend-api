using System;
using System.Collections.Generic;
using Estim8.Backend.Queries.Model;
using MediatR;

namespace Estim8.Backend.Queries
{
    public class GetAllGames : IRequest<IEnumerable<Game>>
    {
        public int Skip { get; set; }
        public int Limit { get; set; }
    }

    public class GetGameById : IRequest<Game>
    {
        public Guid Id { get; set; }

        public GetGameById(Guid id)
        {
            Id = id;
        }
    }
}