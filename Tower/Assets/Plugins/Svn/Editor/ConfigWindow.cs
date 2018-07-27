using UnityEditor;
using UnityEngine;

namespace Assets.Plugins.Svn.Editor
{
	public class ConfigWindow : EditorWindow
	{
		public const string kUsernameKey = "Dream.Configs.Svn.Username";
		public const string kPasswordKey = "Dream.Configs.Svn.Password";

		private static string s_Username = "";
		private static string s_Password = "";


        [MenuItem("SVN/Config")]
        static void OnShowWindow()
		{
			if (PlayerPrefs.HasKey(kUsernameKey))
			{
				s_Username = PlayerPrefs.GetString(kUsernameKey);
			}
			else
			{
				PlayerPrefs.SetString(kUsernameKey, s_Username);
			}
			if (PlayerPrefs.HasKey(kPasswordKey))
			{
				s_Password = PlayerPrefs.GetString(kPasswordKey);
			}
			else
			{
				PlayerPrefs.SetString(kPasswordKey, s_Password);
			}

			EditorWindow.GetWindow(typeof(ConfigWindow));
		}

		void OnGUI()
		{
			s_Username = EditorGUI.TextField(new Rect(0, 0, 500, 30), "用户名", s_Username);
			s_Password = EditorGUI.TextField(new Rect(0, 30, 500, 30), "密码", s_Password);
			

			if (GUI.Button(new Rect(0, 200, 100, 20), "保存"))
			{
				PlayerPrefs.SetString(kUsernameKey, s_Username);
				PlayerPrefs.SetString(kPasswordKey, s_Password);
				

				Close();
			}
		}
	}
}
