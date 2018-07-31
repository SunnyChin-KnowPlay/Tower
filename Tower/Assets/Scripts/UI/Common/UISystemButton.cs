using UnityEngine;
using UnityEngine.UI;

public class UISystemButton : MonoBehaviour
{
    /// <summary>
    /// 图标
    /// </summary>
    private Image iconImage;

    /// <summary>
    /// 标题文本
    /// </summary>
    private Text titleText;

    /// <summary>
    /// 按钮脚本
    /// </summary>
    private Button iconButton;

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

    public void Setup(Sprite iconSprite, string title)
    {
        if (null != iconImage)
        {
            iconImage.sprite = iconSprite;
        }

        this.titleText.text = title;
    }
}

