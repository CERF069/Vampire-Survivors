using System.Collections;
using System.Linq;
using A_Code.Enemies.Data;
using UnityEngine;
using Zenject;

namespace A_Code.Enemies.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private EnemyPool _pool;
        private EnemySpawnConfig _config;

        [Inject]
        public void Construct(EnemySpawnConfig config, EnemyPool pool)
        {
            _config = config;
            _pool = pool;
        }

        private void Start()
        {
            _pool.Init(_config.Settings);
            StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                Vector3 spawnPos = GetSpawnPosition();
                SpawnRandomEnemy(spawnPos);
                yield return new WaitForSeconds(_config.SpawnInterval);
            }
        }

        private void SpawnRandomEnemy(Vector3 position)
        {
            var settings = _config.Settings;

            // нормализуем шансы
            float sum = settings.Sum(x => x.chance);
            float r = Random.value * sum;

            float acc = 0;
            EnemySpawnSettings chosen = null;

            foreach (var s in settings)
            {
                acc += s.chance;
                if (r <= acc)
                {
                    chosen = s;
                    break;
                }
            }

            if (chosen == null)
                return;

            // пытаемся спаунить выбранный тип
            var spawned = _pool.Spawn(chosen.Id, position);

            // если нет свободных объектов этого типа — пробуем другой тип
            if (spawned == null)
            {
                foreach (var s in settings)
                {
                    if (s == chosen) continue;
                    spawned = _pool.Spawn(s.Id, position);
                    if (spawned != null)
                        break;
                }
            }
        }

        private Vector3 GetSpawnPosition()
        {
            float camHeight = _camera.orthographicSize * 2f;
            float camWidth = camHeight * _camera.aspect;

            float left = _camera.transform.position.x - camWidth / 2f;
            float right = _camera.transform.position.x + camWidth / 2f;
            float top = _camera.transform.position.y + camHeight / 2f;
            float bottom = _camera.transform.position.y - camHeight / 2f;

            float spawnDistance = _config.SpawnDistance;

            int side = Random.Range(0, 4);

            float x = 0, y = 0;

            switch (side)
            {
                case 0:
                    x = left - spawnDistance;
                    y = Random.Range(bottom, top);
                    break;
                case 1:
                    x = right + spawnDistance;
                    y = Random.Range(bottom, top);
                    break;
                case 2:
                    x = Random.Range(left, right);
                    y = top + spawnDistance;
                    break;
                case 3:
                    x = Random.Range(left, right);
                    y = bottom - spawnDistance;
                    break;
            }

            return new Vector3(x, y, 0);
        }
    }
}
