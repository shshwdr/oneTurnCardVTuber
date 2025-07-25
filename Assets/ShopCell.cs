using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCell : MonoBehaviour
{
    public CardVisualize cardVisualize;
    public Button buyButton;
    public EffectIcon itemIcon;

    
    public void Init(CardInfo info)
    {
        info.element1 = Random.Range(0, ElementManager.Instance.sprites.Count);
        cardVisualize.Init(info);

        buyButton.GetComponentInChildren<TMP_Text>().text = info.cost+$"<sprite name=\"Money\">";
        buyButton.interactable = GameManager.Instance.Gold >= info.cost;
        UpdateCell();
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            FindObjectOfType<ShopMenu>().purchased.Add(info.identifier);
            HandManager.Instance.AddCard(info,false);
            GameManager.Instance.Gold -= info.cost;
            FindObjectOfType<ShopMenu>().UpdateMenu();
            //FindObjectOfType<ShopMenu>().Hide();
            //GameManager.Instance.Next();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_buy_joker");
            
        });
        
    }

    public void UpdateCell()
    {
        
        if (itemIcon)
        {

            if (FindObjectOfType<ShopMenu>().purchased.Contains(itemIcon.info.identifier))
            {
                buyButton.interactable = false;
                buyButton.GetComponentInChildren<TMP_Text>().text = "Sold Out!";
            }
            else
            {
                var interactable = GameManager.Instance.Gold >= ((ItemInfo)(itemIcon.info)).cost;
                buyButton.interactable = interactable;
                buyButton.GetComponentInChildren<TMP_Text>().color = interactable?Color.black:Color.red;
            }
        }
        else
        {
            if (FindObjectOfType<ShopMenu>().purchased.Contains(cardVisualize.cardInfo.identifier))
            {
                buyButton.interactable = false;
                buyButton.GetComponentInChildren<TMP_Text>().text = "Sold Out!";
            }
            else
            {
                
                var interactable = GameManager.Instance.Gold >= cardVisualize.cardInfo.cost;
                buyButton.interactable = interactable;
                buyButton.GetComponentInChildren<TMP_Text>().color = interactable?Color.black:Color.red;
            }
        }
    }
    public void InitItem(ItemInfo info)
    {
        itemIcon.Init(info);
        itemIcon.image.sprite =
            Resources.Load<Sprite>("item/" + info.image);
        UpdateCell();
        buyButton.GetComponentInChildren<TMP_Text>().text = info.cost+$"<sprite name=\"Money\">";
        buyButton.interactable = GameManager.Instance.Gold >= info.cost;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            FindObjectOfType<ShopMenu>().purchased.Add(info.identifier);
            ItemManager.Instance.AddItem(info);
            GameManager.Instance.Gold -= info.cost;
            //GameManager.Instance.Next();

            FindObjectOfType<ShopMenu>().UpdateMenu();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_buy_joker");
        });
    }
}
