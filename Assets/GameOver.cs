using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MenuBase
{
    public Button button;

    public TMP_Text text;
    public TMP_Text title;

    public void ShowText(string t,bool isWin)
    {
        text.text = t;
        Show();

        if (isWin)
        {
            title.text = "You Win!";
        }
        else
        {
            title.text = "You Lose!";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartGame();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
