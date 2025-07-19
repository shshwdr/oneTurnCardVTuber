using System.Collections;
using System.Collections.Generic;
using Sinbad;
using UnityEngine;

public class CardInfo
{
    public string identifier;
    public List<string> actions;
    public List<string> types;
    public int selectType;
    public int start;
    public string title;
    public string desc;
    public int energy;
    public int cost;
    public bool canDraw;
    public bool exhaust;
    public int unlockAt;
    public string cardBK;
}

public class DisasterInfo:BaseInfo
{
    public bool canDraw;
    public List<string> actions;
    public int unlockAt;
}

public class ItemInfo:BaseInfo
{
    public bool canDraw;
    public List<string> actions;
    public int cost;
    public int unlockAt;
    public string image;
}

public class BaseInfo
{
    
    public string identifier;
    public string desc;
    public string title;
}

public class TurnRequirementInfo
{
    public int turn;
    public List<int> industryReq;
    public List<int> industryReward;
    public List<int> natureReq;
    public List<int> natureDisaster;

}
public class MiscellaneousInfo
{
    public string key;
    public int intValue;
    public List<int> intValueList;
    public List<float> floatValueList;
    public float floatValue;
    public string stringValue;
}
public class CSVLoader : Singleton<CSVLoader>
{
    public Dictionary<string, CardInfo> cardDict = new Dictionary<string, CardInfo>();
    public Dictionary<int, TurnRequirementInfo> turnRequirementDict = new Dictionary<int, TurnRequirementInfo>();
    public Dictionary<string, DisasterInfo> disasterDict = new Dictionary<string, DisasterInfo>();
    public Dictionary<string, ItemInfo> itemDict = new Dictionary<string, ItemInfo>();
    public Dictionary<string, MiscellaneousInfo> miscellaneousInfoDict = new Dictionary<string, MiscellaneousInfo>();

    // Start is called before the first frame update
    public void Init()
    {
        var heroInfos =
            CsvUtil.LoadObjects<CardInfo>(GetFileNameWithABTest("card"));
        foreach (var info in heroInfos)
        {
            cardDict[info.identifier] = info;
        }
        var turnRequirements =
            CsvUtil.LoadObjects<TurnRequirementInfo>(GetFileNameWithABTest("turnRequirement"));
        foreach (var info in turnRequirements)
        {
            turnRequirementDict[info.turn] = info;
        }
        var disasters =
            CsvUtil.LoadObjects<DisasterInfo>(GetFileNameWithABTest("disaster"));
        foreach (var info in disasters)
        {
            disasterDict[info.identifier] = info;
        }
        var items =
            CsvUtil.LoadObjects<ItemInfo>(GetFileNameWithABTest("item"));
        foreach (var info in items)
        {
            itemDict[info.identifier] = info;
        }
        var miscellaneousInfos =
            CsvUtil.LoadObjects<MiscellaneousInfo>(GetFileNameWithABTest("miscellaneous"));
        foreach (var info in miscellaneousInfos)
        {
            miscellaneousInfoDict[info.key] = info;
        }
    }
    
    string GetFileNameWithABTest(string name)
    {
        // if (ABTestManager.Instance.testVersion != 0)
        // {
        //     var newName = $"{name}_{ABTestManager.Instance.testVersion}";
        //     //check if file in resource exist
        //      
        //     var file = Resources.Load<TextAsset>("csv/" + newName);
        //     if (file)
        //     {
        //         return newName;
        //     }
        // }
        return name;
    }
}


