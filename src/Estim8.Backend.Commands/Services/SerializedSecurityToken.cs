namespace Estim8.Backend.Commands.Services
{
    public class SerializedSecurityToken
    {
        public string Token { get; set; }
        public int Expires { get; set; }
        public string Type { get; set; }
    }
}