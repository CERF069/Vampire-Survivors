using System.Collections.Generic;
using A_Code.Enemies.Interface;
using A_Code.Enemies.Spawner;
using UnityEngine;
using Zenject;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemyPool
{
    private readonly DiContainer _container;

    // пул по типам
    private readonly Dictionary<string, List<IEnemyPooled>> _pools =
        new Dictionary<string, List<IEnemyPooled>>();

    public EnemyPool(DiContainer container)
    {
        _container = container;
    }

    public async void Init(List<EnemySpawnSettings> settings)
    {
        foreach (var s in settings)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(s.enemyPrefab);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed load prefab: " + s.enemyPrefab);
                continue;
            }

            GameObject prefab = handle.Result;

            if (!_pools.ContainsKey(s.Id))
                _pools[s.Id] = new List<IEnemyPooled>();

            for (int i = 0; i < s.count; i++)
            {
                var go = _container.InstantiatePrefab(prefab);
                go.SetActive(false);
                _pools[s.Id].Add(go.GetComponent<IEnemyPooled>());
            }
        }
    }

    public IEnemyPooled Spawn(string id, Vector3 position)
    {
        if (!_pools.ContainsKey(id))
            return null;

        var pool = _pools[id];
        var enemy = pool.Find(x => !((MonoBehaviour)x).gameObject.activeSelf);

        if (enemy == null)
            return null;

        enemy.OnSpawn(position);
        return enemy;
    }
}