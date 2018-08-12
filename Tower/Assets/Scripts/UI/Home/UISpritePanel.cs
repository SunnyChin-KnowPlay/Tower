using Dream.Extension.Unity;
using DreamEngine.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpritePanel : UIPanel
{
    public const string Key = "UI/Prefabs/Home/SpritePanel";

    private GridLayoutGroup itemGridLayout = null;

    protected override void Awake()
    {
        base.Awake();

        itemGridLayout = this.transform.Find("Scroll View/Content").GetComponent<GridLayoutGroup>();

    }

    private void OnEnable()
    {
        if (null != itemGridLayout)
        {
            for (int i = 0; i < itemGridLayout.transform.childCount; i++)
            {
                Destroy(itemGridLayout.transform.GetChild(i).gameObject);
            }
        }
        itemGridLayout.transform.DetachChildren();

        List<SpriteInfo> list = new List<SpriteInfo>();
        foreach (var kvp in SpritesTable.Instance)
        {
            SpriteInfo i = new SpriteInfo();
            i.Uid = (uint)kvp.Key;
            i.Count = 20;
            i.Level = 2;
            list.Add(i);
        }

        list.Sort((SpriteInfo l, SpriteInfo r) =>
        {
            return (int)(r.Uid - l.Uid);
        });

        for (int i = 0; i < list.Count; i++)
        {
            UISpriteItem spriteItem = null;
            GameObject obj = Resources.Load("UI/Prefabs/Sprites/Item") as GameObject;
            GameObject go = GameObject.Instantiate(obj);

            spriteItem = go.GetComponentOrAdd<UISpriteItem>();
            spriteItem.SpriteInfo = list[i];

            itemGridLayout.transform.AddChild(go.transform);
        }

        var rt = itemGridLayout.GetComponent<RectTransform>();
        if (null != rt && list.Count > 0)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, list.Count * itemGridLayout.cellSize.y + (list.Count - 1) * itemGridLayout.spacing.y);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
