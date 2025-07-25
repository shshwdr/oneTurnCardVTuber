using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public enum SelectCardsViewType{Discard, Return,RemoveEnergy}
public class SelectCardsView : MenuBase
{
    public Button confirmButton;
    public Button cancelButton;
    public Transform slotParent;
    public RectTransform[] slots;
    public TMP_Text countText;
    Dictionary<RectTransform, CardVisual> visuals = new Dictionary<RectTransform, CardVisual>();

    private int count;
    private SelectCardsViewType type;

    public bool isActive;

    protected override void Start()
    {
        base.Start();
        confirmButton.onClick.AddListener(() =>
        { 
            Confirm();
        });
    }

    public void Show(int count, SelectCardsViewType type)
    {
        visuals.Clear();

        count = math.min(count, HandManager.Instance.handInBattle.Count);
        this.count = count;
        this.type = type;
        Show();
        UpdateView();
        countText.text = $"Select {count} cards";
        isActive = true;
        int i = 0;
        foreach (var slot in slots)
        {
            if (i < count)
            {
                slot.gameObject.SetActive( true);
            }
            else
            {
                slot.gameObject.SetActive(false);
            }

            i++;
        }
        {
            
        }
    }

    public void UpdateView()
    {
        
        confirmButton.gameObject.SetActive(visuals.Values.Count == count);
    }

    public void Confirm()
    {
        foreach (var card in visuals.Values)
        {
            var info = card.GetComponentInChildren<CardVisualize>().cardInfo;
            card.isSelected = false;
            switch (type)
            {
                 case  SelectCardsViewType.Discard:
                    HandManager.Instance.DiscardCard(info);
                    break;
                case SelectCardsViewType.Return:
                    HandManager.Instance.ReturnCard(info);
                    break;
                case SelectCardsViewType.RemoveEnergy:
                    HandManager.Instance.removeEnergy(info);
                    break;
            }
        }

        Hide();
        GameManager.Instance.CheckPlayableAfterPlay();
        EventPool.Trigger("SelectCardFinished");
    }

    public void tryRemove(CardSlot slotT)
    {
        foreach (var slot in slots)
        {
            
                var oldCard = visuals.GetValueOrDefault(slot, null);
                if (oldCard == slotT.cardVisual)
                {
                    oldCard.isSelected = false;
                }
                visuals[ slot] = null;
                UpdateView();
                break;
        }

    }

    public override void Hide(bool immediate = false)
    {
        isActive = false;
        base.Hide(immediate);
    }

    public bool tryInteract(CardSlot cardSlot)
    {
        Vector2 mousePos = Input.mousePosition;
        Camera uiCamera = Camera.main;

        foreach (var slot in slots)
        {
            
            bool isInside = RectTransformUtility.RectangleContainsScreenPoint(slot, mousePos, uiCamera);
            if (isInside)
            {
                var oldCard = visuals.GetValueOrDefault(slot, null);
                if (oldCard)
                {
                    oldCard.isSelected = false;
                }
                Debug.Log("鼠标在 UI 区域内！");
                cardSlot.cardVisual.isSelected = true;
                cardSlot.cardVisual.transform.position = slot.transform.position;
                visuals[ slot] = cardSlot.cardVisual;
                UpdateView();
                return true;
            }
            else
            {
                Debug.Log("鼠标在 UI 区域外！");
            }
        }


        return false;
    }
}
