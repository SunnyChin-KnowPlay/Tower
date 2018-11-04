using System;
using System.IO;

namespace ProtocolUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
                return;

            if (args[0].Length < 1 || args[1].Length < 1)
            {
                Console.WriteLine("路径配置错误");
                return;
            }

            string srcPath = args[0];
            string targetPath = args[1];


            ProcessProtocolSrc(srcPath, targetPath);
        }

        static void ProcessProtocolSrc(string srcPath, string targetPath)
        {

            string protocolsCodePath = targetPath;
            if (Directory.Exists(protocolsCodePath) == false)
            {
                Directory.CreateDirectory(protocolsCodePath);
            }

            var folders = Directory.GetDirectories(protocolsCodePath);
            foreach (var folder in folders)
            {
                foreach (var file in Directory.GetFiles(folder))
                {
                    File.Delete(file);
                }
            }

            var files = Directory.GetFiles(protocolsCodePath);
            foreach (var file in files)
            {
                File.Delete(file);
            }

            var filePaths = Directory.GetFiles(srcPath);

            DreamEditor.Net.Parser.ClearProtocolGenerates();
            DreamEditor.Net.Parser.ParseSourceEnum(filePaths, protocolsCodePath);

            try
            {
                Console.WriteLine("开始处理协议");
                int count = filePaths.Length;
                int i = 0;
                foreach (var filePath in filePaths)
                {
                    var protocolName = Path.GetFileNameWithoutExtension(filePath);

                    i++;
                    float percent = (float)i / (float)count;
                    Console.WriteLine(string.Format("{0}, {1}{2}", "正在处理协议", "协议:", protocolName));

                    DreamEditor.Net.Parser.ParseSourceWithGenerateCode(filePath, protocolsCodePath);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("解析错误" + ex);
            }
            finally
            {

            }
            if (filePaths.Length > 0)
            {
                DreamEditor.Net.Parser.GenerateProtocolManager(Path.Combine(protocolsCodePath, "ProtocolManager.cs"));
                Console.WriteLine("处理完毕");
            }
        }
    }
}
