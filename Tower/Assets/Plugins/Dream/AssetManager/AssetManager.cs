using UnityEngine;
using System.Collections;
using AssetBundles;
using Dream.Utilities;

namespace Dream.Assets
{
    public sealed class AssetManager : Singleton<AssetManager>
    {
        private AssetLoader m_AssetLoader;
        private AssetUpdater m_Updater;
       
        public AssetUpdater Updater
        {
            get { return m_Updater; }
        }

        internal static bool s_IsInitialize = false;

        void Awake()
        {
            if (gameObject.GetComponent<AssetLoader>() == null)
            {
                m_AssetLoader = gameObject.AddComponent<AssetLoader>();
                m_Updater = gameObject.AddComponent<AssetUpdater>();
            }
            else
            {
                m_AssetLoader = gameObject.GetComponent<AssetLoader>();
                m_Updater = gameObject.GetComponent<AssetUpdater>();
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // 
        public IEnumerator Initialize(string remoteDir, string localDir)
        {
            if (!s_IsInitialize)
            {
                SetupAssetBundlePatch(localDir, remoteDir);

                // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
                var request = AssetBundleManager.Initialize();
                if (request != null)
                {
                    yield return StartCoroutine(request);

                    
                }
                s_IsInitialize = true;
            }
        }

        private void SetupAssetBundlePatch(string localDir, string remoteDir)
        {
            AssetBundleManager.SetSourceAssetBundleDirectory(localDir);
            AssetBundleManager.SetSourceAssetBundleURL(remoteDir);
        }

        /// <summary>
        /// 从Resources文件夹内读取单个资源
        /// </summary>
        /// <param name="path">资源在Resources/下的路径</param>
        /// <returns></returns>
        public static Object LoadResource(string path)
        {
            return Resources.Load(path);
        }

        /// <summary>
        /// 从Resources文件夹内读取单个资源
        /// </summary>
        /// <typeparam name="T">资源的类型</typeparam>
        /// <param name="path">资源在Resources/下的路径</param>
        /// <returns></returns>
        public static T LoadResource<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// 从Resources文件夹内异步读取单个资源
        /// </summary>
        /// <param name="path">资源在Resources/下的路径</param>
        /// <param name="handle">回调句柄</param>
        public static void LoadResourceAsync(string path, AssetLoader.AssetLoadedHandle<Object> handle)
        {
            s_Instance.StartCoroutine(s_Instance.LoadResourceAsyncWork(path, handle));
        }

        /// <summary>
        /// 从Resources文件夹内异步读取单个资源
        /// </summary>
        /// <typeparam name="T">资源的类型</typeparam>
        /// <param name="path">资源在Resources/下的路径</param>
        /// <param name="handle">回调句柄</param>
        public static void LoadResourceAsync<T>(string path, AssetLoader.AssetLoadedHandle<T> handle) where T : Object
        {
            s_Instance.StartCoroutine(s_Instance.LoadResourceAsyncWork<T>(path, handle));
        }

        /// <summary>
        /// 从Resources文件夹内读取一个文件夹中的所有资源
        /// </summary>
        /// <param name="path">需要读取所有资源的路径</param>
        /// <returns></returns>
        public static Object[] LoadAllResources(string path)
        {
            return Resources.LoadAll(path);
        }

        /// <summary>
        /// 从Resources文件夹内读取一个文件夹中的所有资源
        /// </summary>
        /// <typeparam name="T">资源的类型</typeparam>
        /// <param name="path">需要读取所有资源的路径</param>
        /// <returns></returns>
        public static T[] LoadAllResources<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path);
        }

        /// <summary>
        /// 释放不使用的资源
        /// </summary>
        public static void UnloadUnusedResources()
        {
            s_Instance.StartCoroutine(s_Instance.UnloadUnusedResourcesWork());
        }

