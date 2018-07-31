﻿using Dream.Assets;
using Dream.Extension.Unity;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
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

    protected virtual void Awake()
    {
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
    void Start()
    {


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

        GameObject go = null;
        UISystemButton systemButton = null;
        Sprite iconSprite = null;

        go = GameObject.Instantiate(obj) as GameObject;
        // 这个函数是一个扩展函数，他的意思是，如果这个对象身上已经有这个脚本了，就会直接获取到，如果没有的话，则先添加并获取该脚本
        systemButton = go.GetComponentOrAdd<UISystemButton>();
        if (null != systemButton)
        {
            iconSprite = Resources.Load<Sprite>("UI/GemsIcons/01");
            systemButton.Setup(iconSprite, "老家");
            systemButton.IconButton.onClick.AddListener(OnClickHome);
            systemButtonGrid.transform.AddChild(systemButton.transform);
        }

        go = GameObject.Instantiate(obj) as GameObject;
        systemButton = go.GetComponentOrAdd<UISystemButton>();
        if (null != systemButton)
        {
            iconSprite = Resources.Load<Sprite>("UI/GemsIcons/02");
            systemButton.Setup(iconSprite, "任务");
            systemButton.IconButton.onClick.AddListener(OnClickTask);
            systemButtonGrid.transform.AddChild(systemButton.transform);
        }


    }
    #endregion

    #region UI Events
    /// <summary>
    /// 点击任务面板
    /// </summary>
    private void OnClickTask()
    {
        UnityEngine.Debug.Log("OnClickTask");
    }

    /// <summary>
    /// 点击主城按钮
    /// </summary>
    private void OnClickHome()
    {
        UnityEngine.Debug.Log("OnClickHome");
    }

    /// <summary>
    /// 点击塔按钮
    /// </summary>
    private void OnClickTower()
    {

    }

    /// <summary>
    /// 点击房子按钮
    /// </summary>
    private void OnClickHouse()
    {

    }
    #endregion
}
