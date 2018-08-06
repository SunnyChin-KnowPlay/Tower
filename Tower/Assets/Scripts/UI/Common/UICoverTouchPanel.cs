using UnityEngine;

public class UICoverTouchPanel : UIPanel
{
    public const string Key = "UI/Prefabs/Common/CoverTouchPanel";

    /// <summary>
    /// 拦截计数
    /// </summary>
    private int coverCount = 0;

    /// <summary>
    /// 拦截计数
    /// </summary>
    public int CoverCount
    {
        get { return coverCount; }
    }

    protected override void Awake()
    {
        base.Awake();


        IsCommon = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 拦截
    /// </summary>
    public void Cover()
    {
        if (coverCount < 1)
        {
            this.gameObject.SetActive(true);
        }

        coverCount++;
    }

    /// <summary>
    /// 取消拦截
    /// </summary>
    public void Discover()
    {
        coverCount--;
        if (coverCount <= 0)
        {
            this.gameObject.SetActive(false);
            coverCount = 0;
        }
    }

    public override void SetSiblingIndex()
    {
        base.SetSiblingIndex();

        this.transform.SetSiblingIndex(UIDefined.CoverTouchSiblingIndex);
    }
}

