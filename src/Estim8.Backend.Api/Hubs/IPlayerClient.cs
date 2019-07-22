using System;
using System.Threading.Tasks;
using Estim8.Backend.Api.Hubs.Messages;
using Estim8.Backend.Queries;

namespace Estim8.Backend.Api.Hubs
{
    public interface IPlayerClient
    {
        Task PlayerAddedToGame(PlayerMessage player);
        Task PlayerRemovedFromGame(PlayerMessage player);
    }
}