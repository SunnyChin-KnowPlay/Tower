using UnityEditor;
using UnityEngine;

namespace Dream.Editor.Utilities
{
    public class ConfigWindow : EditorWindow
    {
        private static string s_ProtocolPath = "";
        private static string s_TablePath = "";
        private static string s_ProtocolCodePath = "";
        private static string s_TableCodePath = "";
        private static string s_TableResPath = "";

        [MenuItem("Utility/Config")]
        static void OnShowWindow()
        {
            if (PlayerPrefs.HasKey(Defined.kProtocolsPathKey))
            {
                s_ProtocolPath = PlayerPrefs.GetString(Defined.kProtocolsPathKey);
            }
            else
            {
                PlayerPrefs.SetString(Defined.kProtocolsPathKey, s_ProtocolPath);
            }
            if (PlayerPrefs.HasKey(Defined.kTablesPathKey))
            {
                s_TablePath = PlayerPrefs.GetString(Defined.kTablesPathKey);
            }
            else
            {
                PlayerPrefs.SetString(Defined.kTablesPathKey, s_TablePath);
            }
            if (PlayerPrefs.HasKey(Defined.kProtocolCodePathKey))
            {
                s_ProtocolCodePath = PlayerPrefs.GetString(Defined.kProtocolCodePathKey);
            }
            else
            {
                s_ProtocolCodePath = "Network/Scripts/Protocols";
                PlayerPrefs.SetString(Defined.kProtocolCodePathKey, s_ProtocolCodePath);
            }
            if (PlayerPrefs.HasKey(Defined.kTableCodePathKey))
            {
                s_TableCodePath = PlayerPrefs.GetString(Defined.kTableCodePathKey);
            }
            else
            {
                s_TableCodePath = "Table/Scripts";
                PlayerPrefs.SetString(Defined.kTableCodePathKey, s_TableCodePath);
            }
            if (PlayerPrefs.HasKey(Defined.kTableResPathKey))
            {
                s_TableResPath = PlayerPrefs.GetString(Defined.kTableResPathKey);
            }
            else
            {
                PlayerPrefs.SetString(Defined.kTableResPathKey, s_TableResPath);
            }
            EditorWindow.GetWindow(typeof(ConfigWindow));
        }

        private void ResetFields()
        {
            s_ProtocolPath = "";
            PlayerPrefs.SetString(Defined.kProtocolsPathKey, s_ProtocolPath);
            s_TablePath = "";
            PlayerPrefs.SetString(Defined.kTablesPathKey, s_TablePath);
            s_ProtocolCodePath = "Network/Scripts/Protocols";
            PlayerPrefs.SetString(Defined.kProtocolCodePathKey, s_ProtocolCodePath);
            s_TableCodePath = "Table/Scripts";
            PlayerPrefs.SetString(Defined.kTableCodePathKey, s_TableCodePath);
            s_TableResPath = "";
            PlayerPrefs.SetString(Defined.kTableResPathKey, s_TableResPath);
        }

        void OnGUI()
        {
            s_ProtocolPath = EditorGUI.TextField(new Rect(0, 0, 500, 30), "协议路径", s_ProtocolPath);
            s_TablePath = EditorGUI.TextField(new Rect(0, 30, 500, 30), "表格路径", s_TablePath);
            s_ProtocolCodePath = EditorGUI.TextField(new Rect(0, 60, 500, 30), "协议代码路径", s_ProtocolCodePath);
            s_TableCodePath = EditorGUI.TextField(new Rect(0, 90, 500, 30), "表格代码路径", s_TableCodePath);
            s_TableResPath = EditorGUI.TextField(new Rect(0, 120, 500, 30), "表格资源路径", s_TableResPath);

            if (GUI.Button(new Rect(0, 200, 100, 20), "保存"))
            {
                PlayerPrefs.SetString(Defined.kProtocolsPathKey, s_ProtocolPath);
                PlayerPrefs.SetString(Defined.kTablesPathKey, s_TablePath);
                PlayerPrefs.SetString(Defined.kProtocolCodePathKey, s_ProtocolCodePath);
                PlayerPrefs.SetString(Defined.kTableCodePathKey, s_TableCodePath);
                PlayerPrefs.SetString(Defined.kTableResPathKey, s_TableResPath);

                Close();
            }

            if (GUI.Button(new Rect(120, 200, 100, 20), "重置"))
            {
                ResetFields();
            }
        }
    }
}

