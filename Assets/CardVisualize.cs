using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;

public class CardVisualize : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    public bool isInShop = false;

    public bool isDraggable = true;
    public TMP_Text text;
    public TMP_Text desc;
    
    [SerializeField]
    public Image cardBK;
    public TMP_Text energy;
    public TMP_Text cost;
    public TMP_Text type;
    
    //private GameObject selectionCircle;
    //public GameObject selectionCirclePrefab;


    public Transform hoverTrans;
    public float hoverTime = 0.15f;
    Vector3 startPos;
    private Vector3 hoverPos;

    public CardInfo cardInfo;
    public bool setPosition;
    public GameObject disable;
    public void Init(CardInfo info)
    {
        cardInfo = info;
        text.text = cardInfo.title;
        desc.text = cardInfo.desc;
        energy.text = cardInfo.energy.ToString();
        //cost.text = cardInfo.cost.ToString();
        type.text = cardInfo.types!=null &&  cardInfo.types.Count > 0 ? cardInfo.types[0] : "";
        cardBK.sprite = Resources.Load<Sprite>("Card/" + cardInfo.cardBK);
        if (canUseCard())
        {
            disable.SetActive(false);
            
        }
        else
        {
            disable.SetActive(true);
        }
    }
    
    public void InitItem(ItemInfo info)
    {
        //cardInfo = info;
        text.text = info.title;
        desc.text = info.desc;
        energy.text = "";
        //cost.text = cardInfo.cost.ToString();
        type.text = "";

        // if (canUseCard())
        // {
        //     disable.SetActive(false);
        //     
        // }
        // else
        // {
        //     disable.SetActive(true);
        // }
    }

    public bool canUseCard()
    {
        return GameManager.Instance.hasEnoughEnergy(cardInfo.energy);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(setposition());
    }

    IEnumerator setposition()
    {
        yield return new WaitForSeconds(0.01f);
        setPosition = true;
        startPos = transform.position;
        hoverPos = hoverTrans.position;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        
        OnPlace();
        
        // if (!isDraggable)
        // {
        //     return;
        // }

        // if (selectionCircle == null)
        // {
        //     
        //     selectionCircle = Instantiate(selectionCirclePrefab);
        // }

        //var radius = float.Parse(cardInfo.actions[1]);
        //selectionCircle.transform.localScale = Vector3.one * radius;
        //selectionCircle.SetActive(true);
        //PlayerControllerManager.Instance.StartDragging(selectionCircle,this);
    }

    public void SetInShop()
    {
        isInShop = true;
        GetComponent<Image>().enabled = false;
    }
    public void OnPlace()
    {
        if (isInShop)
        {
            return;
        }
        if (!GameManager.Instance.hasEnoughEnergy(cardInfo.energy))
        {
            return;
        }

        GameManager.Instance.ConsumeEnergy(cardInfo.energy);
        //FindObjectOfType<TutorialMenu>().StartCoroutine( FindObjectOfType<TutorialMenu>().FinishUseCard());
       // Debug.LogError("place");
        // Collider2D[] results = new Collider2D[20]; // 假设最多检测 10 个碰撞体
        //
        // // 检测重叠
        // ContactFilter2D contactFilter = new ContactFilter2D();
        // contactFilter.useTriggers = true;  // 允许触发器参与检测
       // int count = selectionCircle.GetComponent<Collider2D>().OverlapCollider(contactFilter, results);
        //sort by result's distance to selectionCicle
        // results = results.Where(x => x != null)
        //     .OrderBy(x => Vector3.Distance(x.transform.position, selectionCircle.transform.position)).ToArray();
        HandManager.Instance.useCard(cardInfo);

        if (ItemManager.Instance.buffManager.hasBuff("lastCardTwice") && HandManager.Instance.handInBattle.Count == 0)
        {
            EventPool.Trigger<string>("ItemTrigger","lastCardTwice");

            HandManager.Instance.DoCardAction(cardInfo);
        }
        
        if (ItemManager.Instance.buffManager.hasBuff("drawWhenEmpty") && HandManager.Instance.handInBattle.Count == 0)
        {            
            EventPool.Trigger<string>("ItemTrigger","drawWhenEmpty");

            HandManager.Instance.DrawCard(1);
        }
        
        bool foundTarget = false;
        

        
        
        
        //selectionCircle.SetActive(false);
        ExitCard();
        //Destroy(gameObject);
    }

    public void Cancel()
    {
        
        
        //selectionCircle.SetActive(false);
        ExitCard();
    }

    public bool OnDrag()
    {
        return true;
    }

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        var str = "";
        if (cardInfo.desc.ToLower().Contains("scientist"))
        {
            str +=$"Scientist: +{CSVLoader.Instance.miscellaneousInfoDict["valueAddPerMan"].intValue} <sprite name=\"Industry\"> per scientist when a card adds <sprite name=\"Industry\">\n";
        }
        if (cardInfo.desc.ToLower().Contains("activist"))
        {
            str += $"Activist: +{CSVLoader.Instance.miscellaneousInfoDict["valueAddPerMan"].intValue} <sprite name=\"Nature\"> per Activist when a card adds <sprite name=\"Nature\">\n";
        }
        if (cardInfo.desc.ToLower().Contains("boost"))
        {
            str += $"Boost: Multiply the effect of Scientists or Activists\n";
        }

        if (str != "")
        {
            DescView.Instance.Show(str);
        }
        if (isInShop)
        {
            return;
        }
        if (!setPosition|| !isDraggable)
        {
            return;
        }

        MoveCardUp(true);
        //transform.position = hoverPos;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DescView.Instance.Hide();
        if (!setPosition || !isDraggable)
        {
            return;
        }

        if (PlayerControllerManager.Instance.currentDraggingCell == this)
        {
            
        }
        else
        {
            ExitCard();
        }
        
    }

    public void ExitCard()
    {
        if (this && transform)
        {
            MoveCardUp(false);
            //transform.position = startPos;
        }
    }

    private void MoveCardUp(bool status)
    {
        Vector3 finalPos = startPos;
 
        //If status is true increase scale
        if (status)
            finalPos = hoverPos;
 
        //transform.localScale = finalScale;
        transform.DOMove(finalPos, hoverTime);
    }
}
