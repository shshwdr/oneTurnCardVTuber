using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;

public class CardListMenu : MenuBase
{

    public Transform cardParent;
    private CardVisualize[] cardVisualizes;
    public TMP_Text title;
    
    public void ShowCardsInDeck()
    {
        Show();
        var cards = HandManager.Instance.ownedCards;
        title.text = "All Cards";
        UpdateCards(cards);

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_click");
    }

    public void ShowCardInDeckInBattle()
    {
        Show();
        
        if (!GameRoundManager.Instance.isInBattle)
        {
             ShowCardsInDeck();
             return;
        }
        title.text = "Cards In Hand";
        var cards = HandManager.Instance.deck;
        UpdateCards(cards);

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_click");
    }

    public void ShowCardInDiscardInBattle()
    {
        Show();
        title.text = "Discarded Cards";
        var cards = HandManager.Instance.discardedInBattle;
        UpdateCards(cards);

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_click");
    }

    void UpdateCards( List<CardInfo>cards)
    {
        
        for (int i = 0; i < cards.Count; i++)
        {
            cardVisualizes[i].Init(cards[i]);
            
            cardVisualizes[i].SetInShop();
            cardVisualizes[i].gameObject.SetActive(true);
        }
        for (int i = cards.Count; i < cardVisualizes.Length; i++)
        {
            cardVisualizes[i].gameObject.SetActive(false);

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_click");
        }
    }
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        cardVisualizes = cardParent.GetComponentsInChildren<CardVisualize>();
        
    }

}
