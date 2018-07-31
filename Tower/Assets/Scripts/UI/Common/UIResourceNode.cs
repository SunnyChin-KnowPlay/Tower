using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 资源节点控制脚本
/// 实现以下功能，同步数据到界面上。
/// </summary>
public class UIResourceNode : MonoBehaviour
{

    /// <summary>
    /// 资源的图标
    /// </summary>
    private Image iconImage;

    /// <summary>
    /// 资源的数量文本
    /// </summary>
    private Text countText;

    /// <summary>
    /// 自身对应的资源枚举
    /// </summary>
    public ResourcesInfo.ResourceTypeEnum resourceType;

    /// <summary>
    /// 资源信息
    /// </summary>
    public ResourcesInfo resourcesInfo;

    protected virtual void Awake()
    {
        iconImage = this.transform.Find("Icon").GetComponent<Image>();
        countText = this.transform.Find("Count").GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 当脚本所处的对象被激活时，会进入这个函数
    /// </summary>
    protected virtual void OnEnable()
    {
        Setup();
    }

    /// <summary>
    /// 当脚本所处的对象被失效时(激活的反向操作)，会进入这个函数
    /// </summary>
    protected virtual void OnDisable()
    {

    }

    #region Setup
    /// <summary>
    /// 配置，同步 界面信息
    /// </summary>
    private void Setup()
    {
        // 脚本启动的时候，如果有这个资源信息的对象的话，就利用resourceType这个枚举去里面拿去对应的资源
        if (null != resourcesInfo)
        {
            var count = resourcesInfo.GetResourceCount(resourceType);

            // 看一下上面的GetResourceCount函数 如果返回的值是小于0的话 说明找不到对应的资源 因为资源的数量绝对不会小于0 
            if (count >= 0)
            {
                if (null != countText)
                {
                    countText.text = count.ToString();
                }
            }
        }
    }
    #endregion

    #region UI界面人机交互回调
    public virtual void OnClickAdd()
    {

    }
    #endregion
}
