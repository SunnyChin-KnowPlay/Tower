using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

namespace AssetBundles
{
    // Loaded assetBundle contains the references count which can be used to unload dependent assetBundles automatically.
    public class LoadedAssetBundle
    {
        public AssetBundle m_AssetBundle;
        public int m_ReferencedCount;

        public LoadedAssetBundle(AssetBundle assetBundle)
        {
            m_AssetBundle = assetBundle;
            m_ReferencedCount = 1;
        }
    }

    // Class takes care of loading assetBundle and its dependencies automatically, loading variants automatically.
    public class AssetBundleManager : MonoBehaviour
    {
        public enum LogMode { All, JustErrors };
        public enum LogType { Info, Warning, Error };

        static LogMode m_LogMode = LogMode.All;
        static string m_BaseDownloadingURL = "";
        static string m_BaseDownloadingPath = "";
        static string[] m_ActiveVariants = { };
        static AssetBundleManifest m_AssetBundleManifest = null;

#if UNITY_EDITOR
        static int m_SimulateAssetBundleInEditor = -1;
        const string kSimulateAssetBundles = "SimulateAssetBundles";
#endif
        public static string kAssetBundleExt = ".unity3d";

        static Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
        static Dictionary<string, WWW> m_DownloadingWWWs = new Dictionary<string, WWW>();
        static Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string>();
        static List<AssetBundleLoadOperation> m_InProgressOperations = new List<AssetBundleLoadOperation>();
        static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

        public static LogMode logMode
        {
            get { return m_LogMode; }
            set { m_LogMode = value; }
        }

        public static string BaseDownloadingPath
        {
            get { return m_BaseDownloadingPath; }
            set { m_BaseDownloadingPath = value; }
        }

        // The base downloading url which is used to generate the full downloading url with the assetBundle names.
        public static string BaseDownloadingURL
        {
            get { return m_BaseDownloadingURL; }
            set { m_BaseDownloadingURL = value; }
        }

        // Variants which is used to define the active variants.
        public static string[] ActiveVariants
        {
            get { return m_ActiveVariants; }
            set { m_ActiveVariants = value; }
        }

        // AssetBundleManifest object which can be used to load the dependecies and check suitable assetBundle variants.
        public static AssetBundleManifest AssetBundleManifestObject
        {
            get { return m_AssetBundleManifest; }
            set { m_AssetBundleManifest = value; }
        }

        private static void Log(LogType logType, string text)
        {
            if (logType == LogType.Error)
                Debug.LogError("[AssetBundleManager] " + text);
            else if (m_LogMode == LogMode.All)
                Debug.Log("[AssetBundleManager] " + text);
        }

#if UNITY_EDITOR
        // Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
        public static bool SimulateAssetBundleInEditor
        {
            get
            {
                if (m_SimulateAssetBundleInEditor == -1)
                    m_SimulateAssetBundleInEditor = EditorPrefs.GetBool(kSimulateAssetBundles, true) ? 1 : 0;

                return m_SimulateAssetBundleInEditor != 0;
            }
            set
            {
                int newValue = value ? 1 : 0;
                if (newValue != m_SimulateAssetBundleInEditor)
                {
                    m_SimulateAssetBundleInEditor = newValue;
                    EditorPrefs.SetBool(kSimulateAssetBundles, value);
                }
            }
        }
#endif

        private static string GetStreamingAssetsPath()
        {
            if (Application.isEditor)
                return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/"); // Use the build output folder directly.
            else if (Application.isMobilePlatform || Application.isConsolePlatform)
                return Application.streamingAssetsPath;
            else // For standalone player.
                return "file://" + Application.streamingAssetsPath;
        }

        public static void SetSourceAssetBundleDirectory(string relativePath)
        {
            BaseDownloadingPath = GetStreamingAssetsPath() + "/" + relativePath + Utility.GetPlatformName() + "/";
        }

        public static void SetSourceAssetBundleURL(string absolutePath)
        {
            BaseDownloadingURL = absolutePath + "/" + Utility.GetPlatformName() + "/";
        }

        // Get loaded AssetBundle, only return vaild object when all the dependencies are downloaded successfully.
        static public LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
        {
            if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
                return null;

            LoadedAssetBundle bundle = null;
            m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle == null)
                return null;

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
                return bundle;

