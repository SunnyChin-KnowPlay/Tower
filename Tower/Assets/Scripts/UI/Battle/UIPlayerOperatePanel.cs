using Dream.Extension.Unity;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerOperatePanel : UIPanel
{
    private GridLayoutGroup skillButtonGrid = null;

    /// <summary>
    /// 当前操作的角色
    /// </summary>
    public BattlePlayerInfo CurrentPlayerInfo
    {
        get { return currentPlayerInfo; }
        set { currentPlayerInfo = value; }
    }
    private BattlePlayerInfo currentPlayerInfo = null;

    protected override void Awake()
    {
        base.Awake();

        skillButtonGrid = this.transform.Find("SkillButtonGrid").GetComponent<GridLayoutGroup>();
    }

    private void OnEnable()
    {
        if (null == currentPlayerInfo)
            return;


    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Setup
    /// <summary>
    /// 配置技能队列
    /// 读取当前操作的角色的技能个数，并对应按钮的数量是否满足，不满足的话则添加直到与技能数量相同。
    /// </summary>
    private void SetupSkills()
    {
        if (null == skillButtonGrid)
            return;

        var skillCount = currentPlayerInfo.Skills.Length;
        if (skillButtonGrid.transform.childCount < skillCount)
        {
            var obj = Resources.Load(UITitleButton.Path);

            GameObject go = null;
            UISkillButton skillButton = null;
            
            for (int i = skillButtonGrid.transform.childCount; i < skillCount; i++)
            {
                var skillInfo = currentPlayerInfo.Skills[i];
                go = GameObject.Instantiate(obj) as GameObject;
                skillButton = go.GetComponentOrAdd<UISkillButton>();
                if (null != skillButton)
                {
                    // 加入带参回调，按下按钮的时候就知道是哪个技能了
                    var repeatButton = skillButton.GetComponentOrAdd<UIRepeatButton>();
                    repeatButton.onPress.AddListener(delegate ()
                    {
                        this.OnPressSkill(skillButton);
                    });

                    repeatButton.onRelease.AddListener(delegate ()
                    {
                        this.OnClickSkill(skillButton);
                    });
                    skillButtonGrid.transform.AddChild(skillButton.transform);
                }
            }
        }

        Transform t = null;
        for (int i = 0; i < skillButtonGrid.transform.childCount; i++)
        {
            t = skillButtonGrid.transform.GetChild(i);
            t.gameObject.SetActive(false);
        }

        for (int i = 0; i < skillCount; i++)
        {
            t = skillButtonGrid.transform.GetChild(i);
            UISkillButton skillButton = t.GetComponent<UISkillButton>();
            skillButton.Setup(currentPlayerInfo.Skills[i]);
        }
    }
    #endregion

    #region UI Event
    /// <summary>
    /// 当技能按钮被按下(可以出Tips)时
    /// </summary>
    /// <param name="button"></param>
    private void OnPressSkill(UISkillButton button)
    {

    }

    /// <summary>
    /// 当技能按钮被抬起时
    /// </summary>
    /// <param name="button"></param>
    private void OnClickSkill(UISkillButton button)
    {

    }
    #endregion
}
