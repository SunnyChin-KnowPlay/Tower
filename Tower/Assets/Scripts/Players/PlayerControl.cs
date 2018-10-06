using Dream.Extension.Unity;
using Dream.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour, IPlayerControl
{
    /// <summary>
    /// 角色的信息
    /// </summary>
    public PlayerInfo PlayerInfo
    {
        get { return playerInfo; }
    }
    private PlayerInfo playerInfo = null;

    /// <summary>
    /// 动画控制器
    /// </summary>
    private PlayerAnimControl animControl = null;

    /// <summary>
    /// 战场控制
    /// </summary>
    private BattleControl battleControl = null;

    private List<IPlayerControl> controls = null;

    public void Awake()
    {
        controls = new List<IPlayerControl>();

        battleControl = this.FindComponentInScene<BattleControl>("Battle");

        animControl = this.GetComponentOrAdd<PlayerAnimControl>();
        controls.Add(animControl);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Setup
    public void Setup(PlayerInfo info)
    {
        this.playerInfo = info;

        if (null != playerInfo)
        {
            if (null != controls)
            {
                for (int i = 0; i < controls.Count; i++)
                    controls[i].Setup(info);
            }
        }
    }
    #endregion

    #region Action
    /// <summary>
    /// 落位
    /// 通过battle寻找格子的位置值
    /// </summary>
    public void Place()
    {
        var grid = battleControl.GetGrid(this.playerInfo.Position);
        if (null == grid)
            return;

        this.transform.position = grid.transform.position;
    }

    /// <summary>
    /// 跳跃回原位置
    /// </summary>
    /// <param name="duration">耗时</param>
    /// <returns></returns>
    public IEnumerator JumpToOrigin(float duration)
    {
        yield return new WaitForSeconds(duration);

        this.Place();
    }

    #endregion
}
