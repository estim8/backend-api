using System.Collections.Generic;

namespace Estim8.Backend.Persistence.Model
{
    public class ResultPage<TEntity> where TEntity : Entity
    {
        public string NextPageToken { get; private set; }
        public bool HasNextPage { get; private set; }
        public List<TEntity> Results { get; private set; }

        public ResultPage(List<TEntity> results, bool hasNextPage = false, string nextPageToken = null)
        {
            Results = results;
            HasNextPage = hasNextPage;
            NextPageToken = nextPageToken;
        }
    }
}