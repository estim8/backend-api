using Estim8.Backend.Commands.Commands;
using MediatR;

namespace Estim8.Backend.Commands.Handlers
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Response>
        where TCommand : IRequest<Response>
    {
        
    }
    
    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Response<TResponse>>
        where TCommand : IRequest<Response<TResponse>>
    {
        
    }
}