using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estim8.Backend.Queries.Model;
using Estim8.Backend.Queries.Queries;
using MediatR;

namespace Estim8.Backend.Queries.Handlers
{
    public interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
    }
}