using System;
using System.Collections.Generic;

[Serializable]
public class ActionInfo : BaseInfo
{
    /// <summary>
    /// 行动类型
    /// </summary>
    public ActionTypeEnum ActionType
    {
        get { return actionType; }
        set { actionType = value; }
    }
    protected ActionTypeEnum actionType;

    /// <summary>
    /// 执行顺位
    /// </summary>
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    protected int index = 0;

    /// <summary>
    /// 发起者的单位类型
    /// </summary>
    public ActionUnitTypeEnum LaunchUnitType
    {
        get { return launchUnitType; }
        set { launchUnitType = value; }
    }
    protected ActionUnitTypeEnum launchUnitType = ActionUnitTypeEnum.None;

    /// <summary>
    /// 发起者索引位
    /// </summary>
    public int LaunchIndex
    {
        get { return launchIndex; }
        set { launchIndex = value; }
    }
    protected int launchIndex = 0;

    /// <summary>
    /// 效果信息
    /// </summary>
    public ActionEffectInfo[] EffectInfos
    {
        get
        {
            return effectInfos.ToArray();
        }
        set
        {
            effectInfos.Clear();
            effectInfos.AddRange(value);
        }
    }

    /// <summary>
    /// 提取并移除列表中为首的效果信息
    /// </summary>
    /// <returns></returns>
    public ActionEffectInfo ExtractEffectInfo()
    {
        if (null == effectInfos)
            return null;

        if (effectInfos.Count < 1)
            return null;

        var effectInfo = effectInfos[0];
        effectInfos.Clear();
        return effectInfo;
    }
    protected List<ActionEffectInfo> effectInfos = null;

    public ActionInfo()
    {
        effectInfos = new List<ActionEffectInfo>();
    }
}

