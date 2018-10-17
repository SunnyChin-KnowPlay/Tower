using Dream.Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillButton : UITitleButton
{
    /// <summary>
    /// 技能信息
    /// </summary>
    public SkillInfo SkillInfo
    {
        get { return skillInfo; }
    }
    private SkillInfo skillInfo = null;

    /// <summary>
    /// 所需消耗的能量
    /// </summary>
    public int NeedEnergy
    {
        get
        {
            if (null == skillInfo)
                return 0;

            return skillInfo.Row.Energy;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    public override void Setup(Sprite iconSprite, string title)
    {
        base.Setup(iconSprite, title);
    }

    public void Setup(SkillInfo info)
    {
        if (null == info)
            return;

        var name = info.Row.Name;
        var spriteName = info.Row.Icon;

        this.skillInfo = info;

        var sprite = AssetManager.LoadResource<Sprite>(spriteName);
        base.Setup(sprite, name);
    }
}
