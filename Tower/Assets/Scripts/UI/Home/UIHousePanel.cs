using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHousePanel : UIPanel
{


    public const string Key = "UI/Prefabs/Home/HousePanel";

    // Update is called once per frame
    void Update () {
		
	}
    public void OnClickOpenHouse()
    {
        var housePanel = uiManager.LoadPanel<UIHousePanel>(UIHousePanel.Key);
        if (null != housePanel)
        {
            uiManager.Push(housePanel);
        }
    }
}
