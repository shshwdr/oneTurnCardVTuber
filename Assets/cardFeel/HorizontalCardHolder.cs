using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.Serialization;

public class HorizontalCardHolder : MonoBehaviour
{

    [FormerlySerializedAs("selectedCard")] [SerializeField] private CardSlot selectedCardSlot;
    [FormerlySerializedAs("hoveredCard")] [SerializeReference] private CardSlot hoveredCardSlot;

    [SerializeField] private GameObject slotPrefab;
    private RectTransform rect;

    [Header("Spawn Settings")]
    [SerializeField] private int cardsToSpawn = 7;
    public List<CardSlot> cards => GetComponentsInChildren<CardSlot>().ToList();

    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;

    public void InitCard(CardSlot cardSlot)
    {
        
            cardSlot.PointerEnterEvent.AddListener(CardPointerEnter);
            cardSlot.PointerExitEvent.AddListener(CardPointerExit);
            cardSlot.BeginDragEvent.AddListener(BeginDrag);
            cardSlot.EndDragEvent.AddListener(EndDrag);
        
    }
    void Start()
    {
        
// for (int i = 0; i < cardsToSpawn; i++)
        // {
        //     Instantiate(slotPrefab, transform);
        // }
        rect = GetComponent<RectTransform>();
        //cards = GetComponentsInChildren<Card>().ToList();

        //int cardCount = 0;

        // foreach (Card card in cards)
        // {
        //     card.PointerEnterEvent.AddListener(CardPointerEnter);
        //     card.PointerExitEvent.AddListener(CardPointerExit);
        //     card.BeginDragEvent.AddListener(BeginDrag);
        //     card.EndDragEvent.AddListener(EndDrag);
        //     card.name = cardCount.ToString();
        //     cardCount++;
        // }

        StartCoroutine(Frame());

        IEnumerator Frame()
        {
            yield return new WaitForSecondsRealtime(.1f);
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].cardVisual != null)
                    cards[i].cardVisual.UpdateIndex(transform.childCount);
            }
        }
    }

    private void BeginDrag(CardSlot cardSlot)
    {
        selectedCardSlot = cardSlot;
    }


    void EndDrag(CardSlot cardSlot)
    {
        if (selectedCardSlot == null)
            return;
        
        
        if (FindObjectOfType<SelectCardsView>().isActive)
        {
            if (FindObjectOfType<SelectCardsView>().tryInteract(cardSlot))
            {
                
                //return;
            }
        }
        else
        {
            if (selectedCardSlot.transform.localPosition.y > selectedCardSlot.selectionOffset*2)
            {
                selectedCardSlot.cardVisual.GetComponentInChildren<CardVisualize>().OnPlace();
                return;
            }

        }

        selectedCardSlot.transform.DOLocalMove(selectedCardSlot.selected ? new Vector3(0,selectedCardSlot.selectionOffset,0) : Vector3.zero, tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);

        rect.sizeDelta += Vector2.right;
        rect.sizeDelta -= Vector2.right;

        selectedCardSlot = null;

    }

    void CardPointerEnter(CardSlot cardSlot)
    {
        hoveredCardSlot = cardSlot;
    }

    void CardPointerExit(CardSlot cardSlot)
    {
        hoveredCardSlot = null;
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Delete))
        // {
        //     if (hoveredCard != null)
        //     {
        //         Destroy(hoveredCard.transform.parent.gameObject);
        //         cards.Remove(hoveredCard);
        //
        //     }
        // }
        //
        // if (Input.GetMouseButtonDown(1))
        // {
        //     foreach (Card card in cards)
        //     {
        //         card.Deselect();
        //     }
        // }

        if (selectedCardSlot == null)
            return;

        if (isCrossing)
            return;

        // for (int i = 0; i < cards.Count; i++)
        // {
        //
        //     if (selectedCard.transform.position.x > cards[i].transform.position.x)
        //     {
        //         if (selectedCard.ParentIndex() < cards[i].ParentIndex())
        //         {
        //             Swap(i);
        //             break;
        //         }
        //     }
        //
        //     if (selectedCard.transform.position.x < cards[i].transform.position.x)
        //     {
        //         if (selectedCard.ParentIndex() > cards[i].ParentIndex())
        //         {
        //             Swap(i);
        //             break;
        //         }
        //     }
        // }
    }

    void Swap(int index)
    {
        isCrossing = true;

        Transform focusedParent = selectedCardSlot.transform.parent;
        Transform crossedParent = cards[index].transform.parent;

        cards[index].transform.SetParent(focusedParent);
        cards[index].transform.localPosition = cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero;
        selectedCardSlot.transform.SetParent(crossedParent);

        isCrossing = false;

        if (cards[index].cardVisual == null)
            return;

        bool swapIsRight = cards[index].ParentIndex() > selectedCardSlot.ParentIndex();
        cards[index].cardVisual.Swap(swapIsRight ? -1 : 1);

        //Updated Visual Indexes
        foreach (CardSlot card in cards)
        {
            card.cardVisual.UpdateIndex(transform.childCount);
        }
    }

}
