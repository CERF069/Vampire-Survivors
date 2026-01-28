namespace A_Code.Enemies.Spawner
{
    [System.Serializable]
    public class EnemySpawnSettings
    {
        public string enemyPrefab;
        public float chance;
        public int count;
        
        public string Id => enemyPrefab;
    }
}