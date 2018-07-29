using System.Collections.Generic;
/// <summary>
/// 建筑数据模块
/// </summary>
public class BuildModel : BaseModel<BuildModel>
{
    public const string Key = "Build";

    /// <summary>
    /// 房子的词典迭代器
    /// </summary>
    public IEnumerator<KeyValuePair<uint, HouseInfo>> Houses
    {
        get { return houses.GetEnumerator(); }
    }
    /// <summary>
    /// 塔的词典的迭代器
    /// </summary>
    public IEnumerator<KeyValuePair<uint, TowerInfo>> Towers
    {
        get { return towers.GetEnumerator(); }
    }
    /// <summary>
    /// 老家的信息
    /// </summary>
    public HomeInfo HomeInfo
    {
        get { return homeInfo; }
    }

    private Dictionary<uint, HouseInfo> houses;
    private Dictionary<uint, TowerInfo> towers;
    private HomeInfo homeInfo;

    public BuildModel()
    {
        houses = new Dictionary<uint, HouseInfo>();
        towers = new Dictionary<uint, TowerInfo>();
        homeInfo = new HomeInfo();

    }

    public override string GetKey()
    {
        return Key;
    }

    public override void Reset()
    {
        houses.Clear();
        towers.Clear();
        homeInfo = new HomeInfo();
    }
}

