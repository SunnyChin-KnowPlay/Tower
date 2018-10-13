using Dream.Assets;
using Dream.Extension.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitFactory : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    /// <summary>
    /// 加载角色预制件
    /// </summary>
    /// <param name="info">职业信息</param>
    /// <returns>需加载的角色的控制器</returns>
    public BattleUnitControl LoadUnit(BattleUnitInfo info)
    {
        string assetPath = GetUnitPrefabPath(info);
        if (string.IsNullOrEmpty(assetPath))
            return null;

        var obj = AssetManager.LoadAssetSync<GameObject>(assetPath);
        if (null == obj)
            return null;

        var go = GameObject.Instantiate<GameObject>(obj);
        if (null == go)
            return null;

        var control = go.GetComponentOrAdd<BattleUnitControl>();
        if (null == control)
            return null;

        control.Setup(info);
        return control;
    }

    /// <summary>
    /// 获取单位预制件路径
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private string GetUnitPrefabPath(BattleUnitInfo info)
    {
        switch (info.UnitType)
        {
            case BattleUnitTypeEnum.Player:
                var playerInfo = info as BattlePlayerInfo;
                return GetUnitPrefabPath(playerInfo);
            case BattleUnitTypeEnum.Minion:
                var minionInfo = info as BattleMinionInfo;
                return GetUnitPrefabPath(minionInfo);
            default:
                return "";
        }
    }

    /// <summary>
    /// 通过角色信息获取对应的资源路径
    /// </summary>
    /// <param name="info">信息</param>
    /// <returns>你想要的角色预制件的资源路径</returns>
    private string GetUnitPrefabPath(BattlePlayerInfo info)
    {
        if (null == info)
            return null;

        return System.IO.Path.Combine("players/prefabs", info.Row.ModelName);
    }

    /// <summary>
    /// 通过仆从信息获取对应的预制件路径
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private string GetUnitPrefabPath(BattleMinionInfo info)
    {
        if (null == info)
            return null;

        return System.IO.Path.Combine("minions/prefabs", info.Row.ModelName);
    }
}
