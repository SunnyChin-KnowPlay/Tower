using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace AssetBundles
{
    public abstract class AssetBundleLoadOperation : IEnumerator
    {
        public object Current
        {
            get
            {
                return null;
            }
        }
        public bool MoveNext()
        {
            return !IsDone();
        }

        public void Reset()
        {
        }

        abstract public bool Update();

        abstract public bool IsDone();
    }

#if UNITY_EDITOR
    public class AssetBundleLoadSceneSimulationOperation : AssetBundleLoadOperation
    {
        AsyncOperation m_Operation = null;


        public AssetBundleLoadSceneSimulationOperation(string assetBundleName, string levelName, bool isAdditive)
        {
            string[] levelPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, levelName);
            if (levelPaths.Length == 0)
            {
                ///@TODO: The error needs to differentiate that an asset bundle name doesn't exist
                //        from that there right scene does not exist in the asset bundle...

                Debug.LogError("There is no scene with name \"" + levelName + "\" in " + assetBundleName);
                return;
            }

            if (isAdditive)
                m_Operation = UnityEditor.EditorApplication.LoadLevelAdditiveAsyncInPlayMode(levelPaths[0]);
            else
                m_Operation = UnityEditor.EditorApplication.LoadLevelAsyncInPlayMode(levelPaths[0]);
        }

        public override bool Update()
        {
            return false;
        }

        public override bool IsDone()
        {
            return m_Operation == null || m_Operation.isDone;
        }
    }

