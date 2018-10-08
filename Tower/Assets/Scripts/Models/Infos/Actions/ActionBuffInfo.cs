using System.Xml.Serialization;

/// <summary>
/// Buff所触发的行动信息
/// </summary>
public class ActionBuffInfo : ActionInfo
{
    /// <summary>
    /// Buff的索引位
    /// </summary>
    [XmlElement("BuffIndex")]
    public int BuffIndex
    {
        get { return buffIndex; }
        set { buffIndex = value; }
    }
    protected int buffIndex = 0;

    


}

