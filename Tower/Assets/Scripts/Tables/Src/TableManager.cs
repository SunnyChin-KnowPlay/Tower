using System.Collections.Generic;
using DreamEngine.Utilities;

namespace DreamEngine.Table
{
	

	public partial class TableManager : Singleton<TableManager>
	{
		public void Parse(Dictionary<string, string> dict)
		{
			BuffTable BuffTableObj = BuffTable.Instance;
			if (dict.ContainsKey("BuffTable"))
				BuffTableObj.Parse(dict["BuffTable"]);
			SkillTable SkillTableObj = SkillTable.Instance;
			if (dict.ContainsKey("SkillTable"))
				SkillTableObj.Parse(dict["SkillTable"]);
		}
	}
}

