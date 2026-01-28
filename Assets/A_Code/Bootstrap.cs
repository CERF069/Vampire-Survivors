/*using A_Code.Systems.LoadConfig;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace A_Code
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [Inject] private ConfigStorage _configStorage;

        private async void Start()
        {
            await LoadAllConfigs();
            SceneManager.LoadScene("Game");
        }

        private async UniTask LoadAllConfigs()
        {
            foreach (var (type, address) in ConfigReflectionCache.AllConfigs)
            {
                var config = await JsonConfigLoader.Load(type, address);

                if (config != null)
                {
                    _configStorage.Set(type, config);
                }
            }
        }
    }
}*/