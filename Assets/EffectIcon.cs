using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EffectIcon : MonoBehaviour
{
    public Button button;
    public TMP_Text text;
    public Image image;
    public string desc;
    public BaseInfo info;

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
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        DescView.Instance.Show(info.desc);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        DescView.Instance.Hide();
    }

    public void Init(BaseInfo info)
    {
        this.info = info;
        text.text = info.title;
        
    }
}
