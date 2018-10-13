using DreamEngine.Table;
/// <summary>
/// 战斗角色信息
/// </summary>
public class BattlePlayerInfo : BattleUnitInfo
{
    /// <summary>
    /// 配表行信息
    /// </summary>
    public PlayerRow Row
    {
        get
        {
            if (this.RowId > 0)
            {
                PlayerTable pt = PlayerTable.Instance;
                return pt[RowId];
            }

            return null;
        }
    }
}

