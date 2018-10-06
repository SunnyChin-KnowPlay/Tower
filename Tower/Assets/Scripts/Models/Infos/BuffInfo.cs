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
    /// 剩余的回合数 当回合数为0时 则代表buff失效
    /// </summary>
    public int RemainingRounds
    {
        get { return remainingRounds; }
        set { remainingRounds = value; }
    }
    protected int remainingRounds = 0;

    /// <summary>
    /// Buff类型
    /// </summary>
    public BuffTypeEnum BuffType
    {
        get; set;
    }

}
