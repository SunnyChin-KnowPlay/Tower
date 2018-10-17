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
    FrontOrBack = 1 << 10,
    FrontOrBackLeft1 = 1 << 11,
    FrontOrBackLeft2 = 1 << 12,
    FrontOrBackLeft3 = 1 << 13,
    FrontOrBackLeft4 = 1 << 14,
    FrontOrBackRight1 = 1 << 15,
    FrontOrBackRight2 = 1 << 16,
    FrontOrBackRight3 = 1 << 17,
    FrontOrBackRight4 = 1 << 18,
}

public enum BattleGridPositionEnum : uint
{
    None = 0,
    /// <summary>
    /// 我方基地，放水晶用的
    /// </summary>
    Home_Base = 1,
    /// <summary>
    /// 我方角色
    /// </summary>
    Home_Player_Start = 2,
    Home_1 = 2,
    Home_2 = 3,
    Home_3 = 4,
    Home_4 = 5,
    Home_5 = 6,
    Home_6 = 7,
    Home_7 = 8,
    Home_8 = 9,
    Home_9 = 10,
    Home_10 = 11,
    Home_Player_Ended = 11,

    /// <summary>
    /// 对方基地
    /// </summary>
    Away_Base = 12,
    /// <summary>
    /// 对方角色
    /// </summary>
    Away_Player_Start = 13,
    Away_1 = 13,
    Away_2 = 14,
    Away_3 = 15,
    Away_4 = 16,
    Away_5 = 17,
    Away_6 = 18,
    Away_7 = 19,
    Away_8 = 20,
    Away_9 = 21,
    Away_10 = 22,
    Away_Player_Ended = 22,

}

/// <summary>
/// 战斗单位关系枚举
/// </summary>
public enum BattleUnitRelationEnum : int
{
    None = 0,
    /// <summary>
    /// 自己
    /// </summary>
    Self = 1,
    /// <summary>
    /// 角色自己的仆从
    /// </summary>
    SelfMinion = 2,
    /// <summary>
    /// 友方角色
    /// </summary>
    Friend = 4,
    /// <summary>
    /// 友方仆从
    /// </summary>
    FriendMinion = 8,
    /// <summary>
    /// 对方
    /// </summary>
    Opponent = 16,
    /// <summary>
    /// 对方仆从
    /// </summary>
    OpponentMinion = 32,
}

/// <summary>
/// 战场单位类型枚举
/// </summary>
public enum BattleUnitTypeEnum : int
{
    None = 0,
    /// <summary>
    /// 人物角色
    /// </summary>
    Player = 1,
    /// <summary>
    /// 仆从
    /// </summary>
    Minion = 2,
    /// <summary>
    /// 基地水晶
    /// </summary>
    Diamond = 4,
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
    HpChanged = 1,
    /// <summary>
    /// 叠加一层Buff，参数1为BuffID，参数2为目标位枚举@ActionTargetEnum 相对于Buff的所有人
    /// </summary>
    AddBuff = 2,
    /// <summary>
    /// 移除一层Buff，参数1为BuffID，参数2为目标位枚举@ActionTargetEnum 相对于Buff的所有人
    /// </summary>
    RemoveBuff = 3,
    /// <summary>
    /// 清除所有层数的Buff，参数1为BuffID，参数2为目标位枚举@ActionTargetEnum 相对于Buff的所有人
    /// </summary>
    ClearBuff = 4,
    /// <summary>
    /// 清除所有层数的所有BUFF，参数1无效，参数2位目标位枚举@ActionTargetEnum 相对于Buff的所有人
    /// </summary>
    ClearAllBuff = 5,
}

