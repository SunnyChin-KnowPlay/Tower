using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace Assets.Plugins.Svn.Editor
{

    public class SvnUtility
    {
        private const string SvnUtilityFilePath = @"Plugins\Svn\Editor\Sharp\SVNUtility.exe";


        /// <summary>
        /// 执行命令 只有revision命令才会返回对应的svn版本号
        /// </summary>
        /// <param name="command"></param>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static int ProcessCommand(string command, string path, string log = "auto")
        {
            string fullPath = path;
            string utlityPath = Path.Combine(Application.dataPath, SvnUtilityFilePath);



            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = utlityPath;
            start.UseShellExecute = false;    //是否使用操作系统shell启动
            start.CreateNoWindow = true;

            string cmd = string.Format("{0}" + " " + "{1}" + " " + "{2}" + " " + "{3}" + " " + "{4}", PlayerPrefs.GetString(ConfigWindow.kUsernameKey), PlayerPrefs.GetString(ConfigWindow.kPasswordKey), command, path, log);
            start.Arguments = cmd;

            Process p = Process.Start(start);
            p.WaitForExit();
            int ret = p.ExitCode;
            p.Close();
            return ret;

            //var fullPath = GetFileFullPath("TortoiseProc.exe");
            //if (fullPath.Length < 1)
            //	return;


            //ProcessStartInfo start = new ProcessStartInfo();
            //start.FileName = fullPath;
            //start.UseShellExecute = false;    //是否使用操作系统shell启动
            //start.CreateNoWindow = true;

            //string c = "/command:{0}" + " " + "/path:" + "\"{1}\"" + " " + "/closeonend:2";
            //c = string.Format(c, command, path);
            //start.Arguments = c;


            //Process p = Process.Start(start);
            //p.WaitForExit();
            //p.Close();
        }



        //[MenuItem("SVN/Update", false, 1)]
        public static void SVNUpdate()
        {
            EditorUtility.DisplayProgressBar("正在更新", "请稍后", 0.5f);
            ProcessCommand("update", SVNProjectPath);
            EditorUtility.DisplayProgressBar("正在解析", "请稍后", 0.8f);
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        //[MenuItem("SVN/Revert", false, 3)]
        public static void SVNRevert()
        {
            EditorUtility.DisplayProgressBar("正在回滚", "请稍后", 0.5f);
            ProcessCommand("revert", SVNProjectPath);
            EditorUtility.DisplayProgressBar("正在解析", "请稍后", 0.8f);
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        //[MenuItem("SVN/CleanUp", false, 4)]
        public static void SVNCleanUp()
        {
            EditorUtility.DisplayProgressBar("正在清理", "请稍后", 0.5f);
            ProcessCommand("cleanUp", SVNProjectPath);
            EditorUtility.DisplayProgressBar("正在解析", "请稍后", 0.8f);
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        public static int SVNRevision(string path)
        {
            return ProcessCommand("revision", path);
        }

        private static string SVNProjectPath
        {
            get
            {
                System.IO.DirectoryInfo parent = System.IO.Directory.GetParent(Application.dataPath);
                return parent.ToString();
            }
        }
    }
}