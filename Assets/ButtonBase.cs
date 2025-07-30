using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_click");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
