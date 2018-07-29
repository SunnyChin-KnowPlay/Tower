using System;
/// <summary>
/// 基础资源信息
/// </summary>
[Serializable]
public class ResourcesInfo : BaseInfo
{
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
}

