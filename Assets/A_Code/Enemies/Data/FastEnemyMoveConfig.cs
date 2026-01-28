using A_Code.Systems.LoadConfig;
using A_Code.Systems.Move;

namespace A_Code.Enemies.Data
{
    [ConfigAddress("enemy_move_fast")]
    public class FastEnemyMoveConfig : IMoveConfig
    {
        public float MoveSpeed { get; set; }
        public float Acceleration { get; set; }
        public float Deceleration { get; set; }
    }
}