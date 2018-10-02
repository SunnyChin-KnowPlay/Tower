using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗模块
/// </summary>
public class BattleModel : BaseModel
{
    public const string Key = "Battle";

    private Dictionary<int, PlayerInfo> allPlayers = null;

    public BattleModel()
    {
        allPlayers = new Dictionary<int, PlayerInfo>();

        var s1 = GetAllPlayers();
    }

    public static BattleModel Instance
    {
        get { return ModelManager.Instance.GetModel<BattleModel>(Key); }
    }

    #region Get
    public Dictionary<int, PlayerInfo>.ValueCollection GetAllPlayers()
    {
        return allPlayers.Values;
    }
    #endregion

    public override string GetKey()
    {
        return Key;
    }

    public override void Reset()
    {
        allPlayers.Clear();

    }
}
