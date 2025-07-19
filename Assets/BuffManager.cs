using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffManager 
{
    public Dictionary<string, int> buffs = new Dictionary<string, int>();



    // public void RemoveBuff(string type, int value)
    // {
    //     if (!buffs[type].Contains(value))
    //     {
    //         Debug.LogError("BuffManager: remove a non-existent buff");
    //     }
    //     buffs[type].Remove(value);
    // }
    public void AddBuff(string type, int value)
    {
        if (!buffs.ContainsKey(type))
        {
             buffs[type] = value;
        }
        else
        {
            buffs[type] += value;
        }

        //EventPool.Trigger(EventPoolNames.UpdateBuff);
    }
    public void ClearBuff()
    {
        buffs.Clear();
    }

    public List<string> buffTypes()
    {
        return buffs.Keys.ToList();
    }

    // public void RemoveDebuff()
    // {
    //     foreach (var type in new List<BuffType>() {BuffType.heroMaxHP, BuffType.heroAttack})
    //     {
    //         if (buffs.ContainsKey(type))
    //         {
    //             for (int i = 0; i < buffs[type].Count(); i++)
    //             {
    //                 if (buffs[type][i] < 0)
    //                 {
    //                     buffs[type].RemoveAt(i); 
    //                     i--;
    //                 }
    //             }
    //         }
    //         
    //     }
    //     EventPool.Trigger(EventPoolNames.UpdateBuff);
    // }

    public int GetBuffValue(string type)
    {
        if (buffs.ContainsKey(type))
        {
            return buffs[type];
        }

        return 0;
    }

    public bool hasBuff(string type)
    {
        return buffs.ContainsKey(type) && buffs[type] != 0;
    }
    public void SetBuffValue(string type, int value)
    {
        buffs[type] = value;
    }

    public void ClearBuffValue(string type)
    {
         buffs.Remove(type);
    }
}