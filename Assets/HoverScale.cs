using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class HoverScale : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    private Vector3 normalScale;
    [SerializeField] private float hoveringScale = 1.1f;
    [SerializeField] private float scalingTime = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        normalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IncreaseScale(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        IncreaseScale(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IncreaseScale(false);
    }

    private void IncreaseScale(bool status)
    {
        Vector3 finalScale = normalScale;
 
        //If status is true increase scale
        if (status)
            finalScale = normalScale * hoveringScale;
 
        //transform.localScale = finalScale;
        transform.DOScale(finalScale, scalingTime);
    }

    
}
