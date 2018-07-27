using UnityEngine;
using System.Collections;
using AssetBundles;
using System.Collections.Generic;

namespace Dream.Assets
{
    public class AssetLoader : MonoBehaviour
    {
        public delegate void AssetLoadedHandle<T>(T t) where T : UnityEngine.Object;
        public delegate void AssetsLoadedHandle<T>(T t, string name) where T : UnityEngine.Object;
        public delegate void AllAssetsLoadedHandle(UnityEngine.Object[] objs);

        public T LoadAssetSync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            return AssetBundleManager.LoadAssetSync<T>(assetBundleName, assetName);
        }

        public void LoadAsset<T>(string assetBundleName, string assetName, AssetLoadedHandle<T> handle) where T : UnityEngine.Object
        {
            StartCoroutine(LoadAssetWorkEnumerator(assetBundleName, assetName, handle));
        }

        public AssetBundleLoadOperation LoadAsset<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            return LoadAssetWorkEnumerator<T>(assetBundleName, assetName);
        }

        public UnityEngine.Object[] LoadAllAssetsSync(string assetBundleName)
        {
            return AssetBundleManager.LoadAllAssetSync(assetBundleName);
        }

        public void LoadAllAssets(string assetBundleName, AllAssetsLoadedHandle handle)
        {
            StartCoroutine(LoadAllAssetsWorkEnumerator(assetBundleName, handle));
        }

        public T[] LoadAssetWithSubAssetsSync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            return AssetBundleManager.LoadAssetWithSubAssetsSync<T>(assetBundleName, assetName);
        }

        public void LoadAssetWithSubAssets<T>(string assetBundleName, string assetName, AssetsLoadedHandle<T> handle) where T : UnityEngine.Object
        {
            StartCoroutine(LoadAssetWithSubAssetsWorkEnumerator(assetBundleName, assetName, handle));
        }

        public void LoadAssetWithSubAssets(string assetBundleName, string assetName, AssetsLoadedHandle<UnityEngine.Object> handle)
        {
            StartCoroutine(LoadAssetWithSubAssetsWorkEnumerator(assetBundleName, assetName, handle));
        }

        public IEnumerator LoadAssetBundleEnumerator(string assetBundleName)
        {
            yield return StartCoroutine(AssetBundleManager.LoadAssetBundleWithDependencies(assetBundleName));
        }

        public IEnumerator LoadSceneEnumerator(string assetBundleName, string sceneName, bool isAdditive)
        {
            return LoadSceneWorkEnumerator(assetBundleName, sceneName, isAdditive);
        }

        public IEnumerator LoadAssetWorkEnumerator<T>(string assetBundleName, string assetName, AssetLoadedHandle<T> handle) where T : UnityEngine.Object
        {
            // This is simply to get the elapsed time for this phase of AssetLoading.
            float startTime = Time.realtimeSinceStartup;

            // Load asset from assetBundle.
            AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(T));
            if (request == null)
                yield break;
            yield return StartCoroutine(request);

            T obj = request.GetAsset<T>();
            if (handle != null)
            {
                handle(obj);
            }

            // Calculate and display the elapsed time.
            float elapsedTime = Time.realtimeSinceStartup - startTime;
            //Debug.Log(assetName + (obj == null ? " was not" : " was") + " loaded successfully in " + elapsedTime + " seconds");
        }

        public AssetBundleLoadOperation LoadAssetWorkEnumerator<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            // This is simply to get the elapsed time for this phase of AssetLoading.
            float startTime = Time.realtimeSinceStartup;

            // Load asset from assetBundle.
            AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(T));
            if (request == null)
                return null;
            return request;
        }

        public AssetBundleLoadOperation LoadAllAssetsWorkEnumerator(string assetBundleName)
        {
            float startTime = Time.realtimeSinceStartup;

            AssetBundleLoadAssetsOperation request = AssetBundleManager.LoadAllAssetsAsync(assetBundleName);
            if (request == null)
                return null;
            return request;
        }

        public IEnumerator LoadAllAssetsWorkEnumerator(string assetBundleName, AllAssetsLoadedHandle handle)
        {
            float startTime = Time.realtimeSinceStartup;

            AssetBundleLoadAssetsOperation request = AssetBundleManager.LoadAllAssetsAsync(assetBundleName);
            if (request == null)
                yield break;
            yield return StartCoroutine(request);

            if (handle != null)
            {
                UnityEngine.Object[] objs = request.GetAssets();
                handle(objs);
            }

            float elapsedTime = Time.realtimeSinceStartup - startTime;
            //Debug.Log(assetBundleName + "was loaded successfully in " + elapsedTime + " seconds");
        }

        public IEnumerator LoadAssetWithSubAssetsWorkEnumerator<T>(string assetBundleName, string assetName, AssetsLoadedHandle<T> handle) where T : UnityEngine.Object
        {
            float startTime = Time.realtimeSinceStartup;

            AssetBundleLoadAssetsOperation request = AssetBundleManager.LoadAssetWithSubAssetsAsync(assetBundleName, assetName, typeof(T));
            if (request == null)
                yield break;
            yield return StartCoroutine(request);

            if (handle != null)
            {
                var objs = request.GetAssets();
                foreach (var obj in objs)
                {
                    if (obj.GetType() == typeof(T))
                        handle(obj as T, obj.name);
                }
            }

            float elapsedTime = Time.realtimeSinceStartup - startTime;
            //Debug.Log(assetBundleName + " was loaded successfully in " + elapsedTime + " seconds");
        }

        public AssetBundleLoadOperation LoadAssetWithSubAssetsWorkEnumerator<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            float startTime = Time.realtimeSinceStartup;

            AssetBundleLoadAssetsOperation request = AssetBundleManager.LoadAssetWithSubAssetsAsync(assetBundleName, assetName, typeof(T));
            if (request == null)
                return null;
            return request;
        }

        public IEnumerator LoadAssetWithSubAssetsWorkEnumerator(string assetBundleName, string assetName, AssetsLoadedHandle<UnityEngine.Object> handle)
        {
            float startTime = Time.realtimeSinceStartup;

            AssetBundleLoadAssetsOperation request = AssetBundleManager.LoadAssetWithSubAssetsAsync(assetBundleName, assetName, typeof(UnityEngine.Object));
            if (request == null)
                yield break;
            yield return StartCoroutine(request);

            if (handle != null)
            {
                var objs = request.GetAssets();
                foreach (var obj in objs)
                {
                    handle(obj, obj.name);
                }
            }

            float elapsedTime = Time.realtimeSinceStartup - startTime;
            //Debug.Log(assetBundleName + " was loaded successfully in " + elapsedTime + " seconds");
        }

        public AssetBundleLoadOperation LoadAssetWithSubAssetsWorkEnumerator(string assetBundleName, string assetName)
        {
            float startTime = Time.realtimeSinceStartup;

            AssetBundleLoadAssetsOperation request = AssetBundleManager.LoadAssetWithSubAssetsAsync(assetBundleName, assetName, typeof(UnityEngine.Object));
            if (request == null)
                return null;
            return request;
        }

        public AssetBundleLoadOperation LoadSceneWorkEnumerator(string assetBundleName, string sceneName, bool isAdditive)
        {
            AssetBundleLoadOperation request = AssetBundleManager.LoadSceneAsync(assetBundleName, sceneName, isAdditive);

            if (request == null)
                return null;

            return request;
        }

    }
}