using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

public class DisasterManager : Singleton<DisasterManager>
{
    public List<DisasterInfo> disasters = new List<DisasterInfo>();
    public BuffManager buffManager = new BuffManager();
    
    public void AddAll()
    {
        foreach (var v in CSVLoader.Instance.disasterDict.Values)
        {
            AddDisaster(v);
        }
    }
    public void AddDisaster(DisasterInfo info)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster");

        disasters.Add(info);
        
        for (int i = 0;i< info.actions.Count; i++)
        {
            var action =  info.actions[i];
            switch (action)
            {
                case "disableIndustryMan":
                case "disableNatureMan":
                case "doubleLose":
                case "IndustryLoseNature":
                case "natureLoseIndustry":
                    buffManager.AddBuff(action,1);
                    break;
                case "endTurn":
                    
                    i++;
                    var value = int.Parse(info.actions[i]);
                    buffManager.AddBuff(action,value);
                    break;
            }
        }
        
        
        EventPool.Trigger("DisasterChanged");
    }

    public void ClearDisaster()
    {
        disasters.Clear();
        buffManager.ClearBuff();
        EventPool.Trigger("DisasterChanged");
    }
}
