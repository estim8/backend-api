using System;
using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Commands.Handlers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Estim8.Backend.Commands.Decorators
{
    public class ExceptionResponseDecorator<TCommand> : Decorator<ICommandHandler<TCommand>>, ICommandHandler<TCommand> where TCommand : IRequest<Response>
    {
        public ExceptionResponseDecorator(ICommandHandler<TCommand> decorated, ILogger<ICommandHandler<TCommand>> logger) : base(decorated, logger)
        {
        }

        public async Task<Response> Handle(TCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await Decorated.Handle(request, cancellationToken);
            }
            catch (Exception e)
            {
                Log.LogError(e, "Error executing {@Command} at {CommandHandler}", request, Decorated.GetType().FullName);
                return Response.Failed(e, $"Error executing {request.GetType().Name} at {GetType().Name}");
            }
        }
    }
}