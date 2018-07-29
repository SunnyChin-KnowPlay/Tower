using System;

[Serializable]
public class BaseInfo : ICloneable
{
    public virtual object Clone()
    {
        return new BaseInfo();
    }
}

