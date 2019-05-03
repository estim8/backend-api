using System;
using MediatR;

namespace Estim8.Backend.Commands.Commands
{
    public class CreateGame : IRequest<Response>
    {
        public Guid Id { get; set; }
        
    }
}