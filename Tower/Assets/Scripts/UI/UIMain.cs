using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    /// <summary>
    /// 用户名字
    /// </summary>
    public Text userNameText;

    // Use this for initialization
    void Start()
    {
        var userInfo = UserModel.Instance.Self;

        if (null != userInfo)
        {
            userNameText.text = userInfo.Name;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
