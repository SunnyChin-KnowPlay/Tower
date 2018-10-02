using System;
using System.Collections.Generic;
using System.Xml.Serialization;

public class PlayerInfo : BaseInfo
{
    /// <summary>
    /// 角色关系枚举
    /// </summary>
    public enum RelationEnum : int
    {
        None = 0,
        /// <summary>
        /// 自己
        /// </summary>
        Self = 1,
        /// <summary>
        /// 友方
        /// </summary>
        Friendly = 2,
        /// <summary>
        /// 对方
        /// </summary>
        Opponent = 4,
    }

    /// <summary>
    /// 索引位 0-9
    /// </summary>
    [XmlElement("Index")]
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    protected int index = 0;

    /// <summary>
    /// 战场上的位置 0-9
    /// </summary>
    [XmlElement("Position")]
    public int Position
    {
        get { return position; }
        set { position = value; }
    }
    protected int position = 0;

    /// <summary>
    /// 角色唯一ID
    /// </summary>
    [XmlElement("Uid")]
    public Guid Uid
    {
        get { return uid; }
        set { uid = value; }
    }
    protected Guid uid = Guid.Empty;

    /// <summary>
    /// 角色的名字
    /// </summary>
    [XmlElement("Name")]
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    protected string name = null;

    /// <summary>
    /// 角色的血量
    /// </summary>
    [XmlElement("Hp")]
    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }
    protected int hp = 0;

    /// <summary>
    /// 与玩家自己的关系
    /// </summary>
    [XmlElement("Relation")]
    public RelationEnum Relation
    {
        get { return relation; }
        set { relation = value; }
    }
    protected RelationEnum relation = RelationEnum.None;

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
    public BuffInfo[] Buffs
    {
        get { return buffs.ToArray(); }
        set
        {
            buffs.Clear();
            buffs.AddRange(value);
        }
    }
    protected List<BuffInfo> buffs = null;
}
