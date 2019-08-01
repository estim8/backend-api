using System;
using Estim8.Backend.Commands.Services;
using MediatR;

namespace Estim8.Backend.Commands.Commands
{
    public class CreateGame : IRequest<Response<SerializedSecurityToken>>
    {
        public Guid Id { get; set; }
        public string Secret { get; set; }
        public string PublicId { get; set; }
        public Guid CardsetId { get; set; }
        public Guid PlayerId { get; set; }
    }
}