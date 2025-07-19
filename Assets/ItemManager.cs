using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public List<ItemInfo> items = new List<ItemInfo>();
    public BuffManager buffManager = new BuffManager();

    public void AddAll()
    {
        foreach (var v in CSVLoader.Instance.itemDict.Values)
        {
            if (v.canDraw)
             AddItem(v);
        }
    }
    public void AddItem(ItemInfo info)
    {
        items.Add(info);
        for (int i = 0;i< info.actions.Count; i++)
        {
            var action =  info.actions[i];
            switch (action)
            {
                case "shareBoost":
                case "lastCardTwice":
                case "drawWhenEmpty":
                case "addEnergy":
                    case "addEnergyWhenDiscard":
                        case "discardAndDraw":
                    buffManager.AddBuff(action,1);
                    break;
                case "natureLoseAddNatureMan":
                case "industryLoseAddIndustryMan":
                    
                    i++;
                    var value = int.Parse(info.actions[i]);
                    buffManager.AddBuff(action,value);
                    break;
            }
        }
        EventPool.Trigger("ItemChanged");
    }

    public void Clear()
    {
        items.Clear();
        EventPool.Trigger("ItemChanged");
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_joker_activation");
    }
}
