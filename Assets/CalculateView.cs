using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculateView : MonoBehaviour
{
    public TMP_Text boostValueText;
    public TMP_Text baseValueText;
    public TMP_Text multiplyValueText;
    public TMP_Text valueText;
    public TMP_Text currentTotalValueText;
    public TMP_Text targetTotalValueText;
    public Image element1Img;
    public Image element2Img;
    public CardVisualize cardVisualize;
    // Start is called before the first frame update
    void Start()
    {
        UpdateViewResetValue();
        EventPool.OptIn("BaseValueChanged", UpdateView);
        EventPool.OptIn("MultiplyValueChanged", UpdateView);
        EventPool.OptIn("Calculate", UpdateCalculateView);
        EventPool.OptIn("AfterCalculate", UpdateViewResetValue);
    }
    
    public void UpdateView()
    {
        boostValueText.text = GameManager.Instance.boost.ToString();
        baseValueText.text = GameManager.Instance.BaseValue.ToString();
        multiplyValueText.text = GameManager.Instance.MultiplyValue.ToString();
        //valueText.text = "";//(GameManager.Instance.BaseValue * GameManager.Instance.MultiplyValue).ToString();
        currentTotalValueText.text = GameManager.Instance.CurrentTotalValue.ToString() +"/"+GameManager.Instance.TargetValue+ " - "+GameManager.Instance.TargetLevel;
        targetTotalValueText.text = "1000";
        updateElement();
    }
    public void UpdateViewResetValue()
    {
        boostValueText.text = GameManager.Instance.boost.ToString();
        baseValueText.text = GameManager.Instance.BaseValue.ToString();
        multiplyValueText.text = GameManager.Instance.MultiplyValue.ToString();
        valueText.text = "";//(GameManager.Instance.BaseValue * GameManager.Instance.MultiplyValue).ToString();
        currentTotalValueText.text = GameManager.Instance.CurrentTotalValue.ToString() +"/"+GameManager.Instance.TargetValue+ " - "+GameManager.Instance.TargetLevel;
        targetTotalValueText.text = "1000";
        updateElement();
    }
    
    public void UpdateCalculateView()
    {
        boostValueText.text = GameManager.Instance.boost.ToString();
        baseValueText.text = GameManager.Instance.BaseValue.ToString();
        multiplyValueText.text = GameManager.Instance.MultiplyValue.ToString();
        valueText.text = "="+(GameManager.Instance.BaseValue * GameManager.Instance.MultiplyValue).ToString();
        currentTotalValueText.text = GameManager.Instance.CurrentTotalValue.ToString() +"/"+GameManager.Instance.TargetValue+ " - "+GameManager.Instance.TargetLevel;
        targetTotalValueText.text = "1000";
        updateElement();
    }

    void updateElement()
    {
        
        if (GameManager.Instance.cardInfo != null)
        {
            // element1Img.sprite = ElementManager.Instance.sprites[GameManager.Instance.element1];
            // element2Img.sprite = ElementManager.Instance.sprites[GameManager.Instance.element2];
            cardVisualize.gameObject.SetActive(true);
            cardVisualize.Init(GameManager.Instance.cardInfo);
            cardVisualize.canInteract = false;
        }
        else
        {
            cardVisualize.gameObject.SetActive(false);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
