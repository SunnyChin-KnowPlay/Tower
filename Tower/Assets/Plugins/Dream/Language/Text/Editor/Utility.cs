#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace Assets.Plugin.Language.Text.Editor
{
	public partial class Utility
	{
		private const string kTextOutputPath = "Text/Text.csv";

		[MenuItem("Utility/Text/Generate", false, 1)]
		static void GenerateText()
		{
			string srcPath = PlayerPrefs.GetString(Defined.kTextPkgPathKey);
			if (srcPath.Length < 1)
				return;

			string outputPath = PlayerPrefs.GetString(Defined.kOutputPathKey);
			if (outputPath.Length < 1)
				return;

			string outputFullPath = System.IO.Path.Combine(outputPath, kTextOutputPath);
			var directoryName = System.IO.Path.GetDirectoryName(outputFullPath);
			if (System.IO.Directory.Exists(directoryName) == false)
			{
				System.IO.Directory.CreateDirectory(directoryName);
			}
		
			try
			{
				var b = DreamEditor.Table.Parser.ParseExcelToText(srcPath, "text", outputFullPath);
			}
			catch (System.Exception ex)
			{
				UnityEngine.Debug.LogError(ex);
			}
			finally
			{
				AssetDatabase.Refresh();
			}
		}
	}
}
