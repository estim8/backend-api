using System;

namespace Estim8.Backend.Persistence.Model
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}