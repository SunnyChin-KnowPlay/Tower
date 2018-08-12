using System.Collections.Generic;
using DreamEngine.Utilities;
using System.Collections;

namespace DreamEngine.Table
{
	public partial class SpritesRow : BaseRow
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
		private int m_MakeSrcID;
		/// <summary>
		/// 合成原
		/// </summary>
		public int MakeSrcID
		{
			get
			{
				return m_MakeSrcID;
			}
		}
		private int m_MakeNum;
		/// <summary>
		/// 合成数量
		/// </summary>
		public int MakeNum
		{
			get
			{
				return m_MakeNum;
			}
		}
		private int m_CreateTargetID;
		/// <summary>
		/// 产生目标
		/// </summary>
		public int CreateTargetID
		{
			get
			{
				return m_CreateTargetID;
			}
		}
		private int m_CreateNum;
		/// <summary>
		/// 产生的数量
		/// </summary>
		public int CreateNum
		{
			get
			{
				return m_CreateNum;
			}
		}
		private string m_Icon;
		/// <summary>
		/// 图标
		/// </summary>
		public string Icon
		{
			get
			{
				return m_Icon;
			}
		}

		internal void Parse(string row)
		{
			string[] paramList = row.Split(commaSeparator);

			Parse(paramList[0], ref m_Id);
			Parse(paramList[1], ref m_Type);
			Parse(paramList[2], ref m_Name);
			Parse(paramList[3], ref m_MaxLevel);
			Parse(paramList[4], ref m_MakeSrcID);
			Parse(paramList[5], ref m_MakeNum);
			Parse(paramList[6], ref m_CreateTargetID);
			Parse(paramList[7], ref m_CreateNum);
			Parse(paramList[8], ref m_Icon);
		}

	}

	public partial class SpritesTable : Singleton<SpritesTable>, IEnumerable<KeyValuePair<int, SpritesRow>>
	{
		private const char rowSeparator = '\r';
		private Dictionary<int, SpritesRow> m_Rows = new Dictionary<int, SpritesRow>();

		public delegate void OnParseFinished(string strData);
		public event OnParseFinished ParseFinished;

		internal void Parse(string data)
		{
			if (data.Length < 1)
				return;

			string[] rowList = data.Split(rowSeparator);

			SpritesRow row = null;
			for (int i = 0; i < rowList.Length; i++)
			{
				string rowText = rowList[i];

				if (rowText.IndexOf(BaseRow.commaSeparator) < 0)
					continue;

				int mainKey = default(int);
				BaseRow.Parse(rowText.Substring(0, rowText.IndexOf(BaseRow.commaSeparator)), ref mainKey);

				if (m_Rows.ContainsKey(mainKey))
				{
					row = m_Rows[mainKey];
					row.Parse(rowText);
				}
				else
				{
					row = new SpritesRow();
					row.Parse(rowText);
					m_Rows.Add(row.Id, row);
				}
			}
		}

		public bool ContainsKey(int key)
		{
			return m_Rows.ContainsKey(key);
		}

		public SpritesRow GetRow(int key)
        {
			if (!m_Rows.ContainsKey(key))
			{
				UnityEngine.Debug.LogError(string.Format("not found key:{0} and with in:{1} table.", key, "Sprites"));
				return null;
			}
            return m_Rows[key];
        }

        public SpritesRow this[int key]
		{
			get
			{
				if (!m_Rows.ContainsKey(key))
				{
					UnityEngine.Debug.LogError(string.Format("not found key:{0} and with in:{1} table.", key, "Sprites"));
					return null;
				}
				return m_Rows[key];
			}
		}

		public int Count
		{
			get
			{
				return m_Rows.Count;
			}
		}

		public IEnumerator<KeyValuePair<int, SpritesRow>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<int, SpritesRow>>)m_Rows).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<int, SpritesRow>>)m_Rows).GetEnumerator();
		}
	}
}

