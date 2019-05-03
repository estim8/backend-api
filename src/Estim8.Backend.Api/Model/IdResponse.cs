using System;

namespace Estim8.Backend.Api.Model
{
    public class IdResponse
    {
        public Guid Id { get; set; }

        public IdResponse(Guid id)
        {
            Id = id;
        }
    }
}