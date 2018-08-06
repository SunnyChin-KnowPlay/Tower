using System.Collections.Generic;
using DreamEngine.Utilities;

namespace DreamEngine.Table
{
	

	public partial class TableManager : Singleton<TableManager>
	{
		public void Parse(Dictionary<string, string> dict)
		{
			SpritesModel SpritesModelObj = SpritesModel.Instance;
			if (dict.ContainsKey("SpritesModel"))
				SpritesModelObj.Parse(dict["SpritesModel"]);
		}
	}
}

