using System;
using System.Collections.Generic;

public class UserModel : BaseModel<UserModel>
{
    public const string Key = "User";

    /// <summary>
    /// 获取所有的用户信息，迭代器，只能用于读取
    /// </summary>
    public IEnumerator<KeyValuePair<Guid, UserInfo>> Users
    { get { return users.GetEnumerator(); } }

    /// <summary>
    /// 存放所有用户的词典，所有的用户信息都在里面
    /// </summary>
    private Dictionary<Guid, UserInfo> users = null;

    private UserInfo selfUserInfo = null;

    /// <summary>
    /// 玩家自己的用户信息
    /// </summary>
    public UserInfo Self
    {
        get
        {
            return selfUserInfo;
        }
    }

    public static UserModel Instance
    {
        get { return ModelManager.Instance.GetModel<UserModel>(Key); }
    }

    public UserModel()
    {
        users = new Dictionary<Guid, UserInfo>();

        UserInfo userInfo = new UserInfo();
        userInfo.Uuid = Guid.NewGuid();
        userInfo.Name = "陈斌";
        userInfo.ResourcesInfo.SpriteCount = 999;
        userInfo.ResourcesInfo.LightCount = 10;
        userInfo.ResourcesInfo.CrystalCount = 1000;

        selfUserInfo = userInfo;
        users.Add(userInfo.Uuid, userInfo);
    }

    public override string GetKey()
    {
        return Key;
    }

    public override void Reset()
    {

    }

    #region Find
    /// <summary>
    /// 通过用户唯一ID来获取对应的用户信息，如果数据里面有这个用户则返回信息，没有则返回null
    /// </summary>
    /// <param name="uuid">你想要得到的用户的唯一ID</param>
    /// <returns>你想要的用户信息</returns>
    public UserInfo GetUserWithUUID(Guid uuid)
    {
        if (users.ContainsKey(uuid))
            return users[uuid];
        return null;
    }
    #endregion
}
