using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗模块
/// </summary>
public class BattleModel : BaseModel
{
    public const string Key = "Battle";

    /// <summary>
    /// 所有角色的词典
    /// </summary>
    private Dictionary<int, PlayerInfo> allPlayers = null;

    /// <summary>
    /// 当前回合需要执行的行动队列
    /// </summary>
    private List<ActionInfo> currentActions = null;

    public BattleModel()
    {
        allPlayers = new Dictionary<int, PlayerInfo>();
        currentActions = new List<ActionInfo>();
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

    /// <summary>
    /// 通过枚举位来获取对应的角色数组
    /// </summary>
    /// <param name="relation">你想要的数组</param>
    /// <returns></returns>
    public PlayerInfo[] GetPlayers(PlayerInfo.RelationEnum relation)
    {
        List<PlayerInfo> list = new List<PlayerInfo>();
        foreach (var p in GetAllPlayers())
        {
            if ((p.Relation & relation) == relation)
                list.Add(p);
        }

        return list.ToArray();
    }

    public PlayerInfo GetSelfPlayer()
    {
        var list = this.GetPlayers(PlayerInfo.RelationEnum.Self);
        if (null != list && list.Length > 0)
            return list[0];
        return null;
    }

    public PlayerInfo[] GetFriendlyPlayers()
    {
        return GetPlayers(PlayerInfo.RelationEnum.Friendly);
    }

    public PlayerInfo[] GetFriendlyAndSelfPlayers()
    {
        return GetPlayers(PlayerInfo.RelationEnum.Self | PlayerInfo.RelationEnum.Friendly);
    }

    public PlayerInfo[] GetOpponentPlayers()
    {
        return GetPlayers(PlayerInfo.RelationEnum.Opponent);
    }
    #endregion

    #region Action
    /// <summary>
    /// 当前回合内是否还有剩余的需要执行的行动
    /// </summary>
    /// <returns></returns>
    public bool HasRemainderActionsInCurrentRound()
    {
        return currentActions != null && currentActions.Count > 0;
    }

    /// <summary>
    /// 提取最新的一条行动
    /// </summary>
    /// <returns></returns>
    public ActionInfo ExtractAction()
    {
        if (!HasRemainderActionsInCurrentRound())
            return null;

        var actionInfo = currentActions[0];
        currentActions.RemoveAt(0);

        return actionInfo;
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
