using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescView : Singleton<DescView>
{
    public TMP_Text text;
    public GameObject go;
    public void Show(string t)
    {
        go.SetActive(true);
        text.text = t;
    }
    public void Hide()
    {
        go.SetActive(false);
    }
}
