using DreamEngine.Table;
using System.Xml.Serialization;

public class BuffInfo : BaseInfo
{

    /// <summary>
    /// 索引位
    /// </summary>
    [XmlElement("Index")]
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    private int index = 0;

    /// <summary>
    /// 配表ID
    /// </summary>
    [XmlElement("ConfId")]
    public int ConfId
    {
        get { return confId; }
        set { confId = value; }
    }
    private int confId = 0;

    /// <summary>
    /// 配表行信息
    /// </summary>
    public BuffRow Row
    {
        get
        {
            if (confId > 0)
                return BuffTable.Instance[confId];
            return null;
        }
    }
}
