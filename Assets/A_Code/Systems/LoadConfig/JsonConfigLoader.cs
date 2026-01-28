using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace A_Code.Systems.LoadConfig
{
    public static class JsonConfigLoader
    {
        public static async UniTask<object> Load(Type type, string address)
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(address);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"[ConfigLoader] Failed to load: {address}");
                return null;
            }

            var settings = new JsonSerializerSettings
            {
                Converters = { new StringEnumConverter() },
                MissingMemberHandling = MissingMemberHandling.Error
            };

            return JsonConvert.DeserializeObject(handle.Result.text, type, settings);
        }
    }
}