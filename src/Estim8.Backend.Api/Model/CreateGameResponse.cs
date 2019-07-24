using System;

namespace Estim8.Backend.Api.Model
{
    public class CreateGameResponse : IdResponse
    {
        public AccessToken Token { get; set; }
        public Guid PlayerId { get; set; }

        public CreateGameResponse(Guid id) : base(id)
        {
        }
    }
}