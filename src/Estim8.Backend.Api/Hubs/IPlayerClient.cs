using System;
using System.Threading.Tasks;
using Estim8.Backend.Queries;

namespace Estim8.Backend.Api.Hubs
{
    public interface IPlayerClient
    {
        Task PlayerAddedToGame(Guid gameId, Guid playerId);
        Task PlayerRemovedFromGame(Guid gameId, Guid playerId);
    }
}