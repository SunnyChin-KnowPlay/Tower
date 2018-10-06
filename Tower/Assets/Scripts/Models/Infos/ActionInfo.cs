using System.Xml.Serialization;

public class ActionInfo : BaseInfo
{
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

    /// <summary>
    /// 主目标的单位类型
    /// </summary>
    [XmlElement("MainTargetUnitType")]
    public ActionUnitTypeEnum MainTargetUnitType
    {
        get { return mainTargetUnitType; }
        set { mainTargetUnitType = value; }
    }
    protected ActionUnitTypeEnum mainTargetUnitType = ActionUnitTypeEnum.None;

    /// <summary>
    /// 主目标位置
    /// </summary>
    [XmlElement("MainTargetIndex")]
    public int MainTargetIndex
    {
        get { return mainTargetIndex; }
        set { mainTargetIndex = value; }
    }
    protected int mainTargetIndex = 0;


}

