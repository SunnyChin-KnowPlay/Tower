using System;
using System.Xml.Serialization;

[Serializable]
public class UserInfo : BaseInfo
{
    /// <summary>
    /// 用户唯一ID
    /// </summary>
    public Guid Uuid
    {
        get { return uuid; }
        set { uuid = value; }
    }
    protected Guid uuid;

    /// <summary>
    /// 用户的名字
    /// </summary>
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    protected string name;

    /// <summary>
    /// 基础资源信息
    /// </summary>
    public ResourcesInfo ResourcesInfo
    {
        get { return resourcesInfo; }
    }
    protected ResourcesInfo resourcesInfo;

    public UserInfo()
    {
        resourcesInfo = new ResourcesInfo();
    }
}

