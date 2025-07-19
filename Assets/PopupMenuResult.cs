using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupMenuResult : MenuBase
{
    public TextPopup textPopup;
    public TMP_Text industryText;
    public TMP_Text natureText;
    public TMP_Text goldText;
    public TMP_Text disasterText;
    public void ShowText(string it,string nt,string gt,string dt)
    {
        Show();
        textPopup.gameObject.SetActive(true);
        industryText.text = it;
        natureText.text = nt;
        goldText.text = gt;
        disasterText.text = dt;
        

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        closeButton.onClick.AddListener(() =>
        {
            GameRoundManager.Instance.Next();   
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