            // Make sure all dependencies are loaded
            foreach (var dependency in dependencies)
            {
                if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
                    return bundle;

                // Wait all the dependent assetBundles being loaded.
                LoadedAssetBundle dependentBundle;
                m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                if (dependentBundle == null)
                    return null;
            }

            return bundle;
        }

        static public AssetBundleLoadManifestOperation Initialize()
        {
            return Initialize(Utility.GetPlatformName());
        }

        // Load AssetBundleManifest.
        static public AssetBundleLoadManifestOperation Initialize(string manifestAssetBundleName)
        {
#if UNITY_EDITOR
            //Log(LogType.Info, "Simulation Mode: " + (SimulateAssetBundleInEditor ? "Enabled" : "Disabled"));
#endif

            GameObject go = GameObject.Find("AssetBundleManager");
            if (go == null)
            {
                go = new GameObject("AssetBundleManager", typeof(AssetBundleManager));
                DontDestroyOnLoad(go);
            }

#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't need the manifest assetBundle.
            if (SimulateAssetBundleInEditor)
                return null;
#endif

            LoadAssetBundle(manifestAssetBundleName, true);
            var operation = new AssetBundleLoadManifestOperation(manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
            m_InProgressOperations.Add(operation);
            return operation;
        }

        // Load AssetBundle and its dependencies.
        static protected void LoadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest = false)
        {
            //Log(LogType.Info, "Loading Asset Bundle " + (isLoadingAssetBundleManifest ? "Manifest: " : ": ") + assetBundleName);

#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't have to really load the assetBundle and its dependencies.
            if (SimulateAssetBundleInEditor)
                return;
#endif

            if (!isLoadingAssetBundleManifest)
            {
                if (m_AssetBundleManifest == null)
                {
                    Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                    return;
                }
            }

            // Check if the assetBundle has already been processed.
            bool isAlreadyProcessed = LoadAssetBundleInternal(assetBundleName, isLoadingAssetBundleManifest);

            // Load dependencies.
            if (!isAlreadyProcessed && !isLoadingAssetBundleManifest)
                LoadDependencies(assetBundleName);
        }

        static protected void LoadManifestInStreamingAssets(string assetBundleName, string key)
        {
#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't have to really load the assetBundle and its dependencies.
            if (SimulateAssetBundleInEditor)
                return;
#endif

            WWW download = null;
            string path = m_BaseDownloadingPath + assetBundleName;

            download = new WWW(path);

            m_DownloadingWWWs.Add(key, download);
        }

        // Remaps the asset bundle name to the best fitting asset bundle variant.
        static protected string RemapVariantName(string assetBundleName)
        {
            string[] bundlesWithVariant = m_AssetBundleManifest.GetAllAssetBundlesWithVariant();

            string[] split = assetBundleName.Split('.');

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;
            // Loop all the assetBundles with variant to find the best fit variant assetBundle.
            for (int i = 0; i < bundlesWithVariant.Length; i++)
            {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                if (curSplit[0] != split[0])
                    continue;

                int found = System.Array.IndexOf(m_ActiveVariants, curSplit[1]);

                // If there is no active variant found. We still want to use the first 
                if (found == -1)
                    found = int.MaxValue - 1;

                if (found < bestFit)
                {
                    bestFit = found;
                    bestFitIndex = i;
                }
            }

            if (bestFit == int.MaxValue - 1)
            {
                Debug.LogWarning("Ambigious asset bundle variant chosen because there was no matching active variant: " + bundlesWithVariant[bestFitIndex]);
            }

            if (bestFitIndex != -1)
            {
                return bundlesWithVariant[bestFitIndex];
            }
            else
            {
                return assetBundleName;
            }
        }

        static public bool CheckPackageIsCaching(string package)
        {
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor)
                return true;
#endif

            var assetBundles = m_AssetBundleManifest.GetAllAssetBundles();

            package = package.ToLower();
            bool isCaching = true;
            foreach (var assetBundleName in assetBundles)
            {
                if (assetBundleName.Contains(package))
                {
                    string url = m_BaseDownloadingURL + assetBundleName;

                    var hash = m_AssetBundleManifest.GetAssetBundleHash(assetBundleName);
                    if (!Caching.IsVersionCached(url, hash))
                    {
                        isCaching = false;
                        break;
                    }
                }
            }

