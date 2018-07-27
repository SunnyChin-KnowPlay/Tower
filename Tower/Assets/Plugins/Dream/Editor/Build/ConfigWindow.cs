#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace Assets.Plugins.Dream.Editor.Build
{
    public class ConfigWindow : EditorWindow
    {
        private static string s_PublishLocationPath = "";
        private static string s_FTPLocationPath = "";
        private static string s_ProductNamePath = "";

        [MenuItem("Publish/Config")]
        static void OnShowWindow()
        {
            if (PlayerPrefs.HasKey(Defined.kPublishPathKey))
            {
                s_PublishLocationPath = PlayerPrefs.GetString(Defined.kPublishPathKey);
            }
            else
            {
                PlayerPrefs.SetString(Defined.kPublishPathKey, s_PublishLocationPath);
            }

            if (PlayerPrefs.HasKey(Defined.kPublishFTPPathKey))
            {
                s_FTPLocationPath = PlayerPrefs.GetString(Defined.kPublishFTPPathKey);
            }
            else
            {
                PlayerPrefs.SetString(Defined.kPublishFTPPathKey, s_FTPLocationPath);
            }

            if (PlayerPrefs.HasKey(Defined.kPublishProductKey))
            {
                s_ProductNamePath = PlayerPrefs.GetString(Defined.kPublishProductKey);
            }
            else
            {
                PlayerPrefs.SetString(Defined.kPublishProductKey, s_ProductNamePath);
            }

            EditorWindow.GetWindow(typeof(ConfigWindow));
        }

        void OnGUI()
        {
            s_PublishLocationPath = EditorGUI.TextField(new Rect(0, 0, 500, 30), "发布路径", s_PublishLocationPath);
            s_FTPLocationPath = EditorGUI.TextField(new Rect(0, 40, 500, 30), "FTP路径(可选)", s_FTPLocationPath);
            s_ProductNamePath = EditorGUI.TextField(new Rect(0, 80, 500, 30), "游戏名字", s_ProductNamePath);

            if (GUI.Button(new Rect(0, 200, 100, 20), "保存"))
            {
                PlayerPrefs.SetString(Defined.kPublishPathKey, s_PublishLocationPath);
                PlayerPrefs.SetString(Defined.kPublishFTPPathKey, s_FTPLocationPath);
                PlayerPrefs.SetString(Defined.kPublishProductKey, s_ProductNamePath);

                Close();
            }
        }
    }
}
