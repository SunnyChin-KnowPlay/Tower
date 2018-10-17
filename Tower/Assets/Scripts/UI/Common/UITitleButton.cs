using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 带标题的按钮，标题置于下方
/// </summary>
public class UITitleButton : MonoBehaviour
{
    public const string Path = "UI/Prefabs/Common/TitleButton";

    /// <summary>
    /// 图标
    /// </summary>
    protected Image iconImage;

    /// <summary>
    /// 标题文本
    /// </summary>
    protected Text titleText;

    /// <summary>
    /// 按钮脚本
    /// </summary>
    protected Button iconButton;

    /// <summary>
    /// 按钮脚本属性
    /// </summary>
    public Button IconButton
    {
        get { return iconButton; }
    }

    private void Awake()
    {
        iconImage = this.GetComponent<Image>();
        iconButton = this.GetComponent<Button>();
        titleText = this.transform.Find("TitleText").GetComponent<Text>();
    }

    private void Start()
    {

    }

    public virtual void Setup(Sprite iconSprite, string title)
    {
        if (null != iconImage)
        {
            iconImage.sprite = iconSprite;
        }

        this.titleText.text = title;
    }
}

