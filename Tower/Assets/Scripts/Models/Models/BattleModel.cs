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
    /// 所有单位的词典
    /// </summary>
    private Dictionary<int, BattleUnitInfo> allUnits = null;

    public Dictionary<int, BattleUnitInfo> AllUnits
    {
        get { return allUnits; }
    }

    /// <summary>
    /// 当前回合需要执行的行动队列
    /// </summary>
    private List<ActionInfo> currentActions = null;

    public BattleModel()
    {
        allUnits = new Dictionary<int, BattleUnitInfo>();
        currentActions = new List<ActionInfo>();
    }

    public static BattleModel Instance
    {
        get { return ModelManager.Instance.GetModel<BattleModel>(Key); }
    }

    #region Get
    public Dictionary<int, BattleUnitInfo>.ValueCollection GetAllUnits()
    {
        return allUnits.Values;
    }

    /// <summary>
    /// 通过枚举位来获取对应的角色数组
    /// </summary>
    /// <param name="relation">你想要的数组</param>
    /// <returns></returns>
    public BattleUnitInfo[] GetUnits(BattleUnitRelationEnum relation)
    {
        List<BattleUnitInfo> list = new List<BattleUnitInfo>();
        foreach (var p in GetAllUnits())
        {
            if ((p.Relation & relation) == relation)
                list.Add(p);
        }

        return list.ToArray();
    }

    /// <summary>
    /// 获取用户自己的角色信息
    /// </summary>
    /// <returns></returns>
    public BattlePlayerInfo GetSelfPlayer()
    {
        var list = this.GetUnits(BattleUnitRelationEnum.Self);
        if (null != list && list.Length > 0)
            return list[0] as BattlePlayerInfo;
        return null;
    }

    /// <summary>
    /// 获取友方人物角色
    /// </summary>
    /// <returns></returns>
    public BattlePlayerInfo[] GetFriendlyPlayers()
    {
        return (BattlePlayerInfo[])GetUnits(BattleUnitRelationEnum.Friend);
    }

    /// <summary>
    /// 获取友方和自己人物角色
    /// </summary>
    /// <returns></returns>
    public BattlePlayerInfo[] GetFriendlyAndSelfPlayers()
    {
        return (BattlePlayerInfo[])GetUnits(BattleUnitRelationEnum.Self | BattleUnitRelationEnum.Friend);
    }

    /// <summary>
    /// 获取对方人物角色
    /// </summary>
    /// <returns></returns>
    public BattlePlayerInfo[] GetOpponentPlayers()
    {
        return (BattlePlayerInfo[])GetUnits(BattleUnitRelationEnum.Opponent);
    }

    /// <summary>
    /// 获取所有角色
    /// </summary>
    /// <returns></returns>
    public BattlePlayerInfo[] GetAllPlayers()
    {
        return (BattlePlayerInfo[])GetUnits(BattleUnitRelationEnum.Self | BattleUnitRelationEnum.Friend | BattleUnitRelationEnum.Opponent);
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
        allUnits.Clear();

    }
}