        private IEnumerator UnloadUnusedResourcesWork()
        {
            AsyncOperation op = Resources.UnloadUnusedAssets();
            while (!op.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        public IEnumerator LoadResourceAsyncWork<T>(string path, AssetLoader.AssetLoadedHandle<T> handle) where T : Object
        {
            var request = Resources.LoadAsync<T>(path);
            if (request == null)
                yield break;

            while (!request.isDone)
            {
                yield return null;
            }

            if (handle != null)
            {
                handle(request.asset as T);
            }
            yield return request;
        }

        public static T LoadAssetSync<T>(string assetFullName) where T : UnityEngine.Object
        {
            if (assetFullName.Length == 0)
                return null;

            assetFullName = assetFullName.Replace('\\', '/');
            string assetBundleName = assetFullName.Substring(0, assetFullName.LastIndexOf('/'));
            string assetName = assetFullName.Substring(assetFullName.LastIndexOf('/') + 1);
            assetBundleName += AssetBundleManager.kAssetBundleExt;

            return LoadAssetSync<T>(assetBundleName, assetName);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产
        /// </summary>
        /// <param name="assetFullName">资产全路径 .. assetBundleName/assetName </param>
        /// <param name="handle">回调句柄</param>
        public static void LoadAssetAsync(string assetFullName, AssetLoader.AssetLoadedHandle<UnityEngine.Object> handle)
        {
            if (assetFullName.Length == 0)
                return;

            assetFullName = assetFullName.Replace('\\', '/');
            string assetBundleName = assetFullName.Substring(0, assetFullName.LastIndexOf('/'));
            string assetName = assetFullName.Substring(assetFullName.LastIndexOf('/') + 1);
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            LoadAssetAsync(assetBundleName, assetName, handle);
        }

        public static AssetBundleLoadOperation LoadAssetAsyncEnumerator(string assetFullName)
        {
            if (assetFullName.Length == 0)
                return null;

            assetFullName = assetFullName.Replace('\\', '/');
            string assetBundleName = assetFullName.Substring(0, assetFullName.LastIndexOf('/'));
            string assetName = assetFullName.Substring(assetFullName.LastIndexOf('/') + 1);
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            return LoadAssetAsyncEnumerator(assetBundleName, assetName);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产
        /// </summary>
        /// <param name="assetBundleName">包名 assetBundleName</param>
        /// <param name="assetName">资产名</param>
        /// <param name="handle">回调句柄</param>
        public static void LoadAssetAsync(string assetBundleName, string assetName, AssetLoader.AssetLoadedHandle<UnityEngine.Object> handle)
        {
            assetBundleName = assetBundleName.ToLower();
            assetName = assetName.ToLower();
            s_Instance.m_AssetLoader.LoadAsset(assetBundleName, assetName, handle);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产
        /// </summary>
        /// <param name="assetBundleName">包名 assetBundleName</param>
        /// <param name="assetName">资产名</param>
        public static AssetBundleLoadOperation LoadAssetAsync(string assetBundleName, string assetName)
        {
            assetBundleName = assetBundleName.ToLower();
            assetName = assetName.ToLower();
            return s_Instance.m_AssetLoader.LoadAsset<UnityEngine.Object>(assetBundleName, assetName);
        }

        public static T LoadAssetSync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            assetBundleName = assetBundleName.ToLower();
            assetName = assetName.ToLower();

            return s_Instance.m_AssetLoader.LoadAssetSync<T>(assetBundleName, assetName);
        }

        /// <summary>
        /// 加载资源，返回迭代。
        /// </summary>
        /// <param name="assetBundleName">包名 assetBundleName</param>
        /// <param name="assetName">资产名</param>
        /// <returns></returns>
        public static AssetBundleLoadOperation LoadAssetAsyncEnumerator(string assetBundleName, string assetName)
        {
            assetBundleName = assetBundleName.ToLower();
            assetName = assetName.ToLower();
            return s_Instance.m_AssetLoader.LoadAssetWorkEnumerator<UnityEngine.Object>(assetBundleName, assetName);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产
        /// </summary>
        /// <typeparam name="T">资产类型</typeparam>
        /// <param name="assetFullName">资产全路径 .. assetBundleName/assetName </param>
        /// <param name="handle">回调句柄</param>
        public static void LoadAssetAsync<T>(string assetFullName, AssetLoader.AssetLoadedHandle<T> handle) where T : UnityEngine.Object
        {
            if (assetFullName.Length == 0)
                return;

            assetFullName = assetFullName.Replace('\\', '/');
            string assetBundleName = assetFullName.Substring(0, assetFullName.LastIndexOf('/'));
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            string assetName = assetFullName.Substring(assetFullName.LastIndexOf('/') + 1);
            LoadAssetAsync(assetBundleName, assetName, handle);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产
        /// </summary>
        /// <typeparam name="T">资产类型</typeparam>
        /// <param name="assetFullName">资产全路径 .. assetBundleName/assetName </param>
        public static AssetBundleLoadOperation LoadAssetAsyncEnumerator<T>(string assetFullName) where T : UnityEngine.Object
        {
            if (assetFullName.Length == 0)
                return null;

            assetFullName = assetFullName.Replace('\\', '/');
            string assetBundleName = assetFullName.Substring(0, assetFullName.LastIndexOf('/'));
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            string assetName = assetFullName.Substring(assetFullName.LastIndexOf('/') + 1);
            return LoadAssetAsyncEnumerator<T>(assetBundleName, assetName);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产
        /// </summary>
        /// <typeparam name="T">资产类型</typeparam>
        /// <param name="assetBundleName">包名 assetBundleName</param>
        /// <param name="assetName">资产名</param>
        /// <param name="handle">回调句柄</param>
        public static void LoadAssetAsync<T>(string assetBundleName, string assetName, AssetLoader.AssetLoadedHandle<T> handle) where T : UnityEngine.Object
        {
            assetBundleName = assetBundleName.ToLower();
            assetName = assetName.ToLower();
            s_Instance.m_AssetLoader.LoadAsset(assetBundleName, assetName, handle);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产
        /// </summary>
        /// <typeparam name="T">资产类型</typeparam>
        /// <param name="assetBundleName">包名 assetBundleName</param>
        /// <param name="assetName">资产名</param>
        public static AssetBundleLoadOperation LoadAssetAsyncEnumerator<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            assetBundleName = assetBundleName.ToLower();
            assetName = assetName.ToLower();
            return s_Instance.m_AssetLoader.LoadAssetWorkEnumerator<T>(assetBundleName, assetName);
        }

        public static UnityEngine.Object[] LoadAllAssetSync(string assetBundleName)
        {
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            assetBundleName = assetBundleName.ToLower();

            return s_Instance.m_AssetLoader.LoadAllAssetsSync(assetBundleName);
        }

        /// <summary>
        /// 从AssetBundle里异步读取所有资产
        /// </summary>
        /// <param name="assetBundleName">包名</param>
        /// <param name="handle">回调句柄 该回调是将所有的资产一个一个通过方法调用给观察者</param>
        public static AssetBundleLoadOperation LoadAllAssetAsyncEnumerator(string assetBundleName)
        {
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            assetBundleName = assetBundleName.ToLower();
            return s_Instance.m_AssetLoader.LoadAllAssetsWorkEnumerator(assetBundleName);
        }

        /// <summary>
        /// 从AssetBundle里异步读取所有资产
        /// </summary>
        /// <param name="assetBundleName">包名</param>
        /// <param name="handle">回调句柄 该回调是将所有资源通过数组的方式调用给观察者</param>
        public static void LoadAllAssetAsync(string assetBundleName, AssetLoader.AllAssetsLoadedHandle handle)
        {
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            assetBundleName = assetBundleName.ToLower();
            s_Instance.m_AssetLoader.LoadAllAssets(assetBundleName, handle);
        }

        public static T[] LoadAssetWithSubAssetsSync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            assetBundleName = assetBundleName.ToLower();
            assetName = assetName.ToLower();

            return s_Instance.m_AssetLoader.LoadAssetWithSubAssetsSync<T>(assetBundleName, assetName);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产及子资产
        /// </summary>
        /// <typeparam name="T">需要读取的资产的类型</typeparam>
        /// <param name="assetBundleName">包名</param>
        /// <param name="assetName">资产名</param>
        /// <param name="handle">回调句柄</param>
        public static void LoadAssetWithSubAssetsAsync<T>(string assetBundleName, string assetName, AssetLoader.AssetsLoadedHandle<T> handle) where T : UnityEngine.Object
        {
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            assetBundleName = assetBundleName.ToLower();
            assetName = assetName.ToLower();
            s_Instance.m_AssetLoader.LoadAssetWithSubAssets(assetBundleName, assetName, handle);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产及子资产
        /// </summary>
        /// <typeparam name="T">需要读取的资产的类型</typeparam>
        /// <param name="assetBundleName">包名</param>
        /// <param name="assetName">资产名</param>
        public static AssetBundleLoadOperation LoadAssetWithSubAssetsAsyncEnumerator<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            assetBundleName = assetBundleName.ToLower();
            assetName = assetName.ToLower();
            return s_Instance.m_AssetLoader.LoadAssetWithSubAssetsWorkEnumerator<T>(assetBundleName, assetName);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产及子资产
        /// </summary>
        /// <param name="assetBundleName">包名</param>
        /// <param name="assetName">资产名</param>
        /// <param name="handle">回调句柄</param>
        public static void LoadAssetWithSubAssetsAsync(string assetBundleName, string assetName, AssetLoader.AssetsLoadedHandle<UnityEngine.Object> handle)
        {
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            assetBundleName = assetBundleName.ToLower();
            assetName = assetName.ToLower();
            s_Instance.m_AssetLoader.LoadAssetWithSubAssets(assetBundleName, assetName, handle);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产及子资产
        /// </summary>
        /// <typeparam name="T">需要读取的资产的类型</typeparam>
        /// <param name="assetFullName">全路径</param>
        /// <param name="handle">回调句柄</param>
        public static void LoadAssetWithSubAssetsAsync<T>(string assetFullName, AssetLoader.AssetsLoadedHandle<T> handle) where T : UnityEngine.Object
        {
            if (assetFullName.Length == 0)
                return;

            assetFullName = assetFullName.Replace('\\', '/');
            string assetBundleName = assetFullName.Substring(0, assetFullName.LastIndexOf('/'));
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            string assetName = assetFullName.Substring(assetFullName.LastIndexOf('/') + 1);

            LoadAssetWithSubAssetsAsync(assetBundleName, assetName, handle);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产及子资产
        /// </summary>
        /// <typeparam name="T">需要读取的资产的类型</typeparam>
        /// <param name="assetFullName">全路径</param>
        public static AssetBundleLoadOperation LoadAssetWithSubAssetsAsyncEnumerator<T>(string assetFullName) where T : UnityEngine.Object
        {
            if (assetFullName.Length == 0)
                return null;

            assetFullName = assetFullName.Replace('\\', '/');
            string assetBundleName = assetFullName.Substring(0, assetFullName.LastIndexOf('/'));
            assetBundleName += AssetBundleManager.kAssetBundleExt;
            string assetName = assetFullName.Substring(assetFullName.LastIndexOf('/') + 1);

            return LoadAssetWithSubAssetsAsyncEnumerator<T>(assetBundleName, assetName);
        }

        /// <summary>
        /// 从AssetBundle里异步读取资产及子资产
        /// </summary>
        /// <param name="assetFullName">全路径</param>
        /// <param name="handle">回调句柄</param>
        public static void LoadAssetWithSubAssetsAsync(string assetFullName, AssetLoader.AssetsLoadedHandle<UnityEngine.Object> handle)
        {
            LoadAssetWithSubAssetsAsync(assetFullName, handle);
        }

        public static void LoadSceneAsync(string assetBundleName, string sceneName, bool isAdditive)
        {

            s_Instance.StartCoroutine(LoadSceneAsyncEnumerator(assetBundleName, sceneName, isAdditive));
        }

        public static IEnumerator LoadSceneAsyncEnumerator(string assetBundleName, string sceneName, bool isAdditive)
        {
            if (sceneName.ToLower() == "sea")
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            }
            else
            {
                assetBundleName += ("/scenes" + AssetBundleManager.kAssetBundleExt);
                assetBundleName = assetBundleName.ToLower();
                yield return s_Instance.m_AssetLoader.LoadSceneEnumerator(assetBundleName, sceneName, isAdditive);
            }
        }
    }
}