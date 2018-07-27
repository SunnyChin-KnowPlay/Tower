using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Dream.Editor.Utilities
{
    public partial class Utility
    {
        [MenuItem("Utility/Process Protocols")]
        static void ProcessProtocolSrc()
        {
            string srcPath = PlayerPrefs.GetString(Defined.kProtocolsPathKey);
            if (srcPath.Length < 1)
                return;

            string targetPath = PlayerPrefs.GetString(Defined.kProtocolCodePathKey);
            if (targetPath.Length < 1)
                return;

            string protocolsCodePath = Path.Combine(Application.dataPath, targetPath);
            if (Directory.Exists(protocolsCodePath) == false)
            {
                Directory.CreateDirectory(protocolsCodePath);
            }

            var folders = Directory.GetDirectories(Path.Combine(Application.dataPath, targetPath));
            foreach (var folder in folders)
            {
                foreach (var file in Directory.GetFiles(folder))
                {
                    File.Delete(file);
                }
            }

            var files = Directory.GetFiles(Path.Combine(Application.dataPath, targetPath));
            foreach (var file in files)
            {
                File.Delete(file);
            }

            var filePaths = Directory.GetFiles(srcPath);

            DreamEditor.Net.Parser.ClearProtocolGenerates();

            DreamEditor.Net.Parser.ParseSourceEnum(filePaths, protocolsCodePath);

            try
            {
                EditorUtility.DisplayProgressBar("正在处理协议", "请稍后", 0.0f);
                int count = filePaths.Length;
                int i = 0;
                foreach (var filePath in filePaths)
                {
                    var protocolName = Path.GetFileNameWithoutExtension(filePath);

                    i++;
                    float percent = (float)i / (float)count;
                    EditorUtility.DisplayProgressBar("正在处理协议", "协议:" + protocolName, percent);
                    DreamEditor.Net.Parser.ParseSourceWithGenerateCode(filePath, protocolsCodePath);
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError(ex);
            }
            finally
            {

            }
            if (filePaths.Length > 0)
            {
                DreamEditor.Net.Parser.GenerateProtocolManager(Path.Combine(protocolsCodePath, "ProtocolManager.cs"));
                AssetDatabase.Refresh();
            }

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        //[MenuItem("Utility/Process Protocols All In One")]
        //static void ProcessProtocolSrcAllInOne()
        //{
        //    string srcPath = PlayerPrefs.GetString(Defined.kProtocolsPathKey);
        //    if (srcPath.Length < 1)
        //        return;

        //    string targetPath = PlayerPrefs.GetString(Defined.kProtocolCodePathKey);
        //    if (targetPath.Length < 1)
        //        return;

        //    string protocolsCodePath = Path.Combine(Application.dataPath, targetPath);
        //    if (Directory.Exists(protocolsCodePath) == false)
        //    {
        //        Directory.CreateDirectory(protocolsCodePath);
        //    }

        //    var targetFilePath = Path.Combine(protocolsCodePath, "Protocols.cs");

        //    DreamEditor.Net.Parser.GenerateAllProtocol(srcPath, targetFilePath);
        //}


        [MenuItem("Utility/Process Tables")]
        static void ProcessTableSrc()
        {
            string srcPath = PlayerPrefs.GetString(Defined.kTablesPathKey);
            if (srcPath.Length < 1)
                return;

            string codePath = PlayerPrefs.GetString(Defined.kTableCodePathKey);
            if (codePath.Length < 1)
                return;

            string resPath = PlayerPrefs.GetString(Defined.kTableResPathKey);
            if (resPath.Length < 1)
                return;

            List<string> modelList = new List<string>();
            var filePaths = Directory.GetFiles(srcPath);
            string tablesCodePath = Path.Combine(Application.dataPath, codePath);
            if (Directory.Exists(tablesCodePath) == false)
            {
                Directory.CreateDirectory(tablesCodePath);
            }

            string tableCSVPath = Path.Combine(Application.dataPath, resPath);
            if (Directory.Exists(tableCSVPath) == false)
            {
                Directory.CreateDirectory(tableCSVPath);
            }

            try
            {
                EditorUtility.DisplayProgressBar("正在处理表格", "请稍后", 0.0f);

                int filesCount = filePaths.Length;
                int i = 0;

                foreach (var filePath in filePaths)
                {
                    if (Path.GetExtension(filePath).Contains(".meta"))
                        continue;

                    if (filePath.Contains("~"))
                        continue;

                    i++;
                    float percent = (float)i / (float)filesCount;

                    string modelName = Path.GetFileNameWithoutExtension(filePath);
                    string codeFileName = Path.Combine(tablesCodePath, modelName + "Model.cs");
                    string csvFileName = Path.Combine(tableCSVPath, modelName + "Model.csv");

                    EditorUtility.DisplayProgressBar("正在处理表格", "表格:" + modelName, percent);

                    bool ret = DreamEditor.Table.Parser.ParseExcelWithGenerateModel(filePath, "Property", modelName, codeFileName, csvFileName);
                    if (ret)
                    {
                        modelList.Add(modelName);
                    }
                }

                if (modelList.Count > 0)
                {
                    string managerCodePath = Path.Combine(Application.dataPath, Path.Combine(codePath, "TableManager.cs"));
                    DreamEditor.Table.Parser.GenerateManagerCode(modelList.ToArray(), managerCodePath);

                    AssetDatabase.Refresh();
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError("Parse table Err:" + ex);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

        }
    }
}
