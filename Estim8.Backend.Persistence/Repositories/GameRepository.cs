using Estim8.Backend.Persistence.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;

namespace Estim8.Backend.Persistence.Repositories
{
    public class GameRepository : Repository<Game>
    {
        public GameRepository(IMongoClient client, IOptions<PersistenceConfiguration> config, ILogger logger) : base(client, config, logger)
        {
        }
    }
}