using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialMenu : MonoBehaviour
{
    public string tutorialKey;
    public TutorialPage[] pages;

    public Button skipTutorial;

    public UnityEvent  finishExtraAction;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    public void StartTutorial()
    {
        page = 0;
        
        ShowPage(0);
        if (skipTutorial)
        {
            skipTutorial.gameObject.SetActive(true);
            skipTutorial.onClick.AddListener(() =>
            {
                FinishTutorial();
            });
        }
    }
    private void Start()
    {
        StartTutorial();

    }
    public void ShowPage(int page)
    {
        if (pages == null|| pages.Length == 0)
        {
            pages = GetComponentsInChildren<TutorialPage>(true);
        }
        for(int i = 0; i < pages.Length; i++){
            pages[i].gameObject.SetActive(i == page);
            //pages[i].StartPage();
        }
        pages[page].Init(this,page);
    }

    public void HidePage()
    {
        for(int i = 0; i < pages.Length; i++){
            pages[i].gameObject.SetActive(false);
        }
    }

    private int page = 0;

    public void FinishTutorial()
    {
        
        HidePage();
        finishExtraAction?.Invoke();
        TutorialManager.Instance.FinishTutorial();
        if (skipTutorial)
        {
            skipTutorial.gameObject.SetActive(false);
        }
    }
    public void gotoNextPage()
    {
        page++;
        if (pages.Length > page)
        {
            
            ShowPage(page);
        }
        else
        {
            FinishTutorial();
        }
    }
}
