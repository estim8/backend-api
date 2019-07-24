using System;

namespace Estim8.Backend.Api.Model
{
    public class AddPlayerToGameResponse
    {
        public Guid PlayerId { get; set; }
        public AccessToken Token { get; set; }
    }
}