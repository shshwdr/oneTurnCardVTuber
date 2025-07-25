using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Pool;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum RemoveFromHandType
{
    toDiscard,
    toDeck
}
public class HandsView : Singleton<HandsView>
{
    public Transform parent;

    public GameObject cardPrefab;
    [FormerlySerializedAs("drawButton")] public Button endDayButton;

    public Button deckButton;
    public Button discardButton;
    public float moveTime = 1;

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
        EventPool.OptIn("DrawHand", UpdateHandView);
        //UpdateHands();
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
        EventPool.OptIn("EnergyChanged", UpdateHandView);
        
        
        EventPool.OptIn("BaseValueChanged", UpdateHandView);
        EventPool.OptIn("MultiplyValueChanged", UpdateHandView);
        EventPool.OptIn("Calculate", UpdateHandView);
        EventPool.OptIn("AfterCalculate", UpdateHandView);
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
        //UpdateHands();
    }
    public void DrawHand()
    {
        HandManager.Instance.DrawHand();
        //UpdateHands();

        //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_draw_card");
    }

    public void ClearHand()
    {
        HandManager.Instance.ClearHand();
        //UpdateHands();
    }

    public void DrawCards(int count)
    {
        HandManager.Instance.DrawCard(count);
        //UpdateHands();
    }

    public void ReturnCard(int count)
    {
        HandManager.Instance.ReturnCard(count);
        //UpdateHands();
    }

    public void DiscardHand()
    {
        HandManager.Instance.DiscardHand();
        //UpdateHands();
    }
    public void DiscardCards(int count)
    {
        HandManager.Instance.DiscardCards(count);
        //UpdateHands();
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

            FindObjectOfType<HorizontalCardHolder>().InitCard(go.GetComponentInChildren<CardSlot>());
            go.GetComponentInChildren<CardSlot>().Init(info);
        }

        UpdatePileNumber();
    }

    public void UpdateHandView()
    {
        
        foreach (var info in VisualCardsHandler.instance. GetComponentsInChildren<CardVisualize>())
        {
            info.Init(info.cardInfo);
        }

        UpdatePileNumber();
    }

    public void AddCardToHand(CardInfo info)
    {
        var go = Instantiate(cardPrefab.gameObject, parent);

        FindObjectOfType<HorizontalCardHolder>().InitCard(go.GetComponentInChildren<CardSlot>());
        go.GetComponentInChildren<CardSlot>().Init(info);

        UpdateHandView();
    }

    public void RemoveAllFromHand()
    {
        foreach (var info in HandManager.Instance.handInBattle.ToList())
        {
            RemoveCardFromHand(info, RemoveFromHandType.toDiscard);
        }
    }
    public void RemoveCardFromHand(CardInfo info,RemoveFromHandType type)
    {
        var target = transform.position;
        switch (type)
        {
            case RemoveFromHandType.toDiscard:
                target = discardButton.transform.position;
                break;
            case RemoveFromHandType.toDeck:
                target = deckButton.transform.position;
                break;
        }
        foreach (var slot in parent.GetComponentsInChildren<CardSlot>())
        {
            var card = slot.cardVisual;
            var cardInfo = card.GetComponentInChildren<CardVisualize>().cardInfo;
            if (cardInfo == info)
            {
                card.parentCardSlot = null;
                Destroy(slot.transform.parent.gameObject);

                card.transform.DOMove(target, moveTime);
                card.transform.DOScale(0.1f, moveTime);
                card.transform.DORotate(new Vector3(0, 0, 720), moveTime, RotateMode.FastBeyond360);
                Destroy(card.gameObject,moveTime);
            }
        }
        UpdateHandView();
    }
    
    public void RemoveCardFromHandBySelect(CardInfo info)
    {
        var target = transform.position;
        foreach (var slot in parent.GetComponentsInChildren<CardSlot>())
        {
            var card = slot.cardVisual;
            var cardInfo = card.GetComponentInChildren<CardVisualize>().cardInfo;
            if (cardInfo == info)
            {
                card.parentCardSlot = null;
                //Destroy(slot.transform.parent.gameObject);
                //Destroy(card.gameObject,moveTime);
            }
        }
        UpdateHandView();
    }

    public void RemoveAllCardFromHand()
    {
        
    }

}