            return isCaching;
        }

        static public IEnumerator LoadAssetBundleWithDependencies(string assetBundleName)
        {
            // Already loaded.
            LoadedAssetBundle assetBundle = null;
            m_LoadedAssetBundles.TryGetValue(assetBundleName, out assetBundle);
            if (assetBundle != null)
            {
                yield break;
            }

            WWW download = null;
            string url = m_BaseDownloadingURL + assetBundleName;
            string path = m_BaseDownloadingPath + assetBundleName;

            string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
            if (dependencies.Length > 0)
            {
                for (int i = 0; i < dependencies.Length; i++)
                {
                    dependencies[i] = RemapVariantName(dependencies[i]);

                    var dependenci = dependencies[i];
                    if (m_LoadedAssetBundles.ContainsKey(dependenci))
                        continue;
                    yield return LoadAssetBundleWithDependencies(dependenci);
                }
            }

            var hash = m_AssetBundleManifest.GetAssetBundleHash(assetBundleName);

            using (download = WWW.LoadFromCacheOrDownload(url, hash, 0))
            {
                yield return download;

                if (download.isDone)
                {
                    if (!string.IsNullOrEmpty(download.error))
                    {

                    }
                    else
                    {
                        AssetBundle bundle = download.assetBundle;
                        if (bundle == null)
                        {
                            yield break;
                        }

                        m_LoadedAssetBundles.Add(assetBundleName, new LoadedAssetBundle(bundle));
                    }
                }
            }
        }

        // Where we actuall call WWW to download the assetBundle.
        static protected bool LoadAssetBundleInternal(string assetBundleName, bool isLoadingAssetBundleManifest)
        {
            // Already loaded.
            LoadedAssetBundle bundle = null;
            m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle != null)
            {
                bundle.m_ReferencedCount++;
                return true;
            }

            if (m_DownloadingWWWs.ContainsKey(assetBundleName))
                return true;

            WWW download = null;
            string url = m_BaseDownloadingURL + assetBundleName;
            string path = m_BaseDownloadingPath + assetBundleName;

            // For manifest assetbundle, always download it as we don't have hash for it.
            if (isLoadingAssetBundleManifest)
            {
                download = new WWW(url);
            }
            else
            {
                var hash = m_AssetBundleManifest.GetAssetBundleHash(assetBundleName);
                if (Caching.IsVersionCached(path, hash))
                {
                    download = WWW.LoadFromCacheOrDownload(path, hash, 0);
                }
                else
                {
                    download = WWW.LoadFromCacheOrDownload(url, hash, 0);
                }
            }

            m_DownloadingWWWs.Add(assetBundleName, download);

            return false;
        }

        static protected bool LoadSceneInternal(string assetBundleName)
        {
            // Already loaded.
            LoadedAssetBundle bundle = null;
            m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle != null)
            {
                bundle.m_ReferencedCount++;
                return true;
            }

            if (m_DownloadingWWWs.ContainsKey(assetBundleName))
                return true;

            WWW download = null;
            string url = m_BaseDownloadingURL + assetBundleName;
            string path = m_BaseDownloadingPath + assetBundleName;

            download = WWW.LoadFromCacheOrDownload(url, 1);

            m_DownloadingWWWs.Add(assetBundleName, download);

            return false;
        }

        // Where we get all the dependencies and load them all.
        static protected void LoadDependencies(string assetBundleName)
        {
            if (m_AssetBundleManifest == null)
            {
                Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                return;
            }

            // Get dependecies from the AssetBundleManifest object..
            string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
            if (dependencies.Length == 0)
                return;

            for (int i = 0; i < dependencies.Length; i++)
                dependencies[i] = RemapVariantName(dependencies[i]);

            // Record and load all dependencies.
            m_Dependencies.Add(assetBundleName, dependencies);
            for (int i = 0; i < dependencies.Length; i++)
                LoadAssetBundleInternal(dependencies[i], false);
        }

        // Unload assetbundle and its dependencies.
        static public void UnloadAssetBundle(string assetBundleName)
        {
#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't have to load the manifest assetBundle.
            if (SimulateAssetBundleInEditor)
                return;
#endif

            //Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory before unloading " + assetBundleName);

            UnloadAssetBundleInternal(assetBundleName);
            UnloadDependencies(assetBundleName);

            //Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + assetBundleName);
        }

        static protected void UnloadDependencies(string assetBundleName)
        {
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
                return;

            // Loop dependencies.
            foreach (var dependency in dependencies)
            {
                UnloadAssetBundleInternal(dependency);
            }

            m_Dependencies.Remove(assetBundleName);
        }

        static protected void UnloadAssetBundleInternal(string assetBundleName)
        {
            string error;
            LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName, out error);
            if (bundle == null)
                return;

            if (--bundle.m_ReferencedCount == 0)
            {
                bundle.m_AssetBundle.Unload(false);
                m_LoadedAssetBundles.Remove(assetBundleName);

                Log(LogType.Info, assetBundleName + " has been unloaded successfully");
            }
        }

        void Update()
        {
            // Collect all the finished WWWs.
            var keysToRemove = new List<string>();
            foreach (var keyValue in m_DownloadingWWWs)
            {
                WWW download = keyValue.Value;

                // If downloading fails.
                if (!string.IsNullOrEmpty(download.error))
                {
                    m_DownloadingErrors.Add(keyValue.Key, string.Format("Failed downloading bundle {0} from {1}: {2}", keyValue.Key, download.url, download.error));
                    keysToRemove.Add(keyValue.Key);
                    continue;
                }

                // If downloading succeeds.
                if (download.isDone)
                {
                    AssetBundle bundle = download.assetBundle;
                    if (bundle == null)
                    {
                        m_DownloadingErrors.Add(keyValue.Key, string.Format("{0} is not a valid asset bundle.", keyValue.Key));
                        keysToRemove.Add(keyValue.Key);
                        continue;
                    }

                    //Debug.Log("Downloading " + keyValue.Key + " is done at frame " + Time.frameCount);
                    m_LoadedAssetBundles.Add(keyValue.Key, new LoadedAssetBundle(download.assetBundle));
                    keysToRemove.Add(keyValue.Key);
                }
            }

            // Remove the finished WWWs.
            foreach (var key in keysToRemove)
            {
                WWW download = m_DownloadingWWWs[key];
                m_DownloadingWWWs.Remove(key);
                download.Dispose();
            }

            // Update all in progress operations
            for (int i = 0; i < m_InProgressOperations.Count;)
            {
                if (!m_InProgressOperations[i].Update())
                {
                    m_InProgressOperations.RemoveAt(i);
                }
                else
                    i++;
            }
        }

        static public T LoadAssetSync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
                if (assetPaths.Length == 0)
                {
                    Debug.LogError("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
                    return null;
                }

                // @TODO: Now we only get the main object from the first asset. Should consider type also.
                Object target = AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
                return target as T;
            }
            else
