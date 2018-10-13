using Dream.Extension.Unity;
using UnityEngine;

public class BattleUnitShowControl : MonoBehaviour, IBattleUnitControl
{
    /// <summary>
    /// 信息
    /// </summary>
    protected BattleUnitInfo unitInfo;

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = this.GetComponentOrAdd<Animator>();
        
    }

    protected virtual void Start()
    {

    }

    public void Setup(BattleUnitInfo info)
    {
        unitInfo = info;


    }
}

