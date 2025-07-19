using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundUpdateView : MonoBehaviour
{
    public Image industryImage;
    public Image natureImage;
    
    // Start is called before the first frame update
    void Start()
    {
        
        EventPool.OptIn<int, bool>("meterUpdate",MeterUpdate);
    }

    void MeterUpdate(int index, bool isIndustry)
    {
        var images = isIndustry ? industryImage : natureImage;
       
        if (index>=0)
        {
            images.sprite = Resources.Load<Sprite>("bk/" + (isIndustry ? "industry" : "nature" )+ (index+1));
            images.gameObject.SetActive(true);
        }else 
        {
            images.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
