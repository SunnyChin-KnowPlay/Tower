using DreamEngine.Table;
using System.Xml.Serialization;

public class BuffInfo : BaseInfo
{
    /// <summary>
    /// Buff类型枚举
    /// </summary>
    public enum BuffTypeEnum
    {
        Buff = 1,
        Debuff = 2,
    }

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

    /// <summary>
    /// 距离上一次改变的次数
    /// </summary>
    public int ChangedCount
    {
        get { return changedCount; }
        set { changedCount = value; }
    }
    protected int changedCount = 0;

    /// <summary>
    /// 叠加的次数 归0时则移除
    /// </summary>
    public int Count
    {
        get { return count; }
        set { count = value; }
    }
    protected int count = 0;

    /// <summary>
    /// Buff类型
    /// </summary>
    public BuffTypeEnum BuffType
    {
        get; set;
    }

}
