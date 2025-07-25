using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MenuBase
{
    public Transform cardsParent1;
    public Transform cardsParent2;
    public Transform itemsParent1;
    public Transform itemsParent2;
    public HashSet<string> purchased;
    
    public TMP_Text goldText;

    public Button refreshButton;
    private int refreshCost = 2;
    void Refresh()
    {
        if (GameManager.Instance.Gold >= refreshCost)
        {
            GameManager.Instance.Gold -= refreshCost;
            ShowItemPurchase();
            refreshButton.GetComponentInChildren<TMP_Text>().color = GameManager.Instance.Gold >= refreshCost? Color.black: Color.red;
            goldText.text = GameManager.Instance.Gold.ToString();
        }
    }

    public void ShowCardSelect()
    {
        Show();
    }

    public void ShowItemPurchase()
    {
        purchased = new HashSet<string>();
        Show();
        {
            var allCandidates = CSVLoader.Instance.cardDict.Values.Where(x => x.canDraw).ToList();
            foreach (var cell in cardsParent1.GetComponentsInChildren<ShopCell>(true))
            {
                cell.Init(allCandidates.PickItem());
                cell.cardVisualize.SetInShop();
            }

            foreach (var cell in cardsParent2.GetComponentsInChildren<ShopCell>(true))
            {
                cell.Init(allCandidates.PickItem());
                cell.cardVisualize.SetInShop();
            }
        }


        // {
        //     var allCandidates = CSVLoader.Instance.itemDict.Values.Where(x => x.canDraw && !ItemManager.Instance.items.Contains(x)).ToList();
        //
        //     // foreach (var cell in itemsParent1.GetComponentsInChildren<ShopCell>(true))
        //     // {
        //     //     cell.InitItem(allCandidates.PickItem());
        //     //    // cell.cardVisualize.isInShop = true;
        //     //     FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_click");
        //     // }
        //
        //     foreach (var cell in itemsParent2.GetComponentsInChildren<ShopCell>(true))
        //     {
        //         cell.InitItem(allCandidates.PickItem());
        //        // cell.cardVisualize.isInShop = true;
        //         FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_click");
        //     }
        // }
        UpdateMenu();
        goldText.text = GameManager.Instance.Gold.ToString();
    }

    protected override void Start()
    {
        base.Start();
        goldText.text = GameManager.Instance.Gold.ToString();
        refreshButton.onClick.AddListener(() =>
        {
            Refresh();
        });
        closeButton.onClick.AddListener(() =>
        {
            GameRoundManager.Instance.
                Next();
        });
    }

    public void UpdateMenu()
    {
        // foreach (var cell in itemsParent1.GetComponentsInChildren<ShopCell>(true))
        // {
        //     cell.UpdateCell();
        // }

        // foreach (var cell in itemsParent2.GetComponentsInChildren<ShopCell>(true))
        // {
        //     cell.UpdateCell();
        // }

        goldText.text = GameManager.Instance.Gold.ToString();
        foreach (var cell in cardsParent1.GetComponentsInChildren<ShopCell>(true))
        {
            cell.UpdateCell();
        }

        // foreach (var cell in cardsParent2.GetComponentsInChildren<ShopCell>(true))
        // {
        //     cell.UpdateCell();
        // }
    }
}