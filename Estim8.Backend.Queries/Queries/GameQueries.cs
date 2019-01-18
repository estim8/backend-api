using System;
using System.Collections.Generic;
using Estim8.Backend.Queries.Model;
using MediatR;

namespace Estim8.Backend.Queries
{
    public class GetGamesByPageNo : IRequest<IEnumerable<Game>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
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