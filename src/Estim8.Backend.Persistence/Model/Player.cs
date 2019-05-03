namespace Estim8.Backend.Persistence.Model
{
    public class Player : Entity
    {
        public PlayerType Type { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

    public enum PlayerType
    {
        Device,
        User
    }
}