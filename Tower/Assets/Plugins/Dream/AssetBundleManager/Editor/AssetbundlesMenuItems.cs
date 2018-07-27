using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using Assets.Plugins.Dream.AssetBundleManager.Editor;

namespace AssetBundles
{

    public class AssetBundlesMenuItems : EditorWindow
    {
        const string kSimulationMode = "Assets/AssetBundles/Simulation Mode";
        const string kAutoMode = "Assets/AssetBundles/AutoProcess Mode";

        private static string s_FTPLocationPath = "";

        [MenuItem(kSimulationMode)]
        public static void ToggleSimulationMode()
        {
#if UNITY_EDITOR
            AssetBundleManager.SimulateAssetBundleInEditor = !AssetBundleManager.SimulateAssetBundleInEditor;
#endif
        }

        [MenuItem(kSimulationMode, true)]
        public static bool ToggleSimulationModeValidate()
        {
#if UNITY_EDITOR
            Menu.SetChecked(kSimulationMode, AssetBundleManager.SimulateAssetBundleInEditor);
#endif
            return true;
        }

        [MenuItem(kAutoMode)]
        public static void ToggleAutoModeMode()
        {
#if UNITY_EDITOR
            AutoSetAssetBundleNamePostProcessor.AutoProcessAssetBundleName = !AutoSetAssetBundleNamePostProcessor.AutoProcessAssetBundleName;
#endif
        }

        [MenuItem(kAutoMode, true)]
        public static bool ToggleAutoModeValidate()
        {
#if UNITY_EDITOR
            Menu.SetChecked(kAutoMode, AutoSetAssetBundleNamePostProcessor.AutoProcessAssetBundleName);
#endif
            return true;
        }

        [MenuItem("Assets/AssetBundles/Build AssetBundles")]
        static public void BuildAssetBundles()
        {
            BuildScript.BuildAssetBundles();
        }

        [MenuItem("Assets/AssetBundles/Config")]
        static void OnShowWindow()
        {
            if (PlayerPrefs.HasKey(Defined.kPublishFTPPathKey))
            {
                s_FTPLocationPath = PlayerPrefs.GetString(Defined.kPublishFTPPathKey);
            }
            else
            {
                PlayerPrefs.SetString(Defined.kPublishFTPPathKey, s_FTPLocationPath);
            }

            EditorWindow.GetWindow(typeof(AssetBundlesMenuItems));
        }

        void OnGUI()
        {
            s_FTPLocationPath = EditorGUI.TextField(new Rect(0, 40, 500, 30), "FTP路径(可选)", s_FTPLocationPath);

            if (GUI.Button(new Rect(0, 200, 100, 20), "保存"))
            {
                PlayerPrefs.SetString(Defined.kPublishFTPPathKey, s_FTPLocationPath);

                Close();
            }
        }
    }

}