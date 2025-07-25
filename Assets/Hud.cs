using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;

public class Hud : Singleton<Hud>
{
    public Transform industryMeter;
    public Transform natureMeter;
    public Transform totalMeter;
    public Transform boosterMeter;

    public TMP_Text calculateForDayText;

    public TMP_Text energyText;

    public TMP_Text goldText;

    public TMP_Text dayText;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        EventPool.OptIn("TurnChanged",UpdateText);
        EventPool.OptIn("EnergyChanged",UpdateText);
        EventPool.OptIn("HandUpdate",UpdateText);
        EventPool.OptIn("SelectCardFinished",UpdateText);
        EventPool.OptIn("DrawHand",UpdateText);
        EventPool.OptIn("CurrentTotalValueChanged",UpdateText);
        EventPool.OptIn("DayChanged",UpdateText);
        
         
    }

    public void UpdateText()
    {
        calculateForDayText.text = $"Turn {GameManager.Instance.Turn}/{GameManager.Instance.turnInDay-1}";
        energyText.text = $"{GameManager.Instance.Energy}";
        goldText.text = $"{GameManager.Instance.CurrentTotalValue} <sprite name=\"Money\">";
        dayText.text = $"Year {GameManager.Instance.Day}";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
