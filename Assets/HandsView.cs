using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HandsView : Singleton<HandsView>
{
    public Transform parent;

    public CardVisualize cardPrefab;
    [FormerlySerializedAs("drawButton")] public Button endDayButton;

    public Button deckButton;
    public Button discardButton;

    public void InitLevel()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        // foreach (var info in HandManager.Instance.hand)
        // {
        //     var go = Instantiate(cardPrefab.gameObject, parent);
        //     go.GetComponent<CardVisualize>().Init(info);
        // }
        EventPool.OptIn("DrawHand", UpdateHands);
        UpdateHands();
        endDayButton.onClick.AddListener(() =>
        {
            EndTurn();
          //  FindObjectOfType<TutorialMenu>(). FinishUseRedraw();
        });
        //endDayButton.gameObject.SetActive(false);
        deckButton.onClick.AddListener(() =>
        {
            FindObjectOfType<CardListMenu>().ShowCardInDeckInBattle();
        });
        discardButton.onClick.AddListener(() =>
        {
            FindObjectOfType<CardListMenu>().ShowCardInDiscardInBattle();
        });
        //UpdatePileNumber();
        EventPool.OptIn("HandUpdate", UpdatePileNumber);
        EventPool.OptIn("EnergyChanged", UpdateHands);
    }

    public void UpdatePileNumber()
    {
        bool isInBattle = GameRoundManager.Instance.isInBattle;
        deckButton.GetComponentInChildren<TMP_Text>().text = isInBattle? HandManager.Instance.deck.Count.ToString(): HandManager.Instance.ownedCards.Count.ToString();
        discardButton.GetComponentInChildren<TMP_Text>().text = isInBattle?HandManager.Instance.discardedInBattle.Count.ToString():0.ToString();
    }

    public void EndTurn()
    {
        
        GameManager.Instance.Turn++;
        if (GameManager.Instance.Turn == 1)
        {
            ClearHand();
        }
        else
        {
            DrawHand();
        }
    }

    // public void showRedrawButton()
    // {
    //     endDayButton.gameObject.SetActive(true);
    // }

    public void ResetHandAndDrawHand()
    {
        HandManager.Instance.ClearHandAndDrawHand();
        UpdateHands();
    }
    public void DrawHand()
    {
        HandManager.Instance.DrawHand();
        UpdateHands();

        //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_draw_card");
    }

    public void ClearHand()
    {
        HandManager.Instance.ClearHand();
        UpdateHands();
    }

    public void DrawCards(int count)
    {
        HandManager.Instance.DrawCard(count);
        UpdateHands();
    }

    public void DiscardHand()
    {
        HandManager.Instance.DiscardHand();
        UpdateHands();
    }
    public void DiscardCards(int count)
    {
        HandManager.Instance.DiscardCards(count);
        UpdateHands();
    }
    public void UpdateHands()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
        foreach (var info in HandManager.Instance.handInBattle)
        {
            var go = Instantiate(cardPrefab.gameObject, parent);
            go.GetComponent<CardVisualize>().Init(info);
        }

        UpdatePileNumber();
    }

}
