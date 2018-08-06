using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    private UIMain mainPanel = null;
    private UIManager uiManager;

    private void Awake()
    {
        uiManager = UIManager.Instance;
        mainPanel = uiManager.LoadPanel<UIMain>(UIMain.Key);
        if (null != mainPanel)
        {
            uiManager.Push(mainPanel);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
