using System;

namespace Estim8.Backend.Api.Model
{
    public class CreateGameRequest
    {
        public string Secret { get; set; }
        public string PlayerName { get; set; }
        public string Gravatar { get; set; }
    }
}