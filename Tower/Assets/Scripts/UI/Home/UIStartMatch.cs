using UnityEngine;
using UnityEngine.UI;

public class UIStartMatch : UIPanel
{
    public const string Key = "UI/Prefabs/Home/StartMatchPanel";

    /// <summary>
    /// 开始比赛按钮
    /// </summary>
    private Button startMatchButton = null;

    protected override void Awake()
    {
        base.Awake();

        startMatchButton = this.transform.Find("StartButton").GetComponent<Button>();
        startMatchButton.onClick.AddListener(OnClickStart);
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region UI Event
    private void OnClickStart()
    {
        BattleModel bm = ModelManager.Instance.GetModel<BattleModel>(BattleModel.Key);
        bm.Reset();

        BattleUnitInfo info = null;
        for (int i = 0; i < 5; i++)
        {
            info = new BattlePlayerInfo();
            info.Hp = 100;
            info.HpMax = 100;
            info.Index = i + 1;
            info.Position = BattleGridPositionEnum.Home_Player_Start + (uint)i;
            info.UnitType = BattleUnitTypeEnum.Player;
            info.RowId = (i % 3) + 1;
            info.Name = "哈哈";
            bm.AllUnits.Add(info.Index, info);
        }

        for (int i = 0; i < 5; i++)
        {
            info = new BattlePlayerInfo();
            info.Hp = 100;
            info.HpMax = 100;
            info.Index = i + 1 + 5;
            info.Position = BattleGridPositionEnum.Away_Player_Start + (uint)i;
            info.UnitType = BattleUnitTypeEnum.Player;
            info.RowId = (i % 3) + 1;
            info.Name = "哈哈";
            bm.AllUnits.Add(info.Index, info);
        }

        for (int i = 0; i < 5; i++)
        {
            info = new BattleMinionInfo();
            info.Hp = 100;
            info.HpMax = 100;
            info.Index = i + 1 + 10;
            info.Position = BattleGridPositionEnum.Home_Player_Start + (uint)(i + 5);
            info.UnitType = BattleUnitTypeEnum.Minion;
            info.RowId = (i % 3) + 1;
            info.Name = "呵呵";
            bm.AllUnits.Add(info.Index, info);
        }

        for (int i = 0; i < 5; i++)
        {
            info = new BattleMinionInfo();
            info.Hp = 100;
            info.HpMax = 100;
            info.Index = i + 1 + 15;
            info.Position = BattleGridPositionEnum.Away_Player_Start + (uint)(i + 5);
            info.UnitType = BattleUnitTypeEnum.Minion;
            info.RowId = (i % 3) + 1;
            info.Name = "呵呵";
            bm.AllUnits.Add(info.Index, info);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("NormalBattle");

    }
    #endregion
}
