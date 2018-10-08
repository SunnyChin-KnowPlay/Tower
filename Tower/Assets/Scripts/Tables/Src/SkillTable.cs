using System.Collections.Generic;
using DreamEngine.Utilities;
using System.Collections;

namespace DreamEngine.Table
{
	public partial class SkillRow : BaseRow
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
		private int m_Energy;
		/// <summary>
		/// 消耗的能量
		/// </summary>
		public int Energy
		{
			get
			{
				return m_Energy;
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
		private int m_HpChanged;
		/// <summary>
		/// 血量基本变化值，如果是伤害则是-值加血则是正值0则不动。
		/// </summary>
		public int HpChanged
		{
			get
			{
				return m_HpChanged;
			}
		}
		public class TargetBuffChanged
		{
			public void InParse(string src)
			{
				var paramValue = src.Split('|');

				Parse(paramValue[0],ref m_Id);
				Parse(paramValue[1],ref m_Count);
			}

		 
			private int m_Id;
			/// <summary>
			/// 改变的buff的ID
			/// </summary>
			public int Id 
			{
				get
				{
					return m_Id;
				}
			}
		 
			private int m_Count;
			/// <summary>
			/// 改变的数量正值为加N层负值则代表消N层
			/// </summary>
			public int Count 
			{
				get
				{
					return m_Count;
				}
			}
		}

		private List<TargetBuffChanged> m_TargetBuffChangedList = new List<TargetBuffChanged>();	
		public List<TargetBuffChanged> TargetBuffChangedList
		{
			get
			{
				return m_TargetBuffChangedList;
			}
		}

		public class UpgradeCondition
		{
			public void InParse(string src)
			{
				var paramValue = src.Split('|');

				Parse(paramValue[0],ref m_Id);
				Parse(paramValue[1],ref m_Count);
				Parse(paramValue[2],ref m_Targets);
			}

		 
			private int m_Id;
			/// <summary>
			/// buff的ID
			/// </summary>
			public int Id 
			{
				get
				{
					return m_Id;
				}
			}
		 
			private int m_Count;
			/// <summary>
			/// Buff的数量
			/// </summary>
			public int Count 
			{
				get
				{
					return m_Count;
				}
			}
		 
			private uint m_Targets;
			/// <summary>
			/// 目标相对于施放技能的人查阅行动目标相关文档
			/// </summary>
			public uint Targets 
			{
				get
				{
					return m_Targets;
				}
			}
		}

		private List<UpgradeCondition> m_UpgradeConditionList = new List<UpgradeCondition>();	
		public List<UpgradeCondition> UpgradeConditionList
		{
			get
			{
				return m_UpgradeConditionList;
			}
		}

		private int m_UpgradeSkillId;
		/// <summary>
		/// 升级后的技能的ID
		/// </summary>
		public int UpgradeSkillId
		{
			get
			{
				return m_UpgradeSkillId;
			}
		}
		private uint m_Targets;
		/// <summary>
		/// 目标相对于技能的选取目标查阅行动目标相关文档
		/// </summary>
		public uint Targets
		{
			get
			{
				return m_Targets;
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
			Parse(paramList[1], ref m_Energy);
			Parse(paramList[2], ref m_Nature);
			Parse(paramList[3], ref m_Name);
			Parse(paramList[4], ref m_HpChanged);
			m_TargetBuffChangedList.Clear();
			var param5List = paramList[5].Split(':');
            for (int nCount = 0; nCount < 2; nCount++)
            {
				if(param5List[nCount] == "/")
					continue;

				m_TargetBuffChangedList.Add(new TargetBuffChanged());
				m_TargetBuffChangedList[nCount].InParse(param5List[nCount]);
            }
			m_UpgradeConditionList.Clear();
			var param6List = paramList[6].Split(':');
            for (int nCount = 0; nCount < 1; nCount++)
            {
				if(param6List[nCount] == "/")
					continue;

				m_UpgradeConditionList.Add(new UpgradeCondition());
				m_UpgradeConditionList[nCount].InParse(param6List[nCount]);
            }
			Parse(paramList[7], ref m_UpgradeSkillId);
			Parse(paramList[8], ref m_Targets);
			Parse(paramList[9], ref m_Icon);
		}

		public TargetBuffChanged TargetBuffChangedFirst
		{
			get
			{
				if(TargetBuffChangedList.Count <= 0)
					return null;

				return TargetBuffChangedList[0];
			}
		}
		public UpgradeCondition UpgradeConditionFirst
		{
			get
			{
				if(UpgradeConditionList.Count <= 0)
					return null;

				return UpgradeConditionList[0];
			}
		}
	}

	public partial class SkillTable : Singleton<SkillTable>, IEnumerable<KeyValuePair<int, SkillRow>>
	{
		private const char rowSeparator = '\r';
		private Dictionary<int, SkillRow> m_Rows = new Dictionary<int, SkillRow>();

		public delegate void OnParseFinished(string strData);
		public event OnParseFinished ParseFinished;

		internal void Parse(string data)
		{
			if (data.Length < 1)
				return;

			string[] rowList = data.Split(rowSeparator);

			SkillRow row = null;
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
					row = new SkillRow();
					row.Parse(rowText);
					m_Rows.Add(row.Id, row);
				}
			}
		}

		public bool ContainsKey(int key)
		{
			return m_Rows.ContainsKey(key);
		}

		public SkillRow GetRow(int key)
        {
			if (!m_Rows.ContainsKey(key))
			{
				UnityEngine.Debug.LogError(string.Format("not found key:{0} and with in:{1} table.", key, "Skill"));
				return null;
			}
            return m_Rows[key];
        }

        public SkillRow this[int key]
		{
			get
			{
				if (!m_Rows.ContainsKey(key))
				{
					UnityEngine.Debug.LogError(string.Format("not found key:{0} and with in:{1} table.", key, "Skill"));
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

		public IEnumerator<KeyValuePair<int, SkillRow>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<int, SkillRow>>)m_Rows).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<int, SkillRow>>)m_Rows).GetEnumerator();
		}
	}
}

