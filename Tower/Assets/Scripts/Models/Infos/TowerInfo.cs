using System;
using System.Xml.Serialization;

/// <summary>
/// 塔的信息
/// 塔的驱动力是精灵
/// 自身产出最后也是精灵
/// </summary>
[Serializable]
public class TowerInfo : BuildInfo
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

