using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
using UnityEngine.Events;

public class TutorialMenu : MonoBehaviour
{
    public string tutorialKey;
    public TutorialPage[] pages;

    public UnityEvent  finishExtraAction;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    public void ShowPage(int page)
    {
        if (pages == null|| pages.Length == 0)
        {
            pages = GetComponentsInChildren<TutorialPage>(true);
        }
        for(int i = 0; i < pages.Length; i++){
            pages[i].gameObject.SetActive(i == page);
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
