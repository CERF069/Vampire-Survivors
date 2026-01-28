using A_Code.Systems.LoadConfig;
using A_Code.Systems.Move;

namespace A_Code.Player.Data
{
    [ConfigAddress("player_move")]
    public sealed class PlayerMoveConfig : IMoveConfig
    {
        public float MoveSpeed { get; set; }
        public float Acceleration { get; set; }
        public float Deceleration { get; set; }
    }

}