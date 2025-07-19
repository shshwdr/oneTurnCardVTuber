using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenuDisaster : MenuBase
{
    public TextPopup textPopup;
    public TextPopup titlePopup;
    public void ShowText(string text)
    {
        Show();
        textPopup.gameObject.SetActive(true);
        textPopup.text.text = text;

    }

    public void ShowTitle(string text)
    {
        Show();
        titlePopup.gameObject.SetActive(true);
        titlePopup.text.text = text;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
