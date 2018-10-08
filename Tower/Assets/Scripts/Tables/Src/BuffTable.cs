using System.Collections.Generic;
using DreamEngine.Utilities;
using System.Collections;

namespace DreamEngine.Table
{
	public partial class BuffRow : BaseRow
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
		private int m_Nature;
		/// <summary>
		/// 自然天赋
		/// </summary>
		public int Nature
		{
			get
			{
				return m_Nature;
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
		private int m_Duration;
		/// <summary>
		/// 存在的回合数
		/// </summary>
		public int Duration
		{
			get
			{
				return m_Duration;
			}
		}
		public class Trigger
		{
			public void InParse(string src)
			{
				var paramValue = src.Split('|');

				Parse(paramValue[0],ref m_Moment);
				Parse(paramValue[1],ref m_ActionType);
				Parse(paramValue[2],ref m_Param1);
				Parse(paramValue[3],ref m_Targets);
			}

		 
			private uint m_Moment;
			/// <summary>
			/// 时刻节点，查阅枚举
			/// </summary>
			public uint Moment 
			{
				get
				{
					return m_Moment;
				}
			}
		 
			private uint m_ActionType;
			/// <summary>
			/// 行为类型，查阅枚举
			/// </summary>
			public uint ActionType 
			{
				get
				{
					return m_ActionType;
				}
			}
		 
			private int m_Param1;
			/// <summary>
			/// 参数1
			/// </summary>
			public int Param1 
			{
				get
				{
					return m_Param1;
				}
			}
		 
			private uint m_Targets;
			/// <summary>
			/// 目标位
			/// </summary>
			public uint Targets 
			{
				get
				{
					return m_Targets;
				}
			}
		}

		private List<Trigger> m_TriggerList = new List<Trigger>();	
		public List<Trigger> TriggerList
		{
			get
			{
				return m_TriggerList;
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
			Parse(paramList[1], ref m_Nature);
			Parse(paramList[2], ref m_Name);
			Parse(paramList[3], ref m_Duration);
			m_TriggerList.Clear();
			var param4List = paramList[4].Split(':');
            for (int nCount = 0; nCount < 1; nCount++)
            {
				if(param4List[nCount] == "/")
					continue;

				m_TriggerList.Add(new Trigger());
				m_TriggerList[nCount].InParse(param4List[nCount]);
            }
			Parse(paramList[5], ref m_Icon);
		}

		public Trigger TriggerFirst
		{
			get
			{
				if(TriggerList.Count <= 0)
					return null;

				return TriggerList[0];
			}
		}
	}

	public partial class BuffTable : Singleton<BuffTable>, IEnumerable<KeyValuePair<int, BuffRow>>
	{
		private const char rowSeparator = '\r';
		private Dictionary<int, BuffRow> m_Rows = new Dictionary<int, BuffRow>();

		public delegate void OnParseFinished(string strData);
		public event OnParseFinished ParseFinished;

		internal void Parse(string data)
		{
			if (data.Length < 1)
				return;

			string[] rowList = data.Split(rowSeparator);

			BuffRow row = null;
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
					row = new BuffRow();
					row.Parse(rowText);
					m_Rows.Add(row.Id, row);
				}
			}
		}

		public bool ContainsKey(int key)
		{
			return m_Rows.ContainsKey(key);
		}

		public BuffRow GetRow(int key)
        {
			if (!m_Rows.ContainsKey(key))
			{
				UnityEngine.Debug.LogError(string.Format("not found key:{0} and with in:{1} table.", key, "Buff"));
				return null;
			}
            return m_Rows[key];
        }

        public BuffRow this[int key]
		{
			get
			{
				if (!m_Rows.ContainsKey(key))
				{
					UnityEngine.Debug.LogError(string.Format("not found key:{0} and with in:{1} table.", key, "Buff"));
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

		public IEnumerator<KeyValuePair<int, BuffRow>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<int, BuffRow>>)m_Rows).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<int, BuffRow>>)m_Rows).GetEnumerator();
		}
	}
}

