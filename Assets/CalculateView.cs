using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;

public class CalculateView : MonoBehaviour
{
    public TMP_Text baseValueText;
    public TMP_Text multiplyValueText;
    public TMP_Text valueText;
    public TMP_Text currentTotalValueText;
    public TMP_Text targetTotalValueText;
    
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
        baseValueText.text = GameManager.Instance.BaseValue.ToString();
        multiplyValueText.text = GameManager.Instance.MultiplyValue.ToString();
        //valueText.text = "";//(GameManager.Instance.BaseValue * GameManager.Instance.MultiplyValue).ToString();
        currentTotalValueText.text = GameManager.Instance.CurrentTotalValue.ToString() +"/"+GameManager.Instance.TargetValue;
        targetTotalValueText.text = "1000";
    }
    public void UpdateViewResetValue()
    {
        baseValueText.text = GameManager.Instance.BaseValue.ToString();
        multiplyValueText.text = GameManager.Instance.MultiplyValue.ToString();
        valueText.text = "";//(GameManager.Instance.BaseValue * GameManager.Instance.MultiplyValue).ToString();
        currentTotalValueText.text = GameManager.Instance.CurrentTotalValue.ToString() +"/"+GameManager.Instance.TargetValue;
        targetTotalValueText.text = "1000";
    }
    
    public void UpdateCalculateView()
    {
        baseValueText.text = GameManager.Instance.BaseValue.ToString();
        multiplyValueText.text = GameManager.Instance.MultiplyValue.ToString();
        valueText.text = "="+(GameManager.Instance.BaseValue * GameManager.Instance.MultiplyValue).ToString();
        currentTotalValueText.text = GameManager.Instance.CurrentTotalValue.ToString() +"/"+GameManager.Instance.TargetValue;
        targetTotalValueText.text = "1000";
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
