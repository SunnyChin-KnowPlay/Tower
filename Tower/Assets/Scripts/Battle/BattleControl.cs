using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControl : MonoBehaviour
{
    private BattleModel battleModel = null;

    private Dictionary<int, PlayerControl> players = null;

    private void Awake()
    {
        battleModel = BattleModel.Instance;

        players = new Dictionary<int, PlayerControl>();
    }

    private void OnEnable()
    {
        this.SetupPlayers();
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
    private void SetupPlayers()
    {
        PlayerControl control = null;
        foreach (var pInfo in battleModel.GetAllPlayers())
        {
            control = this.LoadPlayer(pInfo);
            if (null != control)
            {
                players.Add(control.PlayerInfo.Index, control);
            }
        }
    }

    private PlayerControl LoadPlayer(PlayerInfo info)
    {
        return PlayerFactory.Instance.LoadPlayer(info);
    }
    #endregion
}
