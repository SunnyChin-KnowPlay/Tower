using Dream.Assets;
using DreamEngine.Net.Protocols.Common;
using DreamEngine.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStartup : UIPanel
{
    public const string Key = "UI/Prefabs/Startup/StartPanel";

    private AssetManager assetManager;

    protected override void Awake()
    {
        base.Awake();
        assetManager = AssetManager.Instance;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();


        StartCoroutine(PreloadAssets());

    }

    /// <summary>
    /// 预加载资源并进入游戏
    /// </summary>
    /// <returns></returns>
    private IEnumerator PreloadAssets()
    {
        if (null != uiManager)
            uiManager.CoverTouch();

        yield return new WaitForEndOfFrame();

        if (null != assetManager)
        {
            yield return assetManager.Updater.Preload("", "");

            var objs = AssetManager.LoadAllAssetSync("tables");


            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < objs.Length; i++)
            {
                TextAsset ta = objs[i] as TextAsset;
                if (null != ta)
                {
                    dict.Add(ta.name, ta.text);
                }
            }

            TableManager tm = TableManager.Instance;
            tm.Parse(dict);
        }
        else
        {

        }

        if (null != uiManager)
            uiManager.DiscoverTouch();
        //
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region UI Event
    public void OnClickStartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
    #endregion
}
