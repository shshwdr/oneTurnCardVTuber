using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MeterView : MonoBehaviour
{
    public Transform resultParent;

    public Transform targetParent;

    public Transform targetProgressParent;
    public TMP_Text value;
    
    RewardCell[] resultImages;
    Image[] targetImages;
    TMP_Text[] resultTexts;
    TMP_Text[] targetTexts;
    
    public Button button;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        var str = isIndustry?"Reach enough industry point <sprite name=\"Industry\"> before end of year to earn money<sprite name=\"Money\">. If you failed reaching any target, you lose.":
                 "Reach enough nature point <sprite name=\"Nature\"> before end of year to avoid disasters<sprite name=\"Disaster\">. If you failed reaching any target, you lose.";
        DescView.Instance.Show(str);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        DescView.Instance.Hide();
    }

    public bool isIndustry;
    // Start is called before the first frame update
    void Awake()
    {
        resultImages = resultParent.GetComponentsInChildren<RewardCell>();
            targetImages = targetProgressParent.GetComponentsInChildren<Image>();
        resultTexts = resultParent.GetComponentsInChildren<TMP_Text>();
        targetTexts =  targetParent.GetComponentsInChildren<TMP_Text>();
        
        EventPool.OptIn("IndustryChanged", UpdateView);
        EventPool.OptIn("NatureChanged", UpdateView);
        
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

    

    public void UpdateViewForStartOfTurn()
    {
        var currentTurnReq = CSVLoader.Instance.turnRequirementDict[GameManager.Instance.Day];
        var rewardList = isIndustry?currentTurnReq.industryReward:currentTurnReq.natureDisaster;
        for (int i = 0; i < resultTexts.Length; i++)
        {
            if (rewardList[i] == -1)
            {
                //resultTexts[i].text = "DIE";
                resultImages[i].gameObject.SetActive(false);
            }
            else
            {
                resultImages[i].gameObject.SetActive(true);
                var spriteName =  isIndustry ? "Money" : "Disaster" ;
                resultTexts[i].text =  rewardList [i].ToString() +$"<sprite name=\"{spriteName}\">";
            }
        }
        var reqList = isIndustry?currentTurnReq.industryReq:currentTurnReq.natureReq;
        for (int i = 0; i < targetTexts.Length; i++)
        {
            {
                
                var spriteName =  isIndustry ? "Industry" : "Nature" ;
                targetTexts[i].text = reqList[i].ToString();//+$"<sprite name=\"{spriteName}\">";
            }
        }
        UpdateView();
        
        
    }

    public string currentResult = "";
    public void UpdateView()
    {
        currentResult = "DIE";
        var currentTurnReq = CSVLoader.Instance.turnRequirementDict[GameManager.Instance.Day];
        var reqList = isIndustry?currentTurnReq.industryReq:currentTurnReq.natureReq;
        var rewardList = isIndustry?currentTurnReq.industryReward:currentTurnReq.natureDisaster;
        var currentValue = isIndustry? GameManager.Instance.Industry:GameManager.Instance.Nature;
        value.text = currentValue +"/" + reqList.LastItem();
        if (currentValue < reqList[0])
        {
            value.color = Color.red;
            
        }
        else
        {
            value.color = Color.black;
        }

        int index = -1;
        bool firstFinished = true;
        for (int i = rewardList.Count-1;i>=0;i--)
        {
            int prevReq = i ==0 ? 0: reqList[i-1];
            
            if (currentValue < reqList[i])
            {
                resultImages[i].GetComponent<RewardCell>().check.SetActive(false);
                targetImages[i].fillAmount = (currentValue- prevReq)/(float)(reqList[i] - prevReq);
                // if (i == 0 && firstFinished)
                // {
                //     
                //     //resultImages[i].GetComponent<RewardCell>().check.SetActive(true);
                //     currentResult = resultTexts[i].text;
                // }
            }

            else if (currentValue >= reqList[i])
            {
                resultImages[i].GetComponent<RewardCell>().check.SetActive(true);
                if (firstFinished &&rewardList[i] != -1)
                {
                    // if (i == 0)
                    // {
                    //     //resultImages[i].color = Color.red;
                    //     currentResult = resultTexts[i].text;
                    // }
                    // else
                    {
                        
                        currentResult = rewardList[i].ToString();
                    }
                    firstFinished = false;
                    index = i;
                }
                else
                {
                    
                    resultImages[i].GetComponent<RewardCell>().check.SetActive(true);
                }
                
                targetImages[i].fillAmount = (currentValue- prevReq)/(float)(reqList[i] - prevReq);
            }
        }
        
        EventPool.Trigger("meterUpdate",index,isIndustry);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
