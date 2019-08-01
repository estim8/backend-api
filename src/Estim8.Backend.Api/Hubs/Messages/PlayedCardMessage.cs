using System;

namespace Estim8.Backend.Api.Hubs.Messages
{
    public class PlayedCardMessage
    {
        public Guid PlayerId { get; set; }
        public CardAction Action { get; set; }
        public string CardType { get; set; }
        public double? CardValue { get; set; }

        public enum CardAction
        {
            Play,
            Cancel
        }
    }
}