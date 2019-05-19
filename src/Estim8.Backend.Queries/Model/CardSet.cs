using System.Collections;
using System.Collections.Generic;

namespace Estim8.Backend.Queries.Model
{
    public class CardSet
    {
        public string Name { get; set; }
        public IEnumerable<Card> Cards { get; set; }
    }
}