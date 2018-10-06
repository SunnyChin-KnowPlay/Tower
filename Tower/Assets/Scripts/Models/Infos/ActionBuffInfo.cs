using System.Xml.Serialization;

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

