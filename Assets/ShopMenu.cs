using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopMenu : MenuBase
{
    public Transform cardsParent1;
    public Transform cardsParent2;
    public Transform itemsParent1;
    public Transform itemsParent2;
    public HashSet<string> purchased;

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
    }

    protected override void Start()
    {
        base.Start();
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