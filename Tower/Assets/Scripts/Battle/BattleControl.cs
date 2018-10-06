using Dream.Extension.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControl : MonoBehaviour
{
    private BattleModel battleModel = null;

    /// <summary>
    /// 角色的词典
    /// </summary>
    private Dictionary<int, PlayerControl> players = null;

    /// <summary>
    /// 战场格子的词典 1~12 其中1-10是给角色保留的位置 11和12则是代表着水晶的
    /// </summary>
    private Dictionary<int, BattleGridBehaviour> grids = null;

    public Transform gridsTransform = null;

    private void Awake()
    {
        battleModel = BattleModel.Instance;

        players = new Dictionary<int, PlayerControl>();
        grids = new Dictionary<int, BattleGridBehaviour>();
    }

    private void OnEnable()
    {
        this.SetupGrids();

        this.LoadPlayers();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Setup & Loads
    /// <summary>
    /// 加载角色
    /// </summary>
    private void LoadPlayers()
    {
        PlayerControl control = null;
        foreach (var pInfo in battleModel.GetAllPlayers())
        {
            control = this.LoadPlayer(pInfo);
            if (null != control)
            {
                players.Add(control.PlayerInfo.Index, control);
                control.Place();
            }
        }
    }

    /// <summary>
    /// 配置格子
    /// 其中 所有格子的位置都是从1开始的 位置0保留作其他用
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

                // 这里之所以要+1 是让位置的ID从1开始而非0
                gridBehaviour.Position = i + 1;
                this.grids.Add(gridBehaviour.Position, gridBehaviour);
            }
        }
    }

    private PlayerControl LoadPlayer(PlayerInfo info)
    {
        return PlayerFactory.Instance.LoadPlayer(info);
    }
    #endregion

    #region Get
    /// <summary>
    /// 通过位置索引获取对应的格子控制脚本
    /// </summary>
    /// <param name="position">位置索引号</param>
    /// <returns></returns>
    public BattleGridBehaviour GetGrid(int position)
    {
        if (this.grids.ContainsKey(position))
        {
            return grids[position];
        }
        return null;
    }
    #endregion
}
