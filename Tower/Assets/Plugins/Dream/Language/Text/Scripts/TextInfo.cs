using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInfo
{
	public const char commaSeparator = ',';
	/// <summary>
	/// 键
	/// </summary>
	protected string m_Key;
	/// <summary>
	/// 值
	/// </summary>
	protected string m_Value;

	public string Key { get { return m_Key; } }
	public string Value { get { return m_Value; } }

	public TextInfo(string data)
	{
		var v = data.Split(commaSeparator);
		if (v.Length < 2)
		{
			return;
		}

		m_Key = v[0];
		m_Value = v[1];
	}
}
