using Dream.Assets;
using Dream.Extension.Unity;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIPanel
{
    public const string Key = "UI/Prefabs/Home/MainPanel";

    /// <summary>
    /// 用户名字
    /// </summary>
    private Text userNameText;

    /// <summary>
    /// 资源格子列表
    /// </summary>
    private GridLayoutGroup resourcesGrid;

    /// <summary>
    /// 系统按钮格子列表
    /// </summary>
    private GridLayoutGroup systemButtonGrid;

    protected override void Awake()
    {
        base.Awake();

        userNameText = this.transform.Find("UserNameText").GetComponent<Text>();
        resourcesGrid = this.transform.Find("ResourcesGrid").GetComponent<GridLayoutGroup>();
        systemButtonGrid = this.transform.Find("SystemButtonGrid").GetComponent<GridLayoutGroup>();

        // 属性面板配置 -- 开始
        SetupResourcesGrid();
        // 属性面板配置 -- 结束

        // 底下系统按钮 -- 开始
        SetupSystemGrid();
        // 底下系统按钮 -- 结束
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    protected virtual void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Setup
    /// <summary>
    /// 配置资源格子
    /// </summary>
    private void SetupResourcesGrid()
    {
        var userInfo = UserModel.Instance.Self;
        if (null != userInfo && null != userNameText)
        {
            userNameText.text = userInfo.Name;
        }

        var obj = Resources.Load("UI/Prefabs/Common/ResourceNode");

        var resourcesInfo = userInfo.ResourcesInfo;
        if (null != resourcesInfo)
        {
            if (null != obj)
            {
                GameObject go = null;
                UIResourceNode resourceNode = null;

                for (ResourcesInfo.ResourceTypeEnum type = ResourcesInfo.ResourceTypeEnum.Min; type < ResourcesInfo.ResourceTypeEnum.Max; type++)
                {
                    go = GameObject.Instantiate(obj) as GameObject;
                    resourceNode = go.GetComponent<UIResourceNode>();
                    if (null == resourceNode)
                    {
                        resourceNode = go.AddComponent<UIResourceNode>();
                    }
                    if (null != resourceNode)
                    {
                        resourceNode.resourcesInfo = resourcesInfo;
                        resourceNode.resourceType = type;
                    }

                    resourcesGrid.transform.AddChild(resourceNode.transform);
                }
            }
        }
    }

    /// <summary>
    /// 配置系统按钮格子
    /// </summary>
    private void SetupSystemGrid()
    {
        if (null == systemButtonGrid)
            return;

        var obj = Resources.Load("UI/Prefabs/Common/SystemButton");

        if (null == obj)
            return;

       

    }
    #endregion

    #region UI Events
   
    #endregion
}
