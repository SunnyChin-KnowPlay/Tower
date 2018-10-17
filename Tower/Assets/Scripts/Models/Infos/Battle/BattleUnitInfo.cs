using System;
using System.Collections.Generic;

/// <summary>
/// 战斗单位信息 包括了人物角色，仆从，还有水晶等其他东西
/// </summary>
public class BattleUnitInfo : BaseInfo
{
    /// <summary>
    /// 索引位
    /// </summary>
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    protected int index = 0;

    /// <summary>
    /// 单位类型
    /// </summary>
    public BattleUnitTypeEnum UnitType
    {
        get { return unitType; }
        set { unitType = value; }
    }
    protected BattleUnitTypeEnum unitType = BattleUnitTypeEnum.None;

    /// <summary>
    /// 战场上的位置
    /// </summary>
    public BattleGridPositionEnum Position
    {
        get { return position; }
        set { position = value; }
    }
    protected BattleGridPositionEnum position = BattleGridPositionEnum.None;

    /// <summary>
    /// 配表ID
    /// </summary>
    public int RowId
    {
        get { return rowId; }
        set { rowId = value; }
    }
    protected int rowId = 0;

    /// <summary>
    /// 名字
    /// </summary>
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    protected string name = null;

    /// <summary>
    /// 血量的上限
    /// </summary>
    public int HpMax
    {
        get { return hpMax; }
        set { hpMax = value; }
    }
    protected int hpMax = 0;

    /// <summary>
    /// 血量
    /// </summary>
    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }
    protected int hp = 0;

    /// <summary>
    /// 攻击力
    /// </summary>
    public int Attack
    {
        get { return attack; }
        set { attack = value; }
    }
    protected int attack = 0;

    /// <summary>
    /// 与玩家控制的角色的关系
    /// </summary>
    public BattleUnitRelationEnum Relation
    {
        get { return relation; }
        set { relation = value; }
    }
    protected BattleUnitRelationEnum relation = BattleUnitRelationEnum.None;

    /// <summary>
    /// 技能队列
    /// </summary>
    public SkillInfo[] Skills
    {
        get { return skills.ToArray(); }
        set
        {
            skills.Clear();
            skills.AddRange(value);
        }
    }
    protected List<SkillInfo> skills = null;

    /// <summary>
    /// Buff队列
    /// </summary>
    public List<BuffInfo> Buffs
    {
        get { return buffs; }
    }
    protected List<BuffInfo> buffs = null;

    public BattleUnitInfo()
    {
        skills = new List<SkillInfo>();
        buffs = new List<BuffInfo>();
    }
}
