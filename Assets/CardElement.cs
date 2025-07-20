using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardElement : MonoBehaviour
{

    public int element1;
    public int element2;
    public Image element1Img;
    public Image element2Img;
    // Start is called before the first frame update
    public void Init()
    {
        List<int> elements = new List<int>();
        for (int i = 0; i < ElementManager.Instance.sprites.Count; i++)
        {
            elements.Add(i);
        }

        element1 = elements.PickItem();
        element2 = elements.PickItem();
        // element1Img.sprite = ElementManager.Instance.sprites[element1];
        // element2Img.sprite = ElementManager.Instance.sprites[element2];
    }

    public void UpdateView(CardInfo info)
    {
        element1Img.sprite = ElementManager.Instance.sprites[info.element1];
        element2Img.sprite = ElementManager.Instance.sprites[info.element2];

        if (GameManager.Instance.element1 == -1 || info.element1 == GameManager.Instance.element1 || info.element1 == GameManager.Instance.element2 || !info.actions.Contains("attack"))
        {
            element1Img.color = Color.white;
        }
        else
        {
            element1Img.color = Color.gray;
        }
        if (GameManager.Instance.element1 == -1 || info.element2 == GameManager.Instance.element1 || info.element2 == GameManager.Instance.element2 || !info.actions.Contains("attack"))
        {
            element2Img.color = Color.white;
        }
        else
        {
            element2Img.color = Color.gray;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
