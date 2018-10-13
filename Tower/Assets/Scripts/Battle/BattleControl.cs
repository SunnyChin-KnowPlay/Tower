using Dream.Extension.Unity;
using Dream.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControl : MonoBehaviour
{
    private static string gameObjectName = null;
    /// <summary>
    /// 游戏对象名
    /// </summary>
    public static string GameObjectName
    {
        get { return gameObjectName; }
    }

    private BattleModel battleModel = null;

    /// <summary>
    /// 单位工厂
    /// </summary>
    private BattleUnitFactory unitFactory = null;

    /// <summary>
    /// 角色的词典
    /// </summary>
    private Dictionary<int, BattleUnitControl> units = null;

    /// <summary>
    /// 战场格子的词典
    /// </summary>
    private Dictionary<BattleGridPositionEnum, BattleGridBehaviour> grids = null;

    public Transform gridsTransform = null;

    private void Awake()
    {
        gameObjectName = this.gameObject.name;

        battleModel = BattleModel.Instance;

        unitFactory = this.FindComponentInScene<BattleUnitFactory>("UnitFactory");

        units = new Dictionary<int, BattleUnitControl>();
        grids = new Dictionary<BattleGridPositionEnum, BattleGridBehaviour>();
    }

    private void OnEnable()
    {
        this.SetupGrids();

        this.LoadUnits();
    }

    // Use this for initialization
    void Start()
    {
        SetupUnits();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Setup & Loads
    /// <summary>
    /// 加载单位
    /// </summary>
    private void LoadUnits()
    {
        BattleUnitControl control = null;
        foreach (var pInfo in battleModel.GetAllUnits())
        {
            control = this.LoadUnit(pInfo);
            if (null != control)
            {
                units.Add(control.UnitInfo.Index, control);
            }
        }
    }

    private void SetupUnits()
    {
        foreach (var kvp in units)
        {
            kvp.Value.MoveToOrigin();
        }
    }

    /// <summary>
    /// 配置格子
    /// </summary>
    private void SetupGrids()
    {
        if (null != gridsTransform)
        {
            for (int i = 0; i < gridsTransform.childCount; i++)
            {
                var t = gridsTransform.GetChild(i);

                var gridBehaviour = t.GetComponentOrAdd<BattleGridBehaviour>();
                if (null == gridBehaviour)
                    continue;

                gridBehaviour.Position = this.GetGridPositionFromName(t.gameObject.name);
                this.grids.Add(gridBehaviour.Position, gridBehaviour);
            }
        }
    }

    /// <summary>
    /// 通过名字来获取格子位置的枚举
    /// 首先计算2个base基地，然后判定名字中是否含有Grid_1 取最后的字符串
    /// </summary>
    /// <param name="name">格子对象的节点名字</param>
    /// <returns></returns>
    private BattleGridPositionEnum GetGridPositionFromName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return BattleGridPositionEnum.None;

        if (name == "Grid_1_Base")
            return BattleGridPositionEnum.Home_Base;
        if (name == "Grid_2_Base")
            return BattleGridPositionEnum.Away_Base;
        if (name.Contains("Grid_1_"))
        {
            var subString = name.Substring(name.LastIndexOf('_') + 1);
            uint number = 0;

            if (!uint.TryParse(subString, out number))
            {
                return BattleGridPositionEnum.None;
            }

            return BattleGridPositionEnum.Home_Player_Start + number - 1;
        }
        if (name.Contains("Grid_2_"))
        {
            var subString = name.Substring(name.LastIndexOf('_') + 1);
            uint number = 0;

            if (!uint.TryParse(subString, out number))
            {
                return BattleGridPositionEnum.None;
            }

            return BattleGridPositionEnum.Away_Player_Start + number - 1;
        }
        return BattleGridPositionEnum.None;
    }

    private BattleUnitControl LoadUnit(BattleUnitInfo info)
    {
        return unitFactory.LoadUnit(info);
    }
    #endregion

    #region Get
    /// <summary>
    /// 通过位置索引获取对应的格子控制脚本
    /// </summary>
    /// <param name="position">位置索引号</param>
    /// <returns></returns>
    public BattleGridBehaviour GetGrid(BattleGridPositionEnum position)
    {
        if (this.grids.ContainsKey(position))
        {
            return grids[position];
        }
        return null;
    }
    #endregion
}
