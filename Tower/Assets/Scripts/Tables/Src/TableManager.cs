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
			MinionTable MinionTableObj = MinionTable.Instance;
			if (dict.ContainsKey("MinionTable"))
				MinionTableObj.Parse(dict["MinionTable"]);
			PlayerTable PlayerTableObj = PlayerTable.Instance;
			if (dict.ContainsKey("PlayerTable"))
				PlayerTableObj.Parse(dict["PlayerTable"]);
			SkillTable SkillTableObj = SkillTable.Instance;
			if (dict.ContainsKey("SkillTable"))
				SkillTableObj.Parse(dict["SkillTable"]);
		}
	}
}

