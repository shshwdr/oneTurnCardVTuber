using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoostIcon : MonoBehaviour
{
    public bool isIndustry;
    // Start is called before the first frame update
    public Button button;
    public TMP_Text text;
    public Image image;
    public string desc;

    private void Start()
    {
        
        EventTrigger eventTrigger = button.gameObject.GetComponent<EventTrigger>();

        if (eventTrigger == null)
        {
            eventTrigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // 创建一个 PointerEnter 事件
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        
        
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });

        // 将 PointerEnter 事件添加到 EventTrigger 中
        eventTrigger.triggers.Add(entry);
        eventTrigger.triggers.Add(entry2);
        
        
        UpdateIcon();
        EventPool.OptIn("StateChanged", UpdateIcon);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        DescView.Instance.Show("Boost: Multiply the effect of Scientists or Activists");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        DescView.Instance.Hide();
    }

    

    void UpdateIcon()
    {
        var value = isIndustry ? HandManager.Instance.effectiveIndustryBoost() : HandManager.Instance.effectiveNatureBoost();
        text.text = $"x{value }";

        button.gameObject.SetActive(value != 0);
        if (value == 0)
        {
            
        }
    }
}
