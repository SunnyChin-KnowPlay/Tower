using System.Collections;
using UnityEngine;

public enum TransitionTypeEnum
{
    Push = 1,
    Pop = 2,
    Replace = 3,
    Add = 4,
    Close = 5,
}

public interface IUIPanel
{
    IEnumerator OnShow();

    IEnumerator OnHide();

    /// <summary>
    /// 是否为公用资源
    /// </summary>
    /// <returns></returns>
    bool GetIsCommon();
    // coverTouchPanel.transform.SetSiblingIndex(UIDefined.CoverTouchSiblingIndex);
    /// <summary>
    /// 设置渲染索引
    /// </summary>
    /// <returns></returns>
    void SetSiblingIndex();

    /// <summary>
    /// 关闭自己
    /// </summary>
    void OnClose();
}
