using DreamEngine.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 精灵界面的条目控制脚本
/// </summary>
public class UISpriteItem : MonoBehaviour
{
    /// <summary>
    /// 背景图
    /// </summary>
    private Image backgroundImage;

    /// <summary>
    /// 图标
    /// </summary>
    private Image iconImage;

    /// <summary>
    /// 精灵的名称
    /// </summary>
    private Text nameText;

    /// <summary>
    /// 个数的标签
    /// </summary>
    private Text countText;

    /// <summary>
    /// 等级的文本
    /// </summary>
    private Text levelText;

    /// <summary>
    /// 升级进度条
    /// </summary>
    private Image progressImage;

    /// <summary>
    /// 创建下一级精灵的文本
    /// </summary>
    private Text createDescText;

    public SpriteInfo SpriteInfo
    {
        get { return spriteInfo; }
        set
        {
            spriteInfo = value;
        }
    }
    private SpriteInfo spriteInfo;

    private void Awake()
    {
        backgroundImage = this.transform.Find("Background").GetComponent<Image>();
        iconImage = this.transform.Find("Icon").GetComponent<Image>();
        nameText = this.transform.Find("Name").GetComponent<Text>();
        levelText = this.transform.Find("Level").GetComponent<Text>();
        countText = this.transform.Find("Count").GetComponent<Text>();
        progressImage = this.transform.Find("Progress Bar/Background Mask/Loading Bar").GetComponent<Image>();
        createDescText = this.transform.Find("Create Desc").GetComponent<Text>();
    }

    private void OnEnable()
    {
        InvokeRepeating("UpdateFunc", 0, 1);
    }

    private void OnDisable()
    {
        CancelInvoke("UpdateFunc");

        
    }

    private void UpdateFunc()
    {
        if (null != this.spriteInfo)
        {
            Setup();
        }
    }

    // Use this for initialization
    void Start()
    {

    }


    private void Update()
    {


    }


    #region Setup
    private void Setup()
    {
        nameText.text = spriteInfo.Row.Name;
        iconImage.sprite = Resources.Load<Sprite>(spriteInfo.Row.Icon);
        countText.text = spriteInfo.Count.ToString();
        levelText.text = string.Format("Lv.{0}", spriteInfo.Level.ToString());
        if (null != spriteInfo.Row.CreateTarget)
        {
            createDescText.text = string.Format("生产{0} {1}/s", spriteInfo.Row.CreateTarget.Name, spriteInfo.CreateCount);
        }
    }
    #endregion
}
