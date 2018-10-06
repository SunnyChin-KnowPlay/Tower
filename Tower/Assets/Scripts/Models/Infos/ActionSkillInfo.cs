using System.Xml.Serialization;

public class ActionSkillInfo : ActionInfo
{
    /// <summary>
    /// 技能的索引位
    /// </summary>
    [XmlElement("SkillIndex")]
    public int SkillIndex
    {
        get { return skillIndex; }
        set { skillIndex = value; }
    }
    protected int skillIndex = 0;
}

