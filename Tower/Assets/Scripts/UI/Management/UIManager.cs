using Dream.Extension.Unity;
using Dream.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : Singleton<UIManager>
{
    protected class Transition
    {
        /// <summary>
        /// 下一个界面
        /// </summary>
        public UIPanel Next { get { return next; } }
        private UIPanel next;

        /// <summary>
        /// 转换方式
        /// </summary>
        public TransitionTypeEnum Type { get { return transitionType; } }
        private TransitionTypeEnum transitionType;

        /// <summary>
        /// 是否含有进出场动画
        /// </summary>
        public bool IsAnimation { get; set; }

        public Transition(TransitionTypeEnum transitionType, bool isAnimation, UIPanel next = null)
        {
            this.next = next;
            this.IsAnimation = isAnimation;
            this.transitionType = transitionType;
        }
    }

    /// <summary>
    /// ugui的事件系统
    /// </summary>
    public EventSystem eventSystem;

    /// <summary>
    /// UI照相机
    /// </summary>
    public Camera uiCamera;

    /// <summary>
    /// ui栈
    /// </summary>
    protected Stack<UIPanel> uiStack = null;

    /// <summary>
    /// 已加载的UI面板
    /// </summary>
    protected Dictionary<string, UIPanel> loadedPanels = null;
    #region Transition
    /// <summary>
    /// 转换队列
    /// </summary>
    protected List<Transition> transitions = null;

    public delegate void UIChangedHandle(UIPanel panel, TransitionTypeEnum transitionType);
    /// <summary>
    /// UI界面改变 - 进入
    /// </summary>
    public event UIChangedHandle UIChangedEnter;
    /// <summary>
    /// UI界面改变 - 退出
    /// </summary>
    public event UIChangedHandle UIChangedExit;

    /// <summary>
    /// 是否在处理UI栈
    /// </summary>
    protected bool isStackProcessing = false;
    #endregion

    /// <summary>
    /// 遮盖面板
    /// </summary>
    protected UICoverTouchPanel coverTouchPanel = null;

    private void Awake()
    {
        if (null != eventSystem)
        {
            DontDestroyOnLoad(eventSystem);
        }

        if (null != uiCamera)
        {
            DontDestroyOnLoad(uiCamera);
        }

        loadedPanels = new Dictionary<string, UIPanel>();
        uiStack = new Stack<UIPanel>();
        transitions = new List<Transition>();

        var coverTouchLayerTransform = this.transform.Find("CoverTouchLayer");
        if (null == coverTouchLayerTransform)
        {
            var obj = Resources.Load(UICoverTouchPanel.Key);
            if (null != obj)
            {
                var go = GameObject.Instantiate(obj) as GameObject;
                if (null != go)
                {
                    coverTouchLayerTransform = go.transform;
                    this.transform.AddChild(go.transform);
                }
            }
        }
        if (null != coverTouchLayerTransform)
        {
            coverTouchPanel = coverTouchLayerTransform.GetComponentOrAdd<UICoverTouchPanel>();
            coverTouchPanel.gameObject.SetActive(false);
        }

        this.UIChangedEnter += OnUIPanelEnter;
    }

    // Use this for initialization
    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (null != transitions && transitions.Count > 0)
        {
            var transition = transitions[0];
            if (transition.Type == TransitionTypeEnum.Pop || transition.Type == TransitionTypeEnum.Push || transition.Type == TransitionTypeEnum.Replace)
            {
                if (!isStackProcessing)
                {
                    StartCoroutine(ProcessChangeStack());
                }
            }
            else
            {
                StartCoroutine(ProcessChangeQueue());
            }
        }
    }

    #region Scene Changed
    /// <summary>
    /// 当活动场景被切换时
    /// </summary>
    /// <param name="scene1"></param>
    /// <param name="scene2"></param>
    private void OnSceneUnloaded(UnityEngine.SceneManagement.Scene scene)
    {
        if (this.isActiveAndEnabled)
        {
            RemoveAllWithoutCommon();
        }
    }

    #endregion

    #region Control
    /// <summary>
    /// 加载面板
    /// </summary>
    /// <typeparam name="TPanel">面板脚本</typeparam>
    /// <param name="path">预制件路径</param>
    /// <returns>面板的控制脚本</returns>
    public TPanel LoadPanel<TPanel>(string path) where TPanel : UIPanel
    {
        if (loadedPanels.ContainsKey(path))
            return loadedPanels[path] as TPanel;

        GameObject obj = Resources.Load(path) as GameObject;
        if (null == obj)
            return null;

        GameObject go = GameObject.Instantiate(obj);
        if (null == go)
            return null;

        var rt = this.GetComponent<RectTransform>();
        if (null != rt)
        {
            rt.AddChild(go.GetComponent<RectTransform>());
        }

        TPanel panel = go.GetComponentOrAdd<TPanel>();
        loadedPanels.Add(path, panel);

        ResetAllSiblingIndex();

        return panel;
    }

    /// <summary>
    /// 回收面板
    /// </summary>
    /// <param name="path"></param>
    public void DestroyPanel(string path)
    {
        if (loadedPanels.ContainsKey(path))
        {
            var panel = loadedPanels[path];
            GameObject.Destroy(panel.gameObject);

            loadedPanels.Remove(path);
        }
    }

    /// <summary>
    /// 拦截触摸
    /// </summary>
    public void CoverTouch()
    {
        if (null != coverTouchPanel)
            coverTouchPanel.Cover();
    }

    /// <summary>
    /// 取消拦截触摸
    /// </summary>
    public void DiscoverTouch()
    {
        if (null != coverTouchPanel)
            coverTouchPanel.Discover();
    }

    public void Push(UIPanel ui, bool isAnimation = true)
    {
        Transition t = new Transition(TransitionTypeEnum.Push, isAnimation, ui);
        transitions.Add(t);
    }

    public void Pop(bool isAnimation = true)
    {
        Transition t = new Transition(TransitionTypeEnum.Pop, isAnimation);
        transitions.Add(t);
    }

    public void Replace(UIPanel ui, bool isAnimation = true)
    {
        Transition t = new Transition(TransitionTypeEnum.Replace, isAnimation, ui);
        transitions.Add(t);
    }

    public void Add(UIPanel ui, bool isAnimation = true)
    {
        Transition t = new Transition(TransitionTypeEnum.Add, isAnimation, ui);
        transitions.Add(t);
    }

    public void Close(UIPanel ui, bool isAnimation = true)
    {
        Transition t = new Transition(TransitionTypeEnum.Close, isAnimation, ui);
        transitions.Add(t);
    }

    /// <summary>
    /// 逐个从栈中弹出界面直到到达给定的界面
    /// </summary>
    /// <param name="node"></param>
    private void PopToExits(UIPanel node)
    {
        var list = uiStack.ToArray();
        if (list.Length < 1)
            return;

        int index = -1;
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] == node)
            {
                index = i;
                break;
            }
        }

        if (index >= 0)
        {
            for (int i = 0; i <= index; i++)
            {
                uiStack.Pop();
            }
        }
    }

    /// <summary>
    /// 逻辑大致是，先锁定，防止中途有新的指令插入而影响逻辑
    /// 然后判断类型，如果是push的话，则只把第一个给隐藏而不弹出，剩下的pop和replace都会弹出。
    /// 这中间会做一件事情，把新的节点在栈中寻找，如果找到了则直接弹出到新的节点所处栈的位置。
    /// 之后执行当前最顶层的节点的退出事件。
    /// 接下来把新的推入(如果存在)
    /// 推入后就是执行新的节点的进入事件。
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator ProcessChangeStack()
    {
        isStackProcessing = true;

        if (transitions.Count < 1)
        {
            isStackProcessing = false;
            yield break;
        }

        var transition = transitions[0];
        transitions.RemoveAt(0);
        UIPanel currentPanel = null;

        if (uiStack.Count > 0)
        {
            if (transition.Next != null)
                PopToExits(transition.Next);
            try
            {
                if (uiStack.Count > 0)
                    currentPanel = uiStack.Peek();
            }
            catch (System.InvalidOperationException ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }

        if (null != currentPanel && currentPanel.gameObject.activeSelf)
        {
            if (transition.IsAnimation)
            {
                yield return currentPanel.OnHide();
            }
            currentPanel.gameObject.SetActive(false);
            if (null != UIChangedExit)
            {
                UIChangedExit.Invoke(currentPanel, transition.Type);
            }
        }

        if (transition.Type == TransitionTypeEnum.Pop)
        {
            currentPanel = null;
            if (uiStack.Count > 0)
            {
                uiStack.Pop();
                if (uiStack.Count > 0)
                    currentPanel = uiStack.Peek();
            }
        }
        else if (transition.Type == TransitionTypeEnum.Replace)
        {
            if (uiStack.Count > 0)
            {
                uiStack.Pop();
            }
            currentPanel = transition.Next;
            if (null != currentPanel)
                currentPanel.LastTransitionType = TransitionTypeEnum.Replace;
        }
        else
        {
            currentPanel = transition.Next;
            if (null != currentPanel)
                currentPanel.LastTransitionType = TransitionTypeEnum.Push;
            uiStack.Push(currentPanel);
        }

        if (null != currentPanel)
        {
            currentPanel.gameObject.SetActive(true);
            if (transition.IsAnimation)
            {
                yield return currentPanel.OnShow();
            }

            if (null != UIChangedEnter)
            {
                UIChangedEnter.Invoke(currentPanel, transition.Type);
            }
        }

        if (transition.Type == TransitionTypeEnum.Replace || transition.Type == TransitionTypeEnum.Push)
        {
            currentPanel = transition.Next;

        }

        isStackProcessing = false;
        yield break;

    }

    protected virtual IEnumerator ProcessChangeQueue()
    {
        var transition = transitions[0];
        transitions.RemoveAt(0);
        UIPanel currentPanel = null;
        if (transition.Type == TransitionTypeEnum.Close)
        {
            currentPanel = transition.Next;

            if (null != currentPanel && currentPanel.gameObject.activeSelf)
            {
                if (transition.IsAnimation)
                    yield return currentPanel.OnHide();
                currentPanel.gameObject.SetActive(false);

                if (null != UIChangedExit)
                {
                    UIChangedExit.Invoke(currentPanel, transition.Type);
                }
            }
        }
        else
        {
            currentPanel = transition.Next;

            if (null != currentPanel && !currentPanel.gameObject.activeSelf)
            {
                if (transition.IsAnimation)
                    yield return currentPanel.OnShow();
                currentPanel.gameObject.SetActive(true);
                currentPanel.LastTransitionType = TransitionTypeEnum.Add;

                if (null != UIChangedEnter)
                {
                    UIChangedEnter.Invoke(currentPanel, transition.Type);
                }
            }
        }
    }

    private void OnUIPanelEnter(UIPanel panel, TransitionTypeEnum type)
    {

    }

    /// <summary>
    /// 移除所有的子节点，除公共面板
    /// </summary>
    public void RemoveAllWithoutCommon()
    {
        List<string> keys = new List<string>();
        keys.AddRange(loadedPanels.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            var key = keys[i];
            var value = loadedPanels[key];
            if (null != value && !value.IsCommon)
            {
                value.transform.parent = null;
                Destroy(value.gameObject);

                loadedPanels.Remove(key);
            }
        }

        uiStack.Clear();
        ResetAllSiblingIndex();
    }

    /// <summary>
    /// 重设所有渲染层级
    /// </summary>
    private void ResetAllSiblingIndex()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);

            if (null != child)
            {
                var panel = child.GetComponent<IUIPanel>();
                if (null != panel)
                {
                    panel.SetSiblingIndex();
                }
            }
        }
    }
    #endregion
}
