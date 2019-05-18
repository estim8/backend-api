using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Repositories;
using Estim8.Backend.Queries.Model;

namespace Estim8.Backend.Queries.Handlers
{
    public class GameQueryHandler : 
        IQueryHandler<GetGamesByPageNo, IEnumerable<Game>>,
        IQueryHandler<GetGameById, Game>
    {
        private readonly IRepository<Persistence.Model.Game> _repo;

        public GameQueryHandler(IRepository<Persistence.Model.Game> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Game>> GetPaged(int page, int pageSize)
        {
            return (await _repo.GetPaged(page, pageSize)).Select(x => new Game
            {
                Id = x.Id
            });
        }

        public async Task<IEnumerable<Game>> Handle(GetGamesByPageNo request, CancellationToken cancellationToken)
        {
            return (await _repo.GetPaged(request.PageNumber, request.PageSize)).Select(x => new Game
            {
                Id = x.Id
            });
        }

        public async Task<Game> Handle(GetGameById request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetById(request.Id);

            if (entity == null)
                return null;
            
            return new Game
            {
                Id = entity.Id,
                PublicId = entity.PublicId,
                CreatedTimestamp = entity.CreatedTimestamp
            };        
        }
    }
}