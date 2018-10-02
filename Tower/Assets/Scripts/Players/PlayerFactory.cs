using Dream.Assets;
using Dream.Extension.Unity;
using Dream.Utilities;
using UnityEngine;

public class PlayerFactory : Singleton<PlayerFactory>
{
    /// <summary>
    /// 加载角色预制件
    /// </summary>
    /// <param name="info">职业信息</param>
    /// <returns>需加载的角色的控制器</returns>
    public PlayerControl LoadPlayer(PlayerInfo info)
    {
        string assetPath = GetPlayerPath(info);
        if (string.IsNullOrEmpty(assetPath))
            return null;

        var obj = AssetManager.LoadAssetSync<GameObject>(assetPath);
        if (null == obj)
            return null;

        var go = GameObject.Instantiate<GameObject>(obj);
        if (null == go)
            return null;

        var control = go.GetComponentOrAdd<PlayerControl>();
        if (null == control)
            return null;

        control.Setup(info);
        return control;
    }

    /// <summary>
    /// 通过角色信息获取对应的资源路径
    /// </summary>
    /// <param name="info">角色信息</param>
    /// <returns>你想要的角色预制件的资源路径</returns>
    private string GetPlayerPath(PlayerInfo info)
    {
        return "players/prefabs/Player";
    }
}

