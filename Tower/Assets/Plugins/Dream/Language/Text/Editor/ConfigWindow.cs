#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace Assets.Plugin.Language.Text.Editor
{
	class ConfigWindow : EditorWindow
	{
		private static string s_TextPkgPath = "";
		private static string s_OutputPath = "";

		[MenuItem("Utility/Text/Config", false, 10)]
		static void OnShowWindow()
		{
			if (PlayerPrefs.HasKey(Defined.kTextPkgPathKey))
			{
				s_TextPkgPath = PlayerPrefs.GetString(Defined.kTextPkgPathKey);
			}
			else
			{
				PlayerPrefs.SetString(Defined.kTextPkgPathKey, s_TextPkgPath);
			}

			if (PlayerPrefs.HasKey(Defined.kOutputPathKey))
			{
				s_OutputPath = PlayerPrefs.GetString(Defined.kOutputPathKey);
			}
			else
			{
				PlayerPrefs.SetString(Defined.kOutputPathKey, s_OutputPath);
			}
			EditorWindow.GetWindow(typeof(ConfigWindow));
		}

		void OnGUI()
		{
			s_TextPkgPath = EditorGUI.TextField(new Rect(0, 0, 500, 30), "excel目录", s_TextPkgPath);
			s_OutputPath = EditorGUI.TextField(new Rect(0, 30, 500, 30), "输出cvs路径", s_OutputPath);

			if (GUI.Button(new Rect(0, 200, 100, 20), "保存"))
			{
				PlayerPrefs.SetString(Defined.kTextPkgPathKey, s_TextPkgPath);
				PlayerPrefs.SetString(Defined.kOutputPathKey, s_OutputPath);

				Close();
			}
		}
	}
}
