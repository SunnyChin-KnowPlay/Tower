using System;
using System.Xml.Serialization;

/// <summary>
/// 技能所触发的行动信息
/// </summary>
[Serializable]
public class ActionSkillInfo : ActionInfo
{
    /// <summary>
    /// 技能的索引位
    /// </summary>
    public int SkillIndex
    {
        get { return skillIndex; }
        set { skillIndex = value; }
    }
    protected int skillIndex = 0;

    /// <summary>
    /// 主目标的单位类型
    /// </summary>
    public ActionUnitTypeEnum MainTargetUnitType
    {
        get { return mainTargetUnitType; }
        set { mainTargetUnitType = value; }
    }
    protected ActionUnitTypeEnum mainTargetUnitType = ActionUnitTypeEnum.None;

    /// <summary>
    /// 主目标位置
    /// </summary>
    public int MainTargetIndex
    {
        get { return mainTargetIndex; }
        set { mainTargetIndex = value; }
    }
    protected int mainTargetIndex = 0;
}

