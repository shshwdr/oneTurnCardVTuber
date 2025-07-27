using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
public enum TutorialType{talentMenuTab,cardsMenuTab,shopMenuTab,bpFinishTutorial,pickCardTutorial,redigTutorial,timeModeTutorial,itemUseBookTutorial,itemUseRefreshTutorial,itemUseDoubleTutorial,refreshTutorial,portalTutorial}

public class TutorialManager : Singleton<TutorialManager>
{
    public bool skipStartTutorial = false;
    public bool onlyEnableCertainTutorial = false;
    List<TutorialType> mainMenuTutorial = new List<TutorialType>()
    {
        
    };
    public List<TutorialType> MainMenuTutorial => mainMenuTutorial;
    private int page = 0;
    public TutorialType currentEnabledType;
    private TutorialMenu mainTutorialMenu;
     bool isInTutorial = false;
    public void Init()
    {
        //mainTutorialMenu = GameObject.Find("MainTutorial").GetComponent<TutorialMenu>();
    }

    // public void checkRefreshTutorial()
    // {
    //     if (isAtRefreshWave())
    //     {
    //         if (GameRoundManager.Instance.Gold < 20)
    //         {
    //             GameRoundManager.Instance.AddGold(20 - GameRoundManager.Instance.Gold);
    //         }
    //         TutorialManager.Instance.ShowTutorial(TutorialType.refreshTutorial);
    //     }
    // }



    // public void checkPickCardTutorial()
    // {
    //     TutorialManager.Instance.ShowTutorial(TutorialType.pickCardTutorial);
    // }

    public void checkTutorial(TutorialType type)
    {
        if (onlyEnableCertainTutorial && currentEnabledType != type)
        {
            return;
        }
        switch (type)
        {
            case TutorialType.redigTutorial:
                // if (!hasFinishedTutorial(TutorialType.pickCardTutorial))
                // {
                //     return;
                // }
                break;
        }
        ShowTutorial(type);
    }
    

    public void StartTutorial()
    {
        isInTutorial = true;
    }

    public void FinishTutorial()
    {
        isInTutorial = false;
    }
    
    public bool IsInTutorial=>isInTutorial;


    public TutorialType currentType;
    public void ShowTutorial(TutorialType type)
    {

        if (!shouldShowTutorial(type))
        {
            return;
        }

        if (isInTutorial)
        {
            return;
        }

        currentType = type;
        StartTutorial();
        var prefab = Resources.Load<GameObject>("Tutorials/" + type.ToString());
        var go = Instantiate(prefab, transform);
        go.GetComponent<TutorialMenu>().tutorialKey = type.ToString();
        go.GetComponent<TutorialMenu>().ShowPage(0);
        
    }
    public bool shouldShowTutorial(TutorialType type)
    {

        if (isInTutorial)
        {
            return false;
        }
        // switch (type)
        // {
        //     case TutorialType.HeroUpgrade:
        //         if (HeroManager.Instance == null)
        //         {
        //             return false;
        //         }
        //             return HeroManager.Instance.Level < 1;
        //     case TutorialType.Minion:
        //         if (MinionManager.Instance == null || MinionManager.Instance.equipped == null)
        //         {
        //             return false;
        //         }
        //         return MinionManager.Instance.equipped.Count == 1;
        // }

        return true;
    }

    public bool hasFinishedTutorial(TutorialType type)
    {

        return false;
    }
}
