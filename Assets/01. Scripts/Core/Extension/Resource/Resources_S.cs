namespace Resources
{
    using System.Collections.Generic;
    using UnityEngine;
    public static class Resources_S
    {
        static Dictionary<string, Object> _resourceCache = new();
        public static T Load<T>(string path) where T : Object
        {
            if (!_resourceCache.ContainsKey(path))
                _resourceCache[path] = Resources.Load<T>(path);
            return (T)_resourceCache[path];
        }

        public static void UnloadAsset(Object @object)
        {
            Resources.UnloadAsset(@object);
        }

        public static void UnloadAssetsAll()
        {
            if (_resourceCache.Count > 0)
            {
                _resourceCache.Clear();
                Resources.UnloadUnusedAssets();
            }
        }
    }
}