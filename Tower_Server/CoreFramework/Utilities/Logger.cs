using System;
using System.IO;
using System.Text;

namespace Dream.Server.CoreFramework.Utilities
{
    public static class Logger
    {
        public static string s_LogDirPath = "";

        private static MemoryStream s_MemoryStream = null;
        private static StreamWriter s_StreamWriter = null;

        private const string m_InfoLog = "Info";
        private const string m_WarningLog = "Warning";
        private const string m_ErrorLog = "Error";

        /// <summary>
        /// 打印到流体
        /// </summary>
        /// <param name="level">级别，分为信息输出，警告输出，错误输出等</param>
        /// <param name="cmd">指令</param>
        /// <param name="body">输出体</param>
        private static void Log4Stream(string level, Object cmd, Object body)
        {
            CheckStreamIsReady();

            var dt = DateTime.Now;
            string timeStr = string.Format("{0}:{1}:{2}", dt.Hour, dt.Minute, dt.Second);

            string data = null;

            if (null != cmd && null != body)
                data = string.Format("{0}\t{1}\t{2}\t{3}", level, timeStr, cmd.ToString(), body.ToString());
            else if(null != cmd)
                data = string.Format("{0}\t{1}\t{2}\t{3}", level, timeStr, cmd.ToString(), "");
            else
                data = string.Format("{0}\t{1}\t{2}\t{3}", level, timeStr, "", "");

            lock (s_StreamWriter)
                s_StreamWriter.WriteLine(data);
        }

        public static void Log(Object cmd, Object body)
        {
            Log4Stream(m_InfoLog, cmd, body);
        }

        public static void LogWarning(Object cmd, Object body)
        {
            Log4Stream(m_WarningLog, cmd, body);
        }

        public static void LogError(Object cmd, Object body)
        {
            Log4Stream(m_ErrorLog, cmd, body);
        }

        public static void Write()
        {
            if (s_LogDirPath.Length < 1)
                return;

            if (s_StreamWriter == null)
                return;

            if (s_MemoryStream == null)
                return;

            try
            {
                lock (s_StreamWriter)
                {
                    s_StreamWriter.Flush();

                    using (var sw = File.Open(GetFilePath(), FileMode.Append))
                    {
                        sw.Write(s_MemoryStream.ToArray(), 0, (int)s_MemoryStream.Length);
                        s_MemoryStream.Seek(0, SeekOrigin.Begin);
                        s_MemoryStream.SetLength(0);
                    }
                }
            }
            catch
            {

            }
        }

        private static void CheckStreamIsReady()
        {
            if (s_StreamWriter == null)
            {
                if (s_MemoryStream == null)
                {
                    s_MemoryStream = new MemoryStream();
                }
                s_StreamWriter = new StreamWriter(s_MemoryStream);
            }
        }

        private static string GetFilePath()
        {
            var time = DateTime.Today;
            string timeDesc = string.Format("{0}_{1}_{2}", time.Year, time.Month, time.Day);
            string nowFilePath = string.Format("{0}/{1}/{2}.csv", s_LogDirPath, "Log", timeDesc);

            if (!File.Exists(nowFilePath))
            {
                string dirPath = nowFilePath.Substring(0, nowFilePath.LastIndexOf('/'));

                try
                {
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    using (var fs = File.Create(nowFilePath))
                    {
                        fs.Close();
                    }
                }
                catch
                {
                    throw;
                }
            }

            return nowFilePath;
        }
    }
}
