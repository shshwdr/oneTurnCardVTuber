using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TutorialPage : MonoBehaviour
{

    public GameObject tapToContinue;
    public GameObject nextOb;
    public List<string> moveUpObjsString;
    private List<Canvas> canvases = new List<Canvas>();
    public bool showNextOb = true;
    public string otherNextObString;
    public float showNextObTime = 0.5f;

    public float autoPass = -1;
    public GameObject finger;
    public bool updateFingerPosition = false;
    public UnityEvent  extraAction;

    private TutorialMenu menu;
    public Vector3 fingerOffset = new Vector2(0, 0);
    public string fingerTarget;
    public bool destoryCanv = true;
    public bool hasDialogue = false;
    public string specialBeforeShow;
    public RectTransform clickableRect;
    public void Init(TutorialMenu menu,int index)
    {
        this.menu = menu;
        
        

        var dialogue = GetComponentInChildren<DialogueCell>(true);
        if (hasDialogue)
        {
            
            dialogue.gameObject.SetActive(true);
            dialogue.GetComponent<RectTransform>().localScale =Vector3.zero;
            dialogue.GetComponent<RectTransform>().DOScale(1, 0.5f).SetEase( Ease.OutBack);

            dialogue.GetComponent<CanvasGroup>().alpha = 0;
            dialogue.GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetEase( Ease.InQuad);
            
            
            // if (menu.tutorialKey.Length > 0 )
            // {
            //     var term = menu.tutorialKey + "_" + index;
            //     dialogue.GetComponent<DialogueCell>().text.text = term;
            //     
            // }
            // else
            // {
            //     Debug.LogError("tutorial key is empty");
            // }
        }
        
        // a hack way
        // var mask = transform.Find("mask");
        // if (mask)
        // {
        //     mask.GetComponent<RectTransform>().sizeDelta = new Vector2(
        //         mask.GetComponent<RectTransform>().sizeDelta.x + 20f, // 增加宽度
        //         mask.GetComponent<RectTransform>().sizeDelta.y + 20f  // 增加高度
        //     );
        // }
    }

    public bool IsPointerInsideSpecialRect()
    {
        if (!clickableRect)
        {
            return false;
        }
        // 获取当前鼠标或触摸点的屏幕坐标
        Vector2 pointerPos = Input.mousePosition;
        Vector2 localPoint;
        // 使用 null 表示屏幕空间 Overlay 模式，如果是 ScreenSpace - Camera 模式，请传入对应的摄像机
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(clickableRect, pointerPos, Camera.main, out localPoint))
        {
            return clickableRect.rect.Contains(localPoint);
        }

        //mineral2
        return false;
    }
    IEnumerator specialActionEnumerator()
    {
        yield return null;

        if (specialBeforeShow == "AddCardOb")
        {
            
            var card = FindObjectOfType<CardVisualize>();
            card.energy.transform.parent.gameObject.AddComponent<QuickAccessObject>().SetName("CardEnergy");
            card.gameObject.gameObject.AddComponent<QuickAccessObject>().SetName("CardElement");
        }
    }
    private void Start()
    {

        if (specialBeforeShow.Length > 0)
        {
            StartCoroutine(specialActionEnumerator());
        }

        StartCoroutine((showNext()));
        extraAction?.Invoke();
        
        foreach (var str in moveUpObjsString)
        {
            var obj = SceneMenuManager.Instance.GetQuickAccessObject(str);
            if (obj == null)
            {
                 Debug.LogError($"quickAccessObject not found: {str}");
                 menu.FinishTutorial();
            }
            var canv = obj.GetComponent<Canvas>();
            if (obj.GetComponent<Canvas>())
            {
                //Debug.LogError( "already has canvas" );
            }
            else
            {
                canv = obj.AddComponent<Canvas>();
            }
            //if (otherNextObString == str)
            {
                obj.AddComponent<GraphicRaycaster>();
            }
            if (obj.GetComponentInChildren<Button>())
            {
                
                obj.GetComponentInChildren<Button>().onClick.AddListener(gotoNextPage);
            }
            canv.overrideSorting = true;
            canv.sortingOrder = 10000;
            canv.sortingLayerName = "ui";
            canvases.Add(canv);
            foreach (TMP_Text tex in obj.GetComponentsInChildren<TMP_Text>())
            {
                tex.gameObject.SetActive(false);
                tex.ForceMeshUpdate();
                tex.gameObject.SetActive(true); 
            }

        }

        if (finger && fingerTarget.Length>0)
        {
            var obj = SceneMenuManager.Instance.GetQuickAccessObject(fingerTarget);
            MatchUIPosition(obj.GetComponent<RectTransform>(),finger.GetComponent<RectTransform>(),fingerOffset);
           // finger.transform.position = obj.transform.position + fingerOffset;
        }

        
    }
    public static void MatchUIPosition(RectTransform targetPosition, RectTransform moveTrans,Vector2 fingerOffset)
    {
        // Vector3 worldPos = targetPosition.position;
        // Vector3 localPos = moveTrans.parent.InverseTransformPoint(worldPos);
        // moveTrans.localPosition = localPos +fingerOffset;
        
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPosition.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)moveTrans.parent,
            screenPos,
            Camera.main,
            out Vector2 localPoint
        );
        moveTrans.localPosition = localPoint+fingerOffset;
    }

    IEnumerator showNext()
    {
        if (autoPass > 0)
        {
            yield return  new WaitForSeconds(autoPass);
            gotoNextPage();
            yield break;
        }
        if (showNextOb && nextOb)
        {
            nextOb.SetActive(true);
            if (nextOb.GetComponent<Button>())
            {
                nextOb.GetComponent<Button>().enabled = false;
            }
            yield return new WaitForSeconds(showNextObTime);
            
            if (nextOb.GetComponent<Button>())
            {
                nextOb.GetComponent<Button>().enabled = true;
            }
            if (tapToContinue)
            {
            
                tapToContinue.SetActive(true);
            }
        }
        else
        {
            if (tapToContinue)
            {
            
                tapToContinue.SetActive(false);
            }
        }
        
        
        if (otherNextObString.Length>0)
        {
            var obj = SceneMenuManager.Instance.GetQuickAccessObject(otherNextObString);
            // if ( obj.GetComponent<GoDiceView>())
            // {
            //     obj.GetComponent<GoDiceView>().extraPressDownAction = gotoNextPage;
            // }
            // else
            {
                obj.GetComponentInChildren<Button>().onClick.AddListener(gotoNextPage);
            }
        }
        
        
    }
    public void gotoNextPage()
    {
        if (gameObject.activeSelf)
        {
            
            StartCoroutine((GoToNextPageIenumerator()));
        }

    }


    IEnumerator GoToNextPageIenumerator()
    {
        if (finger)
        {
            finger.gameObject.SetActive(false);
        }
        
        foreach (var canv in canvases)
        {
            if (canv)
            {
                // if (canv.GetComponent<GraphicRaycaster>())
                // {
                //     Destroy(canv.GetComponent<GraphicRaycaster>());
                // }
                //
                // if (destoryCanv)
                // {
                //     Destroy(canv);
                // }
                canv.overrideSorting = false;
            }
        }
        
        
        var dialogue = GetComponentInChildren<DialogueCell>();
        if (dialogue)
        {
            //dialogue.GetComponent<RectTransform>().localScale =Vector3.zero;
            dialogue.GetComponent<RectTransform>().DOScale(0, 0.3f).SetEase( Ease.OutQuad);

                //dialogue.GetComponent<CanvasGroup>().alpha = 0;
            dialogue.GetComponent<CanvasGroup>().DOFade(0, 0.3f).SetEase( Ease.OutQuad);
            
            yield return  new WaitForSeconds(0.3f);
        }
        
        
        yield return  new WaitForSeconds(0.1f);

        if (otherNextObString.Length>0)
        {
            var obj = SceneMenuManager.Instance.GetQuickAccessObject(otherNextObString);
            obj.GetComponentInChildren<Button>(true).onClick.RemoveListener(gotoNextPage);
        }
        menu.gotoNextPage();
    }

    private void Update()
    {
        
        if (finger && fingerTarget.Length>0 &&updateFingerPosition)
        {
            var obj = SceneMenuManager.Instance.GetQuickAccessObject(fingerTarget);
           // MatchUIPosition(obj.GetComponent<RectTransform>(),finger.GetComponent<RectTransform>(),fingerOffset);
             finger.transform.position = obj.transform.position + fingerOffset;
        }
    }
    // public void Init(string moveUpString,int page = 0)
    // {
    //     gameObject.SetActive(true);
    //     moveUpObjsString.Clear();
    //     moveUpObjsString.Add(moveUpString);
    //     
    //     extraAction?.Invoke();
    //     foreach (var str in moveUpObjsString)
    //     {
    //         var obj = SceneMenuManager.Instance.GetQuickAccessObject(str);
    //         var canv = obj.GetComponent<Canvas>();
    //         if (obj.GetComponent<Canvas>())
    //         {
    //             //Debug.LogError( "already has canvas" );
    //         }
    //         else
    //         {
    //             canv = obj.AddComponent<Canvas>();
    //         }
    //         //if (otherNextObString == str)
    //         {
    //             obj.AddComponent<GraphicRaycaster>();
    //         }
    //         obj.GetComponentInChildren<Button>().onClick.AddListener(gotoNextPage);
    //         canv.overrideSorting = true;
    //         canv.sortingOrder = 10000;
    //         canv.sortingLayerName = "UI";
    //         canvases.Add(canv);
    //         finger.transform.position = obj.transform.position + fingerOffset;
    //         foreach (TMP_Text tex in obj.GetComponentsInChildren<TMP_Text>())
    //         {
    //             tex.gameObject.SetActive(false);
    //             tex.ForceMeshUpdate();
    //             tex.gameObject.SetActive(true); 
    //         }
    //
    //     }
    //
    //     
    // }
    //
    //
    //
    // public void gotoNextPage()
    // {
    //
    //     if (gameObject.activeSelf)
    //     {
    //         
    //         StartCoroutine((GoToNextPageIenumerator()));
    //
    //     }
    // }
    //
    //
    // IEnumerator GoToNextPageIenumerator()
    // {
    //     yield return null;
    //     foreach (var canv in canvases)
    //     {
    //         if (canv)
    //         {
    //             if (canv.GetComponent<GraphicRaycaster>())
    //             {
    //                 Destroy(canv.GetComponent<GraphicRaycaster>());
    //             }
    //
    //             if (destoryCanv)
    //             {
    //                 Destroy(canv);
    //             }
    //         }
    //     }
    //
    //     //if (otherNextObString.Length>0)
    //     {
    //         var obj = SceneMenuManager.Instance.GetQuickAccessObject(otherNextObString);
    //         obj.GetComponentInChildren<Button>(true).onClick.RemoveListener(gotoNextPage);
    //     }
    //     gameObject.SetActive(false);
    // }
}
