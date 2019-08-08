namespace Estim8.Backend.Api.Model
{
    public class AddPlayerToGameRequest
    {
        public string Secret { get; set; }
        public string PlayerName { get; set; }
        public string Gravatar { get; set; }
    }
}