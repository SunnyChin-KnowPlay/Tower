using System;
using System.Xml.Serialization;

/// <summary>
/// Buff所触发的行动信息
/// </summary>
[Serializable]
public class ActionBuffInfo : ActionInfo
{
    /// <summary>
    /// Buff的索引位
    /// </summary>
    public int BuffIndex
    {
        get { return buffIndex; }
        set { buffIndex = value; }
    }
    protected int buffIndex = 0;

    /// <summary>
    /// 触发时机
    /// </summary>
    public BuffTriggerMomentEnum TriggerMoment
    {
        get { return triggerMoment; }
        set { triggerMoment = value; }
    }
    protected BuffTriggerMomentEnum triggerMoment;

}

