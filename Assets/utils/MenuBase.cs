using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuBase : MonoBehaviour
{
    public GameObject menu;
    protected Image blockImage;
    public bool IsActive => menu.activeInHierarchy;
    public Button closeButton;
    protected bool destroyWhenHide = false;
    public virtual void UpdateMenu()
    {
        
    }
    
    
    protected virtual void Awake()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        menu.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        blockImage = menu.GetComponent<Image>();
        if (blockImage)
        {
            
            var color = menu.GetComponent<Image>().color;
            color.a = 0.85f;
            menu.GetComponent<Image>().color = color;
        }

        if (closeButton)
        {
            closeButton.onClick.AddListener(() =>
            {
                Hide();
            });
        }
        
        targetTrans = transform.position;
        targetSizeDelta = transform.localScale.x;
        Hide(true);
    }

    public virtual void tryInteract()
    {
        
    }
    public static T FindFirstInstance<T>() where T : MenuBase
    {
        T instance = FindObjectOfType<T>();
        if (instance == null)
        {
            Debug.LogWarning($"No instance of {typeof(T).Name} found in the scene.");
        }
        return instance;
    }
    public static void OpenMenu<T>() where T : MenuBase
    {
        var instance = FindFirstInstance<T>();
        if (instance != null)
        {
            instance.Show();
        }
    }
    protected virtual void Start()
    {
    }

    virtual public void Init()
    {
    }

    public RectTransform animatedRect;
    protected Vector3 startTrans;
    protected Vector3 targetTrans;
    protected float targetSizeDelta;
    private float hideTime = 0.3f;
    public  float showTime = 0.5f;
    virtual public void ShowAnim(bool immediate = false)
    {
        if (immediate)
        {
            animatedRect.localScale =Vector3.one * targetSizeDelta;
            animatedRect.position = targetTrans;
            return;
        }
        
        //animatedRect.position = startTrans;
        animatedRect.localScale =Vector3.zero;
        
        animatedRect.DOMove(targetTrans, showTime).SetEase( Ease.OutQuart).SetUpdate(true);

        // 缩放到目标大小
        animatedRect.DOScale(targetSizeDelta, showTime).SetEase( Ease.OutQuart).SetUpdate(true);
    }

    virtual public void Hide(bool immediate = false)
    {
        if (immediate)
        {
            menu.SetActive(false);
            if (destroyWhenHide)
            {
                Destroy(gameObject);
            }
            return;
        }
        
        if (animatedRect != null)
        {

            animatedRect.DOKill();
           // animatedRect.DOMove(startTrans, hideTime).SetEase(Ease.OutQuad);

            // 缩放到目标大小
            animatedRect.DOScale(0, hideTime).SetEase(Ease.OutQuad).SetUpdate(true);
            StartCoroutine(FullyHide());
        }
        else
        {
            menu.SetActive(false);
            if (destroyWhenHide)
            {
                Destroy(gameObject);
            }
            return;
            
        }
        
        
    }

    virtual public IEnumerator FullyHide()
    {
         yield return new WaitForSecondsRealtime(hideTime);
         
         menu.SetActive(false);
         if (destroyWhenHide)
         {
             Destroy(gameObject);
         }
    }
    virtual public void Show(bool immediate = false)
    {
        DescView.Instance.Hide();
        if (animatedRect != null)
        {
            ShowAnim(immediate);
        }
        
        menu.SetActive(true);
    }
    // virtual public void Hide()
    // {
    //     menu.SetActive(false);
    //
    // }
}

