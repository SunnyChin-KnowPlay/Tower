using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基础UI行为脚本
/// </summary>
public class UIPanel : MonoBehaviour, IUIPanel
{
    protected Animator animator = null;
    protected UIManager uiManager = null;

    /// <summary>
    /// 最后一次的转换方式
    /// </summary>
    public TransitionTypeEnum LastTransitionType { get; set; }
    /// <summary>
    /// 该面板是否为公用面板
    /// </summary>
    public bool IsCommon { get; set; }

    /// <summary>
    /// 进场退场时是否需要拦截触摸事件
    /// </summary>
    protected bool isCoverTouchInIO = false;

    protected virtual void Awake()
    {
        animator = this.GetComponent<Animator>();
        uiManager = UIManager.Instance;

        IsCommon = false;
    }

    // Use this for initialization
    protected virtual void Start()
    {

    }

    #region Animator 
    /// <summary>
    /// 在进场或退场中是否阻拦触摸
    /// </summary>
    /// <returns>true:是</returns>
    protected virtual bool IsCoverTouchInIO()
    {
        return isCoverTouchInIO;
    }

    public IEnumerator OnHide()
    {
        if (null == animator)
            yield break;

        bool isTouchCoverd = false;

        if (IsCoverTouchInIO() && null != uiManager)
        {
            uiManager.CoverTouch();
            isTouchCoverd = true;
        }

        animator.SetTrigger("Out");
        yield return new WaitForEndOfFrame();
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();

        if (isTouchCoverd)
        {
            uiManager.DiscoverTouch();
        }
    }

    public IEnumerator OnShow()
    {
        if (null == animator)
            yield break;

        bool isTouchCoverd = false;

        if (IsCoverTouchInIO() && null != uiManager)
        {
            uiManager.CoverTouch();
            isTouchCoverd = true;
        }

        animator.SetTrigger("In");

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();

        if (isTouchCoverd)
        {
            uiManager.DiscoverTouch();
        }
    }
    #endregion

    #region Common
    public virtual bool GetIsCommon()
    {
        return IsCommon;
    }

    public virtual void SetSiblingIndex()
    {

    }
    #endregion

    #region Close
    public virtual void OnClose()
    {
        if (null != uiManager)
        {
            if (this.LastTransitionType == TransitionTypeEnum.Push || this.LastTransitionType == TransitionTypeEnum.Replace)
                uiManager.Pop();
            else if (this.LastTransitionType == TransitionTypeEnum.Add)
                uiManager.Close(this);
        }
    }
    #endregion

    #region Control
    public virtual void OnClickClose()
    {
        this.OnClose();
    }
    #endregion
}