#endif
            {
                assetBundleName = RemapVariantName(assetBundleName);
                if (m_LoadedAssetBundles.ContainsKey(assetBundleName) == false)
                {
                    return null;
                }

                var assetBundle = m_LoadedAssetBundles[assetBundleName];
                return assetBundle.m_AssetBundle.LoadAsset(assetName) as T;
            }
        }

        // Load asset from the given assetBundle.
        static public AssetBundleLoadAssetOperation LoadAssetAsync(string assetBundleName, string assetName, System.Type type)
        {
            //Log(LogType.Info, "Loading " + assetName + " from " + assetBundleName + " bundle");

            AssetBundleLoadAssetOperation operation = null;
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
                if (assetPaths.Length == 0)
                {
                    Debug.LogError("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
                    return null;
                }

                // @TODO: Now we only get the main object from the first asset. Should consider type also.
                Object target = AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
                operation = new AssetBundleLoadAssetOperationSimulation(target);
            }
            else
#endif
            {
                assetBundleName = RemapVariantName(assetBundleName);
                LoadAssetBundle(assetBundleName);
                operation = new AssetBundleLoadAssetOperationFull(assetBundleName, assetName, type);

                m_InProgressOperations.Add(operation);
            }

            return operation;
        }

        static public AssetBundleLoadAssetsOperation LoadAllAssetsAsync(string assetBundleName)
        {
            //Log(LogType.Info, "Loading " + assetBundleName + " bundle");

            AssetBundleLoadAssetsOperation operation = null;
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
                if (assetPaths.Length == 0)
                {
                    Debug.LogError("There is no asset in " + assetBundleName);
                    return null;
                }
                else
                {
                    List<Object> list = new List<Object>();
                    foreach (string path in assetPaths)
                    {
                        Object[] targets = AssetDatabase.LoadAllAssetsAtPath(path);
                        foreach (Object obj in targets)
                        {
                            list.Add(obj);
                        }
                    }
                    operation = new AssetBundleLoadAssetsOperationSimulation(list.ToArray());
                }
            }
            else
