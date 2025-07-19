using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour
{
    public bool isIndustry;
    public Button button;
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
        
        EventPool.OptIn("CharacterChanged",updateCharacter);
        EventPool.OptIn("DisasterChanged",updateCharacter);
        
    }
    public TMP_Text characterCount;
    public void updateCharacter()
    {
        characterCount.text ="X"+ (isIndustry?GameManager.Instance.industryManCount:GameManager.Instance.natureManCount.ToString());
    }
    public void OnPointerEnter(PointerEventData eventData)
    {            
        var str = isIndustry? $"Scientist: +{CSVLoader.Instance.miscellaneousInfoDict["valueAddPerMan"].intValue} <sprite name=\"Industry\"> per scientist when a card adds <sprite name=\"Industry\">\n"
            :$"Activist: +{CSVLoader.Instance.miscellaneousInfoDict["valueAddPerMan"].intValue} <sprite name=\"Nature\"> per Activist when a card adds <sprite name=\"Nature\">\n";

        DescView.Instance.Show(str);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        DescView.Instance.Hide();
    }

}
