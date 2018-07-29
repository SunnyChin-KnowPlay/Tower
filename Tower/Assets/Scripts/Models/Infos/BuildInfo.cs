using System.Xml.Serialization;
/// <summary>
/// 建筑的信息 所有建筑的基类 建筑包括
/// </summary>
public class BuildInfo : BaseInfo
{
    /// <summary>
    /// 唯一ID，每个建筑都有一个标识号码
    /// </summary>
    [XmlElement("Uid")]
    public uint Uid
    {
        get { return uid; }
        set { uid = value; }
    }
    protected uint uid;

    /// <summary>
    /// 位置号，说明这个建筑是在几号位
    /// </summary>
    [XmlElement("Position")]
    public int Position
    {
        get; set;
    }

    /// <summary>
    /// 建筑的等级
    /// </summary>
    [XmlElement("Level")]
    public int Level
    {
        get; set;
    }

    /// <summary>
    /// 建筑的种类
    /// </summary>
    [XmlElement("Kind")]
    public int Kind
    {
        get; set;
    }

    /// <summary>
    /// 精灵的数量
    /// </summary>
    [XmlElement("Count")]
    public int Count
    {
        get; set;
    }
}

