using System;
using System.Collections.Generic;
using System.Xml.Serialization;

public class PlayerInfo : BaseInfo
{
    /// <summary>
    /// 角色唯一ID
    /// </summary>
    public Guid Uid
    {
        get { return uid; }
        set { uid = value; }
    }
    protected Guid uid = Guid.Empty;


}
