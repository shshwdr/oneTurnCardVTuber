using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMenuManager : Singleton<SceneMenuManager>
{
    public Dictionary<string, GameObject> quickAceessObjects = new Dictionary<string, GameObject>();

    public void Clear()
    {
        quickAceessObjects.Clear();
    }
    public GameObject GetQuickAccessObject(string str)
    {
        if (!quickAceessObjects.ContainsKey(str))
        {
            Debug.LogError(("QuickAccessObject not found: " + str));
            return null;
        }
        return quickAceessObjects[str];
    }
}
