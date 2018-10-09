using System;

/// <summary>
/// 行动效果信息
/// </summary>
[Serializable]
public class ActionEffectInfo : BaseInfo
{
    /// <summary>
    /// 行动目标类型
    /// </summary>
    public ActionUnitTypeEnum ActionTargetType
    {
        get { return actionTargetType; }
        set { actionTargetType = value; }
    }
    protected ActionUnitTypeEnum actionTargetType = ActionUnitTypeEnum.None;

    /// <summary>
    /// 目标位置
    /// </summary>
    public int TargetIndex
    {
        get { return targetIndex; }
        set { targetIndex = value; }
    }
    protected int targetIndex = 0;

    /// <summary>
    /// 血量改变值 增量值 如果是加血则是正值 如果是扣血则是负值 0的话说明并没有任何血量改变，仅仅是上下buff
    /// </summary>
    public int HpChanged
    {
        get { return hpChanged; }
        set { hpChanged = value; }
    }
    protected int hpChanged = 0;

    /// <summary>
    /// 获取Buff的信息
    /// </summary>
    public BuffInfo BuffInfo
    {
        get { return buffInfo; }
        set { buffInfo = value; }
    }
    protected BuffInfo buffInfo = null;

    public ActionEffectInfo()
    {

    }
}

