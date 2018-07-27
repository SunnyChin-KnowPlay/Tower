using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Assets.Plugins.Dream.Editor.Build;

namespace AssetBundles
{
    public class BuildScript
    {
        public static string overloadedDevelopmentServerURL = "";

        public static void BuildAssetBundles()
        {
            string assetBundlesOutputPath = Utility.AssetBundlesOutputPath;

            ProcessConfig();

            System.IO.DirectoryInfo projectDir = System.IO.Directory.GetParent(Application.dataPath);

            // Choose the output path according to the build target.
            string outputPath = Path.Combine(assetBundlesOutputPath, Utility.GetPlatformName());
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            DeleteFilesAndFolders(outputPath);
            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);

            var fullPath = projectDir.FullName;
            fullPath = Path.Combine(fullPath, AssetBundles.Utility.AssetBundlesOutputPath);

            var configPath = getConfigTextPath();
            var configText = File.ReadAllText(configPath);
            AssetBundleConfig assetBundleConfig = new AssetBundleConfig(configText);

            var now = System.DateTime.Now;
            string timeStr = string.Format("{0:0000}{1:00}{2:00}", now.Year, now.Month, now.Day);
            string productName = PlayerPrefs.GetString(Assets.Plugins.Dream.Editor.Build.Defined.kPublishProductKey);
            string ftpPath = PlayerPrefs.GetString(Assets.Plugins.Dream.AssetBundleManager.Editor.Defined.kPublishFTPPathKey);

            string zipFileName = string.Format("{0}_{1}_{2}_{3}", timeStr, productName, "client", assetBundleConfig.GameVersion);
            string zipFullPath = Path.Combine(projectDir.FullName, string.Format("{0}.{1}", zipFileName, "zip"));

            DreamEditor.Utilities.ZipHelper.ZipDirectory(fullPath, zipFullPath);


        }

        //private static void TransportDirectory(string localRoot, string remoteRoot, string src)
        //{
        //    try
        //    {
        //        var localFullPath = System.IO.Path.Combine(localRoot, src);
        //        var directorys = System.IO.Directory.GetDirectories(localFullPath);

        //        for (int i = 0; i < directorys.Length; i++)
        //        {
        //            var dir = directorys[i];
        //            if (!string.IsNullOrEmpty(dir))
        //            {
        //                var index = dir.IndexOf(localRoot);
        //                var dirPath = dir.Substring(index + localRoot.Length + 1);
        //                TransportDirectory(localRoot, remoteRoot, dirPath);
        //            }
        //        }

        //        var files = System.IO.Directory.GetFiles(localFullPath);
        //        for (int i = 0; i < files.Length; i++)
        //        {
        //            var file = files[i];
        //            var index = file.IndexOf(localRoot);
        //            var filePath = file.Substring(index + localRoot.Length + 1);
        //            var fileInDirPath = filePath.Substring(0, filePath.LastIndexOf('\\'));

        //            string destPath = System.IO.Path.Combine(remoteRoot, fileInDirPath);
        //            Transport(file, destPath, file);
        //        }
        //    }
        //    finally
        //    {

        //    }
        //}

        /// <summary>
        /// 向远程文件夹保存本地内容，或者从远程文件夹下载文件到本地
        /// </summary>
        /// <param name="src">要保存的文件的路径，如果保存文件到共享文件夹，这个路径就是本地文件路径如：@"D:\1.avi"</param>
        /// <param name="dst">保存文件的路径，不含名称及扩展名</param>
        /// <param name="fileName">保存文件的名称以及扩展名</param>
        private static void Transport(string src, string dst, string fileName)
        {

            FileStream inFileStream = new FileStream(src, FileMode.Open);
            if (!Directory.Exists(dst))
            {
                Directory.CreateDirectory(dst);
            }
            fileName = System.IO.Path.GetFileName(fileName);

            dst = System.IO.Path.Combine(dst, fileName);
            if (File.Exists(dst))
                File.Delete(dst);

            FileStream outFileStream = new FileStream(dst, FileMode.OpenOrCreate);

            byte[] buf = new byte[inFileStream.Length];
            int byteCount;
            while ((byteCount = inFileStream.Read(buf, 0, buf.Length)) > 0)
            {

                outFileStream.Write(buf, 0, byteCount);

            }

            inFileStream.Flush();
            inFileStream.Close();
            outFileStream.Flush();
            outFileStream.Close();
        }

        /// <summary>  
        /// Recursively delete all the files and folders under the specific path.  
        /// </summary>  
        /// <param name="path">The specific path</param>  
        private static void DeleteFilesAndFolders(string path)
        {
            // Delete files.  
            string[] files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                File.Delete(file);
            }

