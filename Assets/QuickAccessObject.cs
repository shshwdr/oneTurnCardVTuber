using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAccessObject : MonoBehaviour
{
    public string accessName;

    private void Awake()
    {
        Init();
    }

    public void SetName(string n)
    {
        accessName = n;
        Init();
    }
    public void Init()
    {
        if (accessName == null || accessName.Length == 0)
        {
            return;
        }
        if (SceneMenuManager.Instance.quickAceessObjects.ContainsKey(accessName))
        {
            Debug.LogError($"QuickAccessObject name already exists {accessName} {name}");
        }
        SceneMenuManager.Instance.quickAceessObjects[accessName] = gameObject;
    }
}