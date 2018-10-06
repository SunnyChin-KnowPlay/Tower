using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRelation : MonoBehaviour, IPlayerControl
{
    private PlayerInfo playerInfo;

    public void Setup(PlayerInfo info)
    {
        this.playerInfo = info;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
