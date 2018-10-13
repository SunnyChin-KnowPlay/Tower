using DreamEngine.Table;
/// <summary>
/// 战斗仆从信息
/// </summary>
public class BattleMinionInfo : BattleUnitInfo
{
    /// <summary>
    /// 配表行
    /// </summary>
    public MinionRow Row
    {
        get
        {
            if (this.RowId < 1)
                return null;

            return MinionTable.Instance[RowId];
        }
    }
}

