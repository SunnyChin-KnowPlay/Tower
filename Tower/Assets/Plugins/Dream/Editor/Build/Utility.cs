#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
#endif

namespace Assets.Plugins.Dream.Editor.Build
{
    public class SharedTool : IDisposable
    {
        // obtains user token         
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        // closes open handes returned by LogonUser         
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        extern static bool CloseHandle(IntPtr handle);

        [DllImport("Advapi32.DLL")]
        static extern bool ImpersonateLoggedOnUser(IntPtr hToken);

        [DllImport("Advapi32.DLL")]
        static extern bool RevertToSelf();
        const int LOGON32_PROVIDER_DEFAULT = 0;
        const int LOGON32_LOGON_NEWCREDENTIALS = 9;//域控中的需要用:Interactive = 2         
        private bool disposed;

        public SharedTool(string username, string password, string ip)
        {
            // initialize tokens         
            IntPtr pExistingTokenHandle = new IntPtr(0);
            IntPtr pDuplicateTokenHandle = new IntPtr(0);

            try
            {
                // get handle to token         
                bool bImpersonated = LogonUser(username, ip, password,
LOGON32_LOGON_NEWCREDENTIALS, LOGON32_PROVIDER_DEFAULT, ref pExistingTokenHandle);

                if (bImpersonated)
                {
                    if (!ImpersonateLoggedOnUser(pExistingTokenHandle))
                    {
                        int nErrorCode = Marshal.GetLastWin32Error();
                        throw new Exception("ImpersonateLoggedOnUser error;Code=" + nErrorCode);
                    }
                }
                else
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    throw new Exception("LogonUser error;Code=" + nErrorCode);
                }
            }
            finally
            {
                // close handle(s)         
                if (pExistingTokenHandle != IntPtr.Zero)
                    CloseHandle(pExistingTokenHandle);
                if (pDuplicateTokenHandle != IntPtr.Zero)
                    CloseHandle(pDuplicateTokenHandle);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                RevertToSelf();
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }


    public class Utility
    {


        private static void ResetVersion()
        {
#if (UNITY_ANDROID)
            PlayerSettings.Android.bundleVersionCode = PlayerSettings.Android.bundleVersionCode + 1;
#elif UNITY_IOS
            var buildNumber = int.Parse(PlayerSettings.iOS.buildNumber);
            PlayerSettings.iOS.buildNumber = (buildNumber + 1).ToString();
#endif
        }

        [MenuItem("Publish/Build")]
        static void Build()
        {
            string locationPath = PlayerPrefs.GetString(Defined.kPublishPathKey);
            if (locationPath.Length < 1)
                return;

            BuildPlayerOptions options = new BuildPlayerOptions();

            List<string> sceneNames = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                    sceneNames.Add(scene.path);
            }

            ResetVersion();

            options.scenes = sceneNames.ToArray();
            options.target = EditorUserBuildSettings.activeBuildTarget;
            options.targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string packageName = GetPackageName(EditorUserBuildSettings.activeBuildTarget);
            if (options.target == BuildTarget.StandaloneWindows || options.target == BuildTarget.StandaloneWindows64)
                packageName += ".exe";
            else if (options.target == BuildTarget.Android)
                packageName += ".apk";

            string fullPath = Path.Combine(locationPath, packageName);
            options.locationPathName = fullPath;
            var result = BuildPipeline.BuildPlayer(options);

            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 向远程文件夹保存本地内容，或者从远程文件夹下载文件到本地
        /// </summary>
        /// <param name="src">要保存的文件的路径，如果保存文件到共享文件夹，这个路径就是本地文件路径如：@"D:\1.avi"</param>
        /// <param name="dst">保存文件的路径，不含名称及扩展名</param>
        /// <param name="fileName">保存文件的名称以及扩展名</param>
        public static void Transport(string src, string dst, string fileName)
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

        private static string GetPackageName(BuildTarget target)
        {
            var version = PlayerSettings.bundleVersion;

            var buildCode = GetBuildCode();

            string productName = PlayerPrefs.GetString(Defined.kPublishProductKey);
            if (productName.Length < 1)
            {
                UnityEngine.Debug.LogError("package name err!");
                throw new Exception("Err!");
            }

            string name = string.Format("{0}/{1}_{2}_{3}_cy", GetPlatformName(target), productName, version, buildCode);
            return name;
        }

        private static string GetBuildCode()
        {
            string buildCode = "";

#if (UNITY_ANDROID)
            buildCode = string.Format("{0}", PlayerSettings.Android.bundleVersionCode);
#elif UNITY_IOS
            buildCode = PlayerSettings.iOS.buildNumber;
#endif
            return buildCode;
        }

        private static string GetPlatformName(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.WebGL:
                    return "WebGL";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
                case BuildTarget.StandaloneOSX:
                    return "OSX";
                default:
                    return null;
            }
        }
    }
}
