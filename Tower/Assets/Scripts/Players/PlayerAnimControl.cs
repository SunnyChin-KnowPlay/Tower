using Dream.Extension.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControl : MonoBehaviour, IPlayerControl
{
    private Animator animator = null;

    public void Setup(PlayerInfo info)
    {
       
    }

    private void Awake()
    {
        animator = this.GetComponentOrAdd<Animator>();
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
