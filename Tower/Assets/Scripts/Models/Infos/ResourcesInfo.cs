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
        Sprite = 1,
        Crystal,
        Light,
        Max,
    }

    /// <summary>
    /// 精灵的个数
    /// </summary>
    public int SpriteCount
    {
        get { return spriteCount; }
        set { spriteCount = value; }
    }
    protected int spriteCount;

    /// <summary>
    /// 水晶的个数
    /// </summary>
    public int CrystalCount
    {
        get { return crystalCount; }
        set { crystalCount = value; }
    }
    protected int crystalCount;

    /// <summary>
    /// 光的数量
    /// </summary>
    public int LightCount
    {
        get { return lightCount; }
        set { lightCount = value; }
    }
    public int lightCount;

    /// <summary>
    /// 通过资源枚举 获取对应的资源数量
    /// </summary>
    /// <param name="type">你想要得到的资源的枚举类型</param>
    /// <returns>资源的数量，必须大于等于0，如果小于0的话 说明找不到这个资源</returns>
    public int GetResourceCount(ResourceTypeEnum type)
    {
        switch (type)
        {
            case ResourceTypeEnum.Sprite:
                return SpriteCount;
            case ResourceTypeEnum.Crystal:
                return CrystalCount;
            case ResourceTypeEnum.Light:
                return LightCount;
        }

        return -1;
    }
}

