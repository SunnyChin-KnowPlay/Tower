using System.Collections.Generic;
using DreamEngine.Utilities;
using System.Collections;

namespace DreamEngine.Table
{
	public partial class SpritesInfo : BaseInfo
	{
		private int m_Id;
		/// <summary>
		/// 唯一ID
		/// </summary>
		public int Id
		{
			get
			{
				return m_Id;
			}
		}
		private int m_Type;
		/// <summary>
		/// 精灵类型
		/// </summary>
		public int Type
		{
			get
			{
				return m_Type;
			}
		}
		private string m_Name;
		/// <summary>
		/// 目标名称
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
		}
		private int m_MaxLevel;
		/// <summary>
		/// 最大等级
		/// </summary>
		public int MaxLevel
		{
			get
			{
				return m_MaxLevel;
			}
		}

		internal void Parse(string line)
		{
			string[] paramList = line.Split(commaSeparator);

			Parse(paramList[0], ref m_Id);
			Parse(paramList[1], ref m_Type);
			Parse(paramList[2], ref m_Name);
			Parse(paramList[3], ref m_MaxLevel);
		}

	}

	public partial class SpritesModel : Singleton<SpritesModel>, IEnumerable<KeyValuePair<int, SpritesInfo>>
	{
		private const char lineSeparator = '\r';
		private Dictionary<int, SpritesInfo> m_Infos = new Dictionary<int, SpritesInfo>();

		public delegate void OnParseFinished(string strData);
		public event OnParseFinished ParseFinished;

		internal void Parse(string data)
		{
			if (data.Length < 1)
				return;

			string[] lineList = data.Split(lineSeparator);

			SpritesInfo info = null;
			for (int i = 0; i < lineList.Length; i++)
			{
				string line = lineList[i];

				if (line.IndexOf(BaseInfo.commaSeparator) < 0)
					continue;

				int mainKey = default(int);
				BaseInfo.Parse(line.Substring(0, line.IndexOf(BaseInfo.commaSeparator)), ref mainKey);

				if (m_Infos.ContainsKey(mainKey))
				{
					info = m_Infos[mainKey];
					info.Parse(line);
				}
				else
				{
					info = new SpritesInfo();
					info.Parse(line);
					m_Infos.Add(info.Id, info);
				}
			}
		}

		public bool ContainsKey(int key)
		{
			return m_Infos.ContainsKey(key);
		}

		public SpritesInfo GetInfo(int key)
        {
			if (!m_Infos.ContainsKey(key))
			{
				UnityEngine.Debug.LogError(string.Format("not found key:{0} and with in:{1} table.", key, "Sprites"));
				return null;
			}
            return m_Infos[key];
        }

        public SpritesInfo this[int key]
		{
			get
			{
				if (!m_Infos.ContainsKey(key))
				{
					UnityEngine.Debug.LogError(string.Format("not found key:{0} and with in:{1} table.", key, "Sprites"));
					return null;
				}
				return m_Infos[key];
			}
		}

		public int Count
		{
			get
			{
				return m_Infos.Count;
			}
		}

		public IEnumerator<KeyValuePair<int, SpritesInfo>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<int, SpritesInfo>>)m_Infos).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<int, SpritesInfo>>)m_Infos).GetEnumerator();
		}
	}
}

