using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace A_Code.Systems.LoadConfig
{
    public sealed class ConfigBootstrap : MonoBehaviour
    {
        [Inject] private ConfigStorage _storage;

        private async void Start()
        {
            Debug.Log("[ConfigBootstrap] Start loading configs");

            try
            {
                await LoadAll();
                Debug.Log("[ConfigBootstrap] Configs loaded → load Game");

                SceneManager.LoadScene("Game", LoadSceneMode.Single);
            }
            catch (Exception e)
            {
                Debug.LogError($"[ConfigBootstrap] FATAL\n{e}");
            }
        }

        private async UniTask LoadAll()
        {
            await UnityEngine.AddressableAssets.Addressables.InitializeAsync();

            var configs = ConfigReflectionCache.AllConfigs;
            Debug.Log($"[ConfigBootstrap] Found {configs.Count} configs");

            foreach (var (type, address) in configs)
            {
                Debug.Log($"[ConfigBootstrap] Loading {type.Name} ({address})");

                var config = await JsonConfigLoader.Load(type, address);

                if (config == null)
                    throw new Exception($"Failed to load {type.Name}");

                _storage.Set(type, config);
            }
        }
    }
}