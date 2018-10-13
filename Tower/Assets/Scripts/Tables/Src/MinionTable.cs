using System.Collections.Generic;
using DreamEngine.Utilities;
using System.Collections;

namespace DreamEngine.Table
{
	public partial class MinionRow : BaseRow
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
		private string m_Name;
		/// <summary>
		/// 名字
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
		}
		private string m_ModelName;
		/// <summary>
		/// 模型名
		/// </summary>
		public string ModelName
		{
			get
			{
				return m_ModelName;
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
			Parse(paramList[1], ref m_Name);
			Parse(paramList[2], ref m_ModelName);
			Parse(paramList[3], ref m_Icon);
		}

	}

	public partial class MinionTable : Singleton<MinionTable>, IEnumerable<KeyValuePair<int, MinionRow>>
	{
		private const char rowSeparator = '\r';
		private Dictionary<int, MinionRow> m_Rows = new Dictionary<int, MinionRow>();

		public delegate void OnParseFinished(string strData);
		public event OnParseFinished ParseFinished;

		internal void Parse(string data)
		{
			if (data.Length < 1)
				return;

			string[] rowList = data.Split(rowSeparator);

			MinionRow row = null;
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
					row = new MinionRow();
					row.Parse(rowText);
					m_Rows.Add(row.Id, row);
				}
			}
		}

		public bool ContainsKey(int key)
		{
			return m_Rows.ContainsKey(key);
		}

		public MinionRow GetRow(int key)
        {
			if (!m_Rows.ContainsKey(key))
			{
				UnityEngine.Debug.LogError(string.Format("not found key:{0} and with in:{1} table.", key, "Minion"));
				return null;
			}
            return m_Rows[key];
        }

        public MinionRow this[int key]
		{
			get
			{
				if (!m_Rows.ContainsKey(key))
				{
					UnityEngine.Debug.LogError(string.Format("not found key:{0} and with in:{1} table.", key, "Minion"));
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

		public IEnumerator<KeyValuePair<int, MinionRow>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<int, MinionRow>>)m_Rows).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<int, MinionRow>>)m_Rows).GetEnumerator();
		}
	}
}

