using System;

namespace Estim8.Backend.Queries.Model
{
    public class Card
    {
        public Guid Guid { get; set; }
        public double Value { get; set; }
        public string Name { get; set; }
    }
}