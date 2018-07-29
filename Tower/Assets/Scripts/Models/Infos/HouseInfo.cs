using System.Xml.Serialization;

/// <summary>
/// 房子的信息 功能性的房子 用于生产水晶
/// </summary>
public class HouseInfo : BuildInfo
{
    /// <summary>
    /// 制作等级 就是合成等级
    /// </summary>
    [XmlElement("MakeLevel")]
    public int MakeLevel
    {
        get { return makeLevel; }
        set { makeLevel = value; }
    }

    protected int makeLevel;
}

