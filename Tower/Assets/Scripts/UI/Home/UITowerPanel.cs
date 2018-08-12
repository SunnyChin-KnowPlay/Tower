using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITowerPanel : UIPanel
{
    public const string Key = "UI/Prefabs/Home/TowerPanel";

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
            titleText.text = "炮塔";
        }
    }

    protected override void Start()
    {
        base.Start();


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickOpenTower()
    {
        var towerPanel = uiManager.LoadPanel<UITowerPanel>(UITowerPanel.Key);
        if (null != towerPanel)
        {
            uiManager.Push(towerPanel);
        }
    }
}

