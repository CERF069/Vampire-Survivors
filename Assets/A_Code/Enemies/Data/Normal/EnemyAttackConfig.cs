using A_Code.Systems.LoadConfig;

namespace A_Code.Enemies.Data.Normal
{
    [ConfigAddress("Enemy_attack")]
    public class EnemyAttackConfig
    {
        public int damage;
        public float cooldown ;
    }
}