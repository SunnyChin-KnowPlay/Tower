using DreamEngine.Utilities;

public abstract class BaseModel : IModel, IResetable
{
    /// <summary>
    /// 获取本数据模块的键
    /// </summary>
    /// <returns></returns>
    public abstract string GetKey();

    /// <summary>
    /// 重置所有数据
    /// </summary>
    public abstract void Reset();
}

