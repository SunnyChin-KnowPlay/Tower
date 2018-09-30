using System;
/// <summary>
/// 基础资源信息
/// </summary>
[Serializable]
public class ResourcesInfo : BaseInfo
{
    /// <summary>
    /// 资源类型枚举
    /// </summary>
    public enum ResourceTypeEnum
    {
        Min = 1,
        Gold = 1,
        Diamond,
        Max,
    }

    private int gold = 0;
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }

    private int diamond = 0;
    public int Diamond
    {
        get { return diamond; }
        set { diamond = value; }
    }

    /// <summary>
    /// 通过资源枚举 获取对应的资源数量
    /// </summary>
    /// <param name="type">你想要得到的资源的枚举类型</param>
    /// <returns>资源的数量，必须大于等于0，如果小于0的话 说明找不到这个资源</returns>
    public int GetResourceCount(ResourceTypeEnum type)
    {
        switch (type)
        {
            case ResourceTypeEnum.Gold:
                return Gold;
            case ResourceTypeEnum.Diamond:
                return Diamond;
        }

        return -1;
    }
}

