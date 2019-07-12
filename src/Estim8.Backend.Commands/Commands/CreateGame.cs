using System;
using MediatR;

namespace Estim8.Backend.Commands.Commands
{
    public class CreateGame : IRequest<Response>
    {
        public Guid Id { get; set; }
        public string Secret { get; set; }
        public string PublicId { get; set; }
        public Guid CardsetId { get; set; }
        
        
    }
}