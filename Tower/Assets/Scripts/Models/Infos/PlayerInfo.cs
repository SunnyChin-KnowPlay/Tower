using System;
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
        Opponent = 3,
    }

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
}
