using DreamEngine.Table;
using System.Xml.Serialization;

public class SkillInfo : BaseInfo
{
    /// <summary>
    /// 索引位
    /// </summary>
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    private int index = 0;

    /// <summary>
    /// 配表ID
    /// </summary>
    public int ConfId
    {
        get { return confId; }
        set { confId = value; }
    }
    private int confId = 0;

    /// <summary>
    /// 配表行信息
    /// </summary>
    public SkillRow Row
    {
        get
        {
            if (confId > 0)
                return SkillTable.Instance[confId];
            return null;
        }
    }
}
