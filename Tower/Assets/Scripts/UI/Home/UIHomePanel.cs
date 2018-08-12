using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHomePanel : UIPanel
{
    public const string Key = "UI/Prefabs/Home/HomePanel";

    /// <summary>
    /// 标题文本
    /// </summary>
    private Text titleText = null;

    protected override void Awake()
    {
        base.Awake();

        titleText = this.transform.Find("TopBar/TitleText").GetComponent<Text>();
        if (null != titleText)
        {
            titleText.text = "老家";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickOpenTask()
    {
        var taskPanel = uiManager.LoadPanel<UITaskPanel>(UITaskPanel.Key);
        if (null != taskPanel)
        {
            uiManager.Push(taskPanel);
        }
    }
}
