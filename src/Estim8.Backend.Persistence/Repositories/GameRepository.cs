using Cosmonaut;
using Estim8.Backend.Persistence.Model;
using Microsoft.Extensions.Options;
using Serilog;

namespace Estim8.Backend.Persistence.Repositories
{
    public class GameRepository : Repository<Game>
    {
        public GameRepository(ICosmosStore<Game> client, ILogger logger) : base(client, logger)
        {
        }
    }
}