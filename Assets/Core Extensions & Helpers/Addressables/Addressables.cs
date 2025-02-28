using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityAddressables = UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

namespace Core
{
    public static class AddressablesTools
    {
        #region Keys
        public static string KeyAny => "Any";
        public static string KeyItem => "Items";
        public static string KeyNameGenerationSettings => "Name Generation Settings";
        #endregion
        #region Async
        private static async void LoadKeysToAction<T>(List<string> keys, Action<IList<T>> callback)
        {
            AsyncOperationHandle<IList<T>> loadRequest;
            loadRequest = UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<T>(keys, null, UnityEngine.AddressableAssets.Addressables.MergeMode.Union);

            await loadRequest.Task;
            if (loadRequest.Status == AsyncOperationStatus.Succeeded)
            {
                callback?.Invoke(loadRequest.Result);
            }
        }
        public static void LoadKeys<T>(string key, Action<IList<T>> callback) => LoadKeysToAction<T>(new List<string> { key }, callback);
        public static void LoadKeys<T>(string key1, string key2, Action<IList<T>> callback) => LoadKeysToAction<T>(new List<string> { key1, key2 }, callback);
        public static void LoadKeys<T>(string key1, string key2, string key3, Action<IList<T>> callback) => LoadKeysToAction<T>(new List<string> { key1, key2, key3 }, callback);
        public static void LoadKeys<T>(string key1, string key2, string key3, string key4, Action<IList<T>> callback) => LoadKeysToAction<T>(new List<string> { key1, key2, key3, key4 }, callback);
        #endregion
    }
}
