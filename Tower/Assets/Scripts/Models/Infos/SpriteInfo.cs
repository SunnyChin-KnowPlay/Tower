using DreamEngine.Table;
using System;
using System.Xml.Serialization;

/// <summary>
/// 精灵
/// </summary>
[Serializable]
public class SpriteInfo : BaseInfo
{
    /// <summary>
    /// 唯一ID，每个精灵都有一个唯一ID
    /// </summary>
    [XmlElement("Uid")]
    public uint Uid
    {
        get { return uid; }
        set { uid = value; }
    }
    protected uint uid;

    /// <summary>
    /// 表格信息
    /// </summary>
    public SpritesRow Row
    {
        get
        {
            if (uid < 1)
                return null;

            return SpritesTable.Instance[(int)uid];
        }
    }

    /// <summary>
    /// 自身的等级
    /// </summary>
    [XmlElement("Level")]
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    protected int level;

    /// <summary>
    /// 数量
    /// </summary>
    [XmlElement("Count")]
    public UInt64 Count
    {
        get { return count; }
        set { count = value; }
    }

    protected UInt64 count;

    /// <summary>
    /// 创建数量 数量/每秒
    /// </summary>
    public UInt64 CreateCount
    {
        get
        {
            if (null == Row)
                return 0;

            return (UInt64)UnityEngine.Mathf.Pow((UInt64)Row.CreateNum * count, level);
        }
    }
}

