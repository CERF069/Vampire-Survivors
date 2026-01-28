using A_Code.Systems.LoadConfig;

namespace A_Code.Player.Data
{
    [ConfigAddress("player_attack")]
    public class PlayerAttackConfig
    { 
        public int damage;
        public float cooldown ;
    }
}