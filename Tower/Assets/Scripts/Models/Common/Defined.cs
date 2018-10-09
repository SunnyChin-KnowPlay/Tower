/// <summary>
/// 行动目标类型枚举
/// </summary>
public enum ActionUnitTypeEnum
{
    None = 0,
    /// <summary>
    /// 地面 格子
    /// </summary>
    Ground = 1,
    /// <summary>
    /// 角色
    /// </summary>
    Player = 2,
}

/// <summary>
/// 行动类型枚举
/// </summary>
public enum ActionTypeEnum
{
    None = 0,
    /// <summary>
    /// 技能
    /// </summary>
    Skill = 1,
    /// <summary>
    /// Buff
    /// </summary>
    Buff = 2,
    /// <summary>
    /// 比赛
    /// </summary>
    Battle = 4,
}

/// <summary>
/// 行动目标相对关系枚举
/// </summary>
public enum ActionTargetEnum : uint
{
    None = 0,
    Self = 1 << 1,
    Left1 = 1 << 2,
    Left2 = 1 << 3,
    Left3 = 1 << 4,
    Left4 = 1 << 5,
    Right1 = 1 << 6,
    Right2 = 1 << 7,
    Right3 = 1 << 8,
    Right4 = 1 << 9,
    Away = 1 << 10,
    AwayLeft1 = 1 << 11,
    AwayLeft2 = 1 << 12,
    AwayLeft3 = 1 << 13,
    AwayLeft4 = 1 << 14,
    AwayRight1 = 1 << 15,
    AwayRight2 = 1 << 16,
    AwayRight3 = 1 << 17,
    AwayRight4 = 1 << 18,
}

/// <summary>
/// Buff触发器时机枚举
/// </summary>
public enum BuffTriggerMomentEnum : uint
{
    None = 0,
    /// <summary>
    /// 回合开始
    /// </summary>
    RoundStart = 1 << 1,
    /// <summary>
    /// 回合结束
    /// </summary>
    RoundEnded = 1 << 2,
}

/// <summary>
/// 触发器行动类型枚举
/// </summary>
public enum TriggerActionTypeEnum : uint
{
    None = 0,
    /// <summary>
    /// 血量变化 正值为加血 负值为减血
    /// </summary>
    HpChanged = 1 << 1,
    /// <summary>
    /// 叠加一层Buff，参数1为BuffID，参数2为目标位枚举@ActionTargetEnum 相对于Buff的所有人
    /// </summary>
    AddBuff = 1 << 2,
    /// <summary>
    /// 移除一层Buff，参数1为BuffID，参数2为目标位枚举@ActionTargetEnum 相对于Buff的所有人
    /// </summary>
    RemoveBuff = 1 << 3,
    /// <summary>
    /// 清除所有层数的Buff，参数1为BuffID，参数2为目标位枚举@ActionTargetEnum 相对于Buff的所有人
    /// </summary>
    ClearBuff = 1 << 4,
}

