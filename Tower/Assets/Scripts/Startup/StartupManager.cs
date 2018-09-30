using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupManager : MonoBehaviour
{
    private UIManager uiManager = null;

    private void Awake()
    {
        uiManager = UIManager.Instance;


    }

    // Use this for initialization
    void Start()
    {
        var startupPanel = uiManager.LoadPanel<UIStartup>(UIStartup.Key);
        if (null != startupPanel)
        {
            uiManager.Push(startupPanel);
        }
    }

}
