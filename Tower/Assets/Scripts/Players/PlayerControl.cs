using Dream.Extension.Unity;
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

    private List<IPlayerControl> controls = null;

    public void Awake()
    {
        controls = new List<IPlayerControl>();

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
}
