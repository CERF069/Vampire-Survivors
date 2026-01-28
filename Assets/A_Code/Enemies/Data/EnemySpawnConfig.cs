using System.Collections.Generic;
using A_Code.Enemies.Spawner;
using A_Code.Systems.LoadConfig;

namespace A_Code.Enemies.Data
{
    [ConfigAddress("enemy_spawn_config")]
    public class EnemySpawnConfig
    {
        public float SpawnInterval { get; set; }
        public float SpawnDistance { get; set; }
        public bool SpawnSides { get; set; }
        public float SpawnRadius { get; set; }

        public List<EnemySpawnSettings> Settings { get; set; }
    }
}