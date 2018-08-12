using System.Collections.Generic;
using DreamEngine.Utilities;

namespace DreamEngine.Table
{
	

	public partial class TableManager : Singleton<TableManager>
	{
		public void Parse(Dictionary<string, string> dict)
		{
			HousesTable HousesTableObj = HousesTable.Instance;
			if (dict.ContainsKey("HousesTable"))
				HousesTableObj.Parse(dict["HousesTable"]);
			SpritesTable SpritesTableObj = SpritesTable.Instance;
			if (dict.ContainsKey("SpritesTable"))
				SpritesTableObj.Parse(dict["SpritesTable"]);
		}
	}
}

