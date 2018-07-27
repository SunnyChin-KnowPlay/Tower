using System.IO;
using UnityEditor;

namespace AssetBundles
{
    public class AutoSetAssetBundleNamePostProcessor : AssetPostprocessor
    {
        const string kResDirectoryPath = "Assets/Res/";
        const string kGameResDirectoryPath = "Res/";

#if UNITY_EDITOR
        static int m_AutoProcessAssetBundleName = -1;
        const string kAutoProcessAssetBundleName = "AutoProcessAssetBundleName";
#endif

#if UNITY_EDITOR
        // Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
        public static bool AutoProcessAssetBundleName
        {
            get
            {
                if (m_AutoProcessAssetBundleName == -1)
                    m_AutoProcessAssetBundleName = EditorPrefs.GetBool(kAutoProcessAssetBundleName, true) ? 1 : 0;

                return m_AutoProcessAssetBundleName != 0;
            }
            set
            {
                int newValue = value ? 1 : 0;
                if (newValue != m_AutoProcessAssetBundleName)
                {
                    m_AutoProcessAssetBundleName = newValue;
                    EditorPrefs.SetBool(kAutoProcessAssetBundleName, value);
                }
            }
        }
#endif

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (!AutoProcessAssetBundleName)
                return;

            foreach (string assetPath in movedFromAssetPaths)
            {
                if (assetPath.Contains(kResDirectoryPath))
                {
                    foreach (string path in movedAssets)
                    {
                        if (Path.GetFileName(path) == Path.GetFileName(assetPath))
                        {
                            AutoClearAssetBundleName(path);
                            break;
                        }
                    }
                }
                else if (assetPath.Contains(kGameResDirectoryPath))
                {
                    foreach (string path in movedAssets)
                    {
                        if (Path.GetFileName(path) == Path.GetFileName(assetPath))
                        {
                            AutoClearAssetBundleName(path);
                            break;
                        }
                    }
                }
            }

            foreach (string assetPath in importedAssets)
            {
                if (assetPath.Contains(kResDirectoryPath))
                    AutoSetAssetBundleName(assetPath);
                else if (assetPath.Contains(kGameResDirectoryPath))
                {
                    AutoSetGameAssetBundleName(assetPath);
                }
                else
                {
                    AutoClearAssetBundleName(assetPath);
                }
            }

            foreach (string assetPath in movedAssets)
            {
                if (assetPath.Contains(kResDirectoryPath))
                    AutoSetAssetBundleName(assetPath);
                else if (assetPath.Contains(kGameResDirectoryPath))
                {
                    AutoSetGameAssetBundleName(assetPath);
                }
            }
        }

        static void AutoSetAssetBundleName(string assetPath)
        {
            if (File.Exists(assetPath) && Path.GetExtension(assetPath) != ".cs")
            {
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                if (importer)
                {
                    DirectoryInfo info = System.IO.Directory.GetParent(assetPath);
                    if (info == null || !info.Exists)
                        return;

                    string fullName = info.FullName.Replace('\\', '/');
                    int index = fullName.IndexOf(kResDirectoryPath);
                    if (index == -1)
                    {
                        importer.assetBundleName = "";
                        return;
                    }
                    if (index + kResDirectoryPath.Length >= fullName.Length)
                        return;

                    string assetBundleName = fullName.Substring(fullName.IndexOf(kResDirectoryPath) + kResDirectoryPath.Length);
                    importer.assetBundleName = assetBundleName + ".unity3d";
                }
            }
        }

        static void AutoSetGameAssetBundleName(string assetPath)
        {
            if (File.Exists(assetPath) && Path.GetExtension(assetPath) != ".cs")
            {
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                if (importer)
                {
                    DirectoryInfo info = System.IO.Directory.GetParent(assetPath);
                    if (info == null || !info.Exists)
                        return;

                    string fullName = info.FullName.Replace('\\', '/');
                    int index = fullName.IndexOf(kGameResDirectoryPath);
                    if (index == -1)
                    {
                        importer.assetBundleName = "";
                        return;
                    }
                    if (index + kGameResDirectoryPath.Length >= fullName.Length)
                        return;

                    string prefix = fullName.Substring(0, index - 1);
                    string gameName = prefix.Substring(prefix.LastIndexOf('/') + 1);

                    string assetBundleName = gameName + '/' + fullName.Substring(fullName.IndexOf(kGameResDirectoryPath));
                    importer.assetBundleName = assetBundleName + ".unity3d";
                }
            }
        }

        static void AutoClearAssetBundleName(string assetPath)
        {
            if (File.Exists(assetPath) && Path.GetExtension(assetPath) != ".cs")
            {
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                if (importer)
                {
                    DirectoryInfo info = System.IO.Directory.GetParent(assetPath);
                    if (info == null || !info.Exists)
                        return;

                    importer.assetBundleName = "";
                }
            }
        }
    }
}
