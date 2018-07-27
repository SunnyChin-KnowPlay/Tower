using AssetBundles;
using System.Collections;
using UnityEngine;

namespace Dream.Assets
{
    public class AssetUpdater : MonoBehaviour
    {
        public delegate void UpdateProgressHandle(int current, int max, string curretBundleName);
        public event UpdateProgressHandle UpdateProgress;

        public delegate void UpdateStateChangedHandle();
        public event UpdateStateChangedHandle UpdateBegan;
        public event UpdateStateChangedHandle UpdateEnded;

        private AssetManager assetManager;

        private void Awake()
        {
            assetManager = AssetManager.Instance;
        }

        void Start()
        {

        }

        public IEnumerator Preload(string remoteDir, string localDir)
        {
            if (!AssetManager.s_IsInitialize)
            {
                yield return assetManager.Initialize(remoteDir, localDir);
            }

            if (UpdateBegan != null)
            {
                UpdateBegan.Invoke();
            }

#if UNITY_EDITOR
            if (!AssetBundleManager.SimulateAssetBundleInEditor)
#endif
            {
                // 这里是把所有的资源包都download一下，看看有没有最新的
                AssetBundleManifest manifest = AssetBundleManager.AssetBundleManifestObject;
                if (manifest != null)
                {
                    var allAssetBundles = manifest.GetAllAssetBundles();

                    int length;
                    length = allAssetBundles.Length;
                    int i = 0;

                    for (i = 0; i < length; i++)
                    {
                        var assetBundle = allAssetBundles[i];

                        yield return PreloadAssetBundle(assetBundle);
                        if (UpdateProgress != null)
                        {
                            UpdateProgress.Invoke(i, length, assetBundle);
                        }
                    }
                }
            }

            if (UpdateEnded != null)
            {
                UpdateEnded.Invoke();
            }
        }

        public IEnumerator PreloadAssetBundle(string assetBundleName)
        {
            yield return AssetBundleManager.LoadAssetBundleWithDependencies(assetBundleName);
        }
    }
}