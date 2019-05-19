using System;

namespace Estim8.Backend.Api.Model
{
    public class CreateGameRequest
    {
        public Guid CardSetId { get; set; }
        public string Secret { get; set; }
    }
}