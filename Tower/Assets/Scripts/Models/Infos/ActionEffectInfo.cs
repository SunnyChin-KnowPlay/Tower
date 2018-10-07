using System.Collections.Generic;

public class ActionEffectInfo : BaseInfo
{
    /// <summary>
    /// 行动的信息
    /// </summary>
    public ActionInfo ActionInfo
    {
        get { return actionInfo; }
        set { actionInfo = value; }
    }
    protected ActionInfo actionInfo = null;

    /// <summary>
    /// 行动目标类型
    /// </summary>
    public ActionUnitTypeEnum ActionTargetType
    {
        get { return actionTargetType; }
        set { actionTargetType = value; }
    }
    protected ActionUnitTypeEnum actionTargetType = ActionUnitTypeEnum.None;

    /// <summary>
    /// 目标位置
    /// </summary>
    public int TargetIndex
    {
        get { return targetIndex; }
        set { targetIndex = value; }
    }
    protected int targetIndex = 0;

    /// <summary>
    /// 伤害值
    /// </summary>
    public int HurtValue
    {
        get { return hurtValue; }
        set { hurtValue = value; }
    }
    protected int hurtValue = 0;

    /// <summary>
    /// 获取Buff的改变列表
    /// </summary>
    public BuffInfo[] BuffChangeds
    {
        get
        {
            if (null == buffChangeds)
                return null;
            return buffChangeds.ToArray();
        }
        set
        {
            if (null == buffChangeds)
                return;

            buffChangeds.Clear();
            buffChangeds.AddRange(value);
        }
    }
    protected List<BuffInfo> buffChangeds = null;

    /// <summary>
    /// 放入Buff的改变队列
    /// </summary>
    /// <param name="list"></param>
    public void PutBuffChangeds(BuffInfo[] list)
    {
        if (null == buffChangeds)
            return;

        buffChangeds.Clear();
        buffChangeds.AddRange(list);
    }

    public ActionEffectInfo()
    {
        buffChangeds = new List<BuffInfo>();
    }
}

