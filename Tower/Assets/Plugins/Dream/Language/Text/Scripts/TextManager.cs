using DreamEngine.Utilities;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 本地化文本管理器
/// </summary>
public class TextManager : Singleton<TextManager>, IEnumerable<KeyValuePair<string, TextInfo>>
{
	private const char lineSeparator = '\r';
	private Dictionary<string, TextInfo> m_Infos = new Dictionary<string, TextInfo>();

	public TextManager()
	{

	}

	/// <summary>
	/// 通过csv解析文本
	/// </summary>
	/// <param name="data"></param>
	public void Parse(string data)
	{
		if (data.Length < 1)
			return;

		string[] lineList = data.Split(lineSeparator);

		TextInfo info = null;
		for (int i = 0; i < lineList.Length; i++)
		{
			string line = lineList[i];

			info = new TextInfo(line);
			if (!m_Infos.ContainsKey(info.Key))
			{
				m_Infos.Add(info.Key, info);
			}
		}
	}

	/// <summary>
	/// 获取字段
	/// </summary>
	/// <param name="key">键</param>
	/// <returns>如果返回空字符串，则代表着列表中找不到此文本</returns>
	public static string FindValue(string key)
	{
		var info = Instance[key];
		if (info == null)
			return "";

		return info.Key;
	}

	/// <summary>
	/// 获取文本信息
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public TextInfo this[string key]
	{
		get
		{
			if (m_Infos.ContainsKey(key) == false)
				return null;
			return m_Infos[key];
		}
	}

	public IEnumerator<KeyValuePair<string, TextInfo>> GetEnumerator()
	{
		return ((IEnumerable<KeyValuePair<string, TextInfo>>)m_Infos).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return m_Infos.GetEnumerator();
	}
}

