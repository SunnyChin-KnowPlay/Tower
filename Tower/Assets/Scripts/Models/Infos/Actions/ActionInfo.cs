using System.Xml.Serialization;

public class ActionInfo : BaseInfo
{
    /// <summary>
    /// 行动类型
    /// </summary>
    [XmlElement("ActionType")]
    public ActionTypeEnum ActionType
    {
        get { return actionType; }
        set { actionType = value; }
    }
    protected ActionTypeEnum actionType;

    /// <summary>
    /// 执行顺位
    /// </summary>
    [XmlElement("Index")]
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    protected int index = 0;

    /// <summary>
    /// 发起者的单位类型
    /// </summary>
    [XmlElement("LaunchUnitType")]
    public ActionUnitTypeEnum LaunchUnitType
    {
        get { return launchUnitType; }
        set { launchUnitType = value; }
    }
    protected ActionUnitTypeEnum launchUnitType = ActionUnitTypeEnum.None;

    /// <summary>
    /// 发起者索引位
    /// </summary>
    [XmlElement("LaunchIndex")]
    public int LaunchIndex
    {
        get { return launchIndex; }
        set { launchIndex = value; }
    }
    protected int launchIndex = 0;

    


}

