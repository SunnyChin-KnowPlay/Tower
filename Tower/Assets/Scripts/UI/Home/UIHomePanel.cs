using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHomePanel : UIPanel
{
    public const string Key = "UI/Prefabs/Home/HomePanel";

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickOpenTask()
    {
        var taskPanel = uiManager.LoadPanel<UITaskPanel>(UITaskPanel.Key);
        if(null != taskPanel)
        {
            uiManager.Push(taskPanel);
        }
    }
}