            // Delete folders.  
            string[] folders = Directory.GetDirectories(path);
            foreach (var folder in folders)
            {
                DeleteFilesAndFolders(folder);
                Directory.Delete(folder);
            }
        }

        private static string getConfigTextPath()
        {
            string resPath = Path.Combine(Application.dataPath, "Res");
            if (!Directory.Exists(resPath))
            {
                Directory.CreateDirectory(resPath);
            }

            string configsPath = Path.Combine(resPath, "configs");
            if (!Directory.Exists(configsPath))
            {
                Directory.CreateDirectory(configsPath);
            }

            string configPath = Path.Combine(configsPath, "config.txt");
            if (!File.Exists(configPath))
            {
                File.Create(configPath);
            }

            return configPath;
        }

        private static void ProcessConfig()
        {
            var configPath = getConfigTextPath();

            var configText = File.ReadAllText(configPath);
            AssetBundleConfig assetBundleConfig = new AssetBundleConfig(configText);

            var s = Application.dataPath;
            int revision = Assets.Plugins.Svn.Editor.SvnUtility.SVNRevision(s);
            string svnVersion = revision.ToString();

            var version = PlayerSettings.bundleVersion;
            string[] versionText = version.Split('.');

            string newVersionCode = "";
            if (versionText.Length < 2)
            {
                UnityEngine.Debug.LogError("version code err!");
                return;
            }

            newVersionCode += versionText[0];
            newVersionCode += ".";
            newVersionCode += versionText[1];
            newVersionCode += ".";
            newVersionCode += svnVersion;



            assetBundleConfig.GameVersion = newVersionCode;
            var output = assetBundleConfig.Serialize();

            FileStream fs = null;
            try
            {
                fs = File.Open(configPath, FileMode.Truncate, FileAccess.Write);
                fs.Seek(0, SeekOrigin.Begin);
                fs.SetLength(0);

                StreamWriter sw = new StreamWriter(fs);
                sw.Write(output);
                sw.Flush();
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError(ex);
            }
            finally
            {
                fs.Close();
            }
        }

        public static void WriteServerURL()
        {
            string downloadURL;
            if (string.IsNullOrEmpty(overloadedDevelopmentServerURL) == false)
            {
                downloadURL = overloadedDevelopmentServerURL;
            }
            else
            {
                IPHostEntry host;
                string localIP = "";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
                downloadURL = "http://" + localIP + ":7888/";
            }

            string assetBundleManagerResourcesDirectory = "Assets/Plugins/Dream/AssetBundleManager/Resources";
            string assetBundleUrlPath = Path.Combine(assetBundleManagerResourcesDirectory, "AssetBundleServerURL.bytes");
            Directory.CreateDirectory(assetBundleManagerResourcesDirectory);
            File.WriteAllText(assetBundleUrlPath, downloadURL);
            AssetDatabase.Refresh();
        }

        public static void BuildPlayer()
        {
            var allAssets = AssetDatabase.GetAllAssetPaths();

            List<string> scenePaths = new List<string>();
            foreach (var path in allAssets)
            {
                string normalPath = path.Replace("\\", "/");

                if (normalPath.IndexOf("Scenes/") > 0 && normalPath.IndexOf("Games/") > 0)
                {
                    string subPath = normalPath.Substring(normalPath.IndexOf("Games/") + "Games/".Length);
                    string assetBundleName = subPath.Substring(0, subPath.LastIndexOf("/"));
                    string outputPath = Path.Combine(Utility.AssetBundlesOutputPath, Utility.GetPlatformName());

                    string folderPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/") + 1);
                    string directoryPath = folderPath + outputPath.ToLower() + "/";
                    string fullPath = directoryPath + assetBundleName.ToLower() + ".unity3d";



                    fullPath = fullPath.Replace("\\", "/");
                    directoryPath = fullPath.Substring(0, fullPath.LastIndexOf("/"));
                    scenePaths.Add(path);

                    if (Directory.Exists(directoryPath) == false)
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    else
                    {
                        continue;
                    }

                    BuildPipeline.BuildPlayer(scenePaths.ToArray(), fullPath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.BuildAdditionalStreamedScenes);
                    scenePaths.Clear();
                }
            }
            return;
        }

        public static string GetBuildTargetName(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "/test.apk";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "/test.exe";
                case BuildTarget.StandaloneOSX:
                    return "/test.app";
                case BuildTarget.WebGL:
                    return "";
                // Add more build targets for your own.
                default:
                    Debug.Log("Target not implemented.");
                    return null;
            }
        }

        static void CopyAssetBundlesTo(string outputPath)
        {
            // Clear streaming assets folder.
            FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath);
            Directory.CreateDirectory(outputPath);

            string outputFolder = Utility.GetPlatformName();

            // Setup the source folder for assetbundles.
            var source = Path.Combine(Path.Combine(System.Environment.CurrentDirectory, Utility.AssetBundlesOutputPath), outputFolder);
            if (!System.IO.Directory.Exists(source))
                Debug.Log("No assetBundle output folder, try to build the assetBundles first.");

            // Setup the destination folder for assetbundles.
            var destination = System.IO.Path.Combine(outputPath, outputFolder);
            if (System.IO.Directory.Exists(destination))
                FileUtil.DeleteFileOrDirectory(destination);

            FileUtil.CopyFileOrDirectory(source, destination);
        }

        static string[] GetLevelsFromBuildSettings()
        {
            List<string> levels = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
            {
                if (EditorBuildSettings.scenes[i].enabled)
                    levels.Add(EditorBuildSettings.scenes[i].path);
            }

            return levels.ToArray();
        }
    }
}