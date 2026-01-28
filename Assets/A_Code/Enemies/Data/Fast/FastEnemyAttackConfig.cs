using A_Code.Systems.LoadConfig;

namespace A_Code.Enemies.Data.Fast
{
    [ConfigAddress("FastEnemy_attack")]
    public class FastEnemyAttackConfig
    {
        public int damage;
        public float cooldown ;
    }
}