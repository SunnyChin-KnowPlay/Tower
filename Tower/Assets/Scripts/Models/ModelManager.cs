using Dream.Utilities;
using System.Collections.Generic;

public class ModelManager : Singleton<ModelManager>
{
    private Dictionary<string, IModel> models;

    public T GetModel<T>(string key) where T : BaseModel<T>
    {
        if (models.ContainsKey(key))
        {
            var m = models[key];
            return m as T;
        }
        return null;
    }

    private void Awake()
    {
        models = new Dictionary<string, IModel>();

        SetupModels();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Setup
    /// <summary>
    /// 配置所有的数据模型
    /// </summary>
    private void SetupModels()
    {
        models.Add(UserModel.Key, new UserModel());
        models.Add(BuildModel.Key, new BuildModel());
    }

    /// <summary>
    /// 重置所有的数据模型
    /// </summary>
    public void Reset()
    {
        models.Clear();

        SetupModels();
    }
    #endregion
}