#endif
            {
                LoadAssetBundle(assetBundleName);
                operation = new AssetBundleLoadAllAssetsOperationFull(assetBundleName);

                m_InProgressOperations.Add(operation);
            }
            return operation;
        }

        static public UnityEngine.Object[] LoadAllAssetSync(string assetBundleName)
        {
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
                if (assetPaths.Length == 0)
                {
                    Debug.LogError("There is no asset in " + assetBundleName);
                    return null;
                }
                else
                {
                    List<Object> list = new List<Object>();
                    foreach (string path in assetPaths)
                    {
                        Object[] targets = AssetDatabase.LoadAllAssetsAtPath(path);
                        foreach (Object obj in targets)
                        {
                            list.Add(obj);
                        }
                    }
                    return list.ToArray();
                }
            }
            else
#endif
            {
                assetBundleName = RemapVariantName(assetBundleName);
                if (m_LoadedAssetBundles.ContainsKey(assetBundleName) == false)
                {
                    return null;
                }

                var assetBundle = m_LoadedAssetBundles[assetBundleName];
                return assetBundle.m_AssetBundle.LoadAllAssets();

            }
        }

        static public AssetBundleLoadAssetsOperation LoadAssetWithSubAssetsAsync(string assetBundleName, string assetName, System.Type type)
        {
            //Log(LogType.Info, "Loading " + assetName + " from " + assetBundleName + " bundle");

            AssetBundleLoadAssetsOperation operation = null;
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
                if (assetPaths.Length == 0)
                {
                    Debug.LogError("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
                    return null;
                }

                Object[] targets = AssetDatabase.LoadAllAssetsAtPath(assetPaths[0]);
                operation = new AssetBundleLoadAssetsOperationSimulation(targets);
            }
            else
#endif
            {
                LoadAssetBundle(assetBundleName);
                operation = new AssetBundleLoadSubAssetsOperationFull(assetBundleName, assetName, type);

                m_InProgressOperations.Add(operation);
            }
            return operation;
        }

        static public T[] LoadAssetWithSubAssetsSync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
                if (assetPaths.Length == 0)
                {
                    Debug.LogError("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
                    return null;
                }

                Object[] targets = AssetDatabase.LoadAllAssetsAtPath(assetPaths[0]);
                return targets as T[];
            }
            else
#endif
            {
                assetBundleName = RemapVariantName(assetBundleName);
                if (m_LoadedAssetBundles.ContainsKey(assetBundleName) == false)
                {
                    return null;
                }

                var assetBundle = m_LoadedAssetBundles[assetBundleName];
                return assetBundle.m_AssetBundle.LoadAssetWithSubAssets<T>(assetName);
            }
        }

        // Load level from the given assetBundle.
        static public AssetBundleLoadOperation LoadSceneAsync(string assetBundleName, string sceneName, bool isAdditive)
        {
            //Log(LogType.Info, "Loading " + sceneName + " from " + assetBundleName + " bundle");

            AssetBundleLoadOperation operation = null;
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor)
            {
                operation = new AssetBundleLoadSceneSimulationOperation(assetBundleName, sceneName, isAdditive);
            }
            else
#endif
            {
                assetBundleName = RemapVariantName(assetBundleName);
                LoadAssetBundleInternal(assetBundleName, false);
                operation = new AssetBundleLoadSceneOperation(assetBundleName, sceneName, isAdditive);

                m_InProgressOperations.Add(operation);
            }

            return operation;
        }
    } // End of AssetBundleManager.
}