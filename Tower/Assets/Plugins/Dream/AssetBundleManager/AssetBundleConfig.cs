using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AssetBundles
{
    public class AssetBundleConfig
    {
        private Dictionary<string, string> configs = new Dictionary<string, string>();

        public string GameVersion
        {
            get
            {
                if (configs.ContainsKey("gameVersion"))
                    return configs["gameVersion"];
                return "";
            }
            set
            {
                value = Regex.Replace(value, @"\s+", " ");

                if (configs.ContainsKey("gameVersion"))
                    configs["gameVersion"] = value;
                else
                    configs.Add("gameVersion", value);
            }
        }

        public AssetBundleConfig(string text)
        {
            ParseWithText(text);
        }

        public void ParseWithText(string text)
        {
            text = HoverTreeClearMark(text);

            string[] texts = text.Split('\r');

            for (int i = 0; i < texts.Length; i++)
            {
                var textInLine = texts[i];

                textInLine = Regex.Replace(textInLine, @"\s+", "");
                if (textInLine.Contains("=") == false)
                    continue;

                if (textInLine.Contains("//"))
                    continue;

                var configText = textInLine.Split('=');
                configs.Add(configText[0], configText[1]);

                
            }
        }

        public string Serialize()
        {
            string data = "";

            foreach (var kvp in configs)
            {
                data += string.Format("{0} = {1}", kvp.Key, kvp.Value);
                data += "\r";
            }

            return data;
        }

        string HoverTreeClearMark(string input)
        {
            var reg = new Regex(@"(/\*.*?\*/)|//.*", RegexOptions.IgnoreCase);
            input = reg.Replace(input, "");
            return input;
        }
    }
}
