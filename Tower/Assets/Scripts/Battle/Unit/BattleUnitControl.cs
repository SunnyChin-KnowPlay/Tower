using Dream.Extension.Unity;
using Dream.Utilities;
using UnityEngine;

public class BattleUnitControl : MonoBehaviour, IBattleUnitControl
{
    /// <summary>
    /// 战场控制器
    /// </summary>
    protected BattleControl battleControl = null;

    /// <summary>
    /// 表现控制器
    /// </summary>
    protected BattleUnitShowControl showControl = null;

    /// <summary>
    /// 单元信息
    /// </summary>
    public BattleUnitInfo UnitInfo
    {
        get { return unitInfo; }
        set { unitInfo = value; }
    }
    protected BattleUnitInfo unitInfo;

    protected virtual void Awake()
    {
        battleControl = this.FindComponentInScene<BattleControl>(BattleControl.GameObjectName);
        showControl = this.GetComponentOrAdd<BattleUnitShowControl>();
    }

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public virtual void Setup(BattleUnitInfo info)
    {
        this.unitInfo = info;
    }

    #region Action
    public void MoveToOrigin()
    {
        var grid = battleControl.GetGrid(this.unitInfo.Position);
        if (null == grid)
            return;

        this.transform.position = grid.transform.position;
    }
    #endregion
}