#endif
    public class AssetBundleLoadSceneOperation : AssetBundleLoadOperation
    {
        protected string m_AssetBundleName;
        protected string m_SceneName;
        protected string m_DownloadingError;
        protected bool m_IsAdditive;
        protected AsyncOperation m_Request;

        public AssetBundleLoadSceneOperation(string assetbundleName, string sceneName, bool isAdditive)
        {
            m_AssetBundleName = assetbundleName;
            m_SceneName = sceneName;
            m_IsAdditive = isAdditive;
        }

        public override bool Update()
        {
            if (m_Request != null)
                return false;

            LoadedAssetBundle bundle = AssetBundleManager.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
            if (bundle != null)
            {
                
                if (m_IsAdditive)
                {
                    m_Request = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_SceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                }
                else
                {
                    m_Request = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_SceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
                }

                return false;
            }
            else
                return true;
        }

        public override bool IsDone()
        {
            // Return if meeting downloading error.
            // m_DownloadingError might come from the dependency downloading.
            if (m_Request == null && m_DownloadingError != null)
            {
                Debug.LogError(m_DownloadingError);
                return true;
            }

            return m_Request != null && m_Request.isDone;
        }
    }

    public abstract class AssetBundleLoadAssetOperation : AssetBundleLoadOperation
    {
        public abstract T GetAsset<T>() where T : UnityEngine.Object;
    }

    public abstract class AssetBundleLoadAssetsOperation : AssetBundleLoadOperation
    {
        public abstract UnityEngine.Object[] GetAssets();
    }

    public class AssetBundleLoadAssetOperationSimulation : AssetBundleLoadAssetOperation
    {
        UnityEngine.Object m_SimulatedObject;

        public AssetBundleLoadAssetOperationSimulation(UnityEngine.Object simulatedObject)
        {
            m_SimulatedObject = simulatedObject;
        }

        public override T GetAsset<T>()
        {
            return m_SimulatedObject as T;
        }

        public override bool Update()
        {
            return false;
        }

        public override bool IsDone()
        {
            return true;
        }
    }

    public class AssetBundleLoadAssetsOperationSimulation : AssetBundleLoadAssetsOperation
    {
        UnityEngine.Object[] m_SimulatedObjects;

        public AssetBundleLoadAssetsOperationSimulation(UnityEngine.Object[] simulatedObjects)
        {
            m_SimulatedObjects = simulatedObjects;
        }

        public override UnityEngine.Object[] GetAssets()
        {
            return m_SimulatedObjects;
        }

        public override bool IsDone()
        {
            return true;
        }

        public override bool Update()
        {
            return false;
        }
    }

    public class AssetBundleLoadAssetOperationFull : AssetBundleLoadAssetOperation
    {
        protected string m_AssetBundleName;
        protected string m_AssetName;
        protected string m_DownloadingError;
        protected System.Type m_Type;
        protected AssetBundleRequest m_Request = null;

        public AssetBundleLoadAssetOperationFull(string bundleName, string assetName, System.Type type)
        {
            m_AssetBundleName = bundleName;
            m_AssetName = assetName;
            m_Type = type;
        }

        public override T GetAsset<T>()
        {
            if (m_Request != null && m_Request.isDone)
                return m_Request.asset as T;
            else
                return null;
        }

        // Returns true if more Update calls are required.
        public override bool Update()
        {
            if (m_Request != null)
                return false;

            LoadedAssetBundle bundle = AssetBundleManager.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
            if (bundle != null)
            {
                ///@TODO: When asset bundle download fails this throws an exception...
                m_Request = bundle.m_AssetBundle.LoadAssetAsync(m_AssetName, m_Type);
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool IsDone()
        {
            // Return if meeting downloading error.
            // m_DownloadingError might come from the dependency downloading.
            if (m_Request == null && m_DownloadingError != null)
            {
                Debug.LogError(m_DownloadingError);
                return true;
            }

            return m_Request != null && m_Request.isDone;
        }
    }

    public class AssetBundleLoadAllAssetsOperationFull : AssetBundleLoadAssetsOperation
    {
        protected string m_AssetBundleName;
        protected string m_DownloadingError;
        protected AssetBundleRequest m_Request = null;

        public AssetBundleLoadAllAssetsOperationFull(string assetBundleName)
        {
            this.m_AssetBundleName = assetBundleName;
        }

        public override UnityEngine.Object[] GetAssets()
        {
            if (m_Request != null && m_Request.isDone)
                return m_Request.allAssets;
            return null;
        }

        public override bool IsDone()
        {
            // Return if meeting downloading error.
            // m_DownloadingError might come from the dependency downloading.
            if (m_Request == null && m_DownloadingError != null)
            {
                Debug.LogError(m_DownloadingError);
                return true;
            }

            return m_Request != null && m_Request.isDone;
        }

        public override bool Update()
        {
            if (m_Request != null)
                return false;

            LoadedAssetBundle bundle = AssetBundleManager.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
            if (bundle != null)
            {
                ///@TODO: When asset bundle download fails this throws an exception...
                m_Request = bundle.m_AssetBundle.LoadAllAssetsAsync();
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class AssetBundleLoadSubAssetsOperationFull : AssetBundleLoadAllAssetsOperationFull
    {
        protected string m_AssetName;
        protected System.Type m_Type;

        public AssetBundleLoadSubAssetsOperationFull(string assetBundleName, string assetName, System.Type type)
            : base(assetBundleName)
        {
            m_AssetName = assetName;
            m_Type = type;
        }

        public override bool Update()
        {
            if (m_Request != null)
                return false;

            LoadedAssetBundle bundle = AssetBundleManager.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
            if (bundle != null)
            {
                m_Request = bundle.m_AssetBundle.LoadAssetWithSubAssetsAsync(m_AssetName, m_Type);
                return false;
            }
            else
            {
                return true;
            }
        }
    }


    public class AssetBundleLoadManifestOperation : AssetBundleLoadAssetOperationFull
    {
        public AssetBundleLoadManifestOperation(string bundleName, string assetName, System.Type type)
            : base(bundleName, assetName, type)
        {
        }

        public override bool Update()
        {
            base.Update();

            if (m_Request != null && m_Request.isDone)
            {
                AssetBundleManager.AssetBundleManifestObject = GetAsset<AssetBundleManifest>();
                return false;
            }
            else
                return true;
        }
    }


}
