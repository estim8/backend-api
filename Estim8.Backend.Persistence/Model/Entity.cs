using System;
using Cosmonaut.Attributes;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace Estim8.Backend.Persistence.Model
{
    public class Entity
    {
        [CosmosPartitionKey]
        [JsonProperty("id")]
        public Guid Id { get; protected set; }
    }
}