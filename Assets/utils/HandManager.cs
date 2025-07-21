using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Live2D.Cubism.Framework.Motion;
using Pool;
using UnityEngine;

public class HandManager : Singleton<HandManager>
{
    public List<CardInfo> ownedCards = new List<CardInfo>();
    
    public List<CardInfo> deck = new List<CardInfo>();
    public List<CardInfo> handInBattle = new List<CardInfo>();
    public List<CardInfo> discardedInBattle = new List<CardInfo>();
    
    private int handMax = 4;

    private int placedCount = 0;
    public void InitDeck()
    {
        deck = ownedCards.ToList();
    }

    public void useCard(CardInfo info)
    {
        placedCount++;
        handInBattle.Remove(info);
        if (info.exhaust)
        {
            
        }
        else
        {
            discardedInBattle.Add(info);
        }
        EventPool.Trigger("DrawHand");
        
        DoCardAction(info);
        
        
        if (DisasterManager.Instance.buffManager.GetBuffValue("endTurn") > 0 &&
            placedCount >= DisasterManager.Instance.buffManager.GetBuffValue("endTurn"))
        {
            EventPool.Trigger<string>("DisasterTrigger","endTurn");
            placedCount = 0;
            HandsView.Instance.EndTurn();
        }
    }

    public CardGameCubism model;
   public void DoCardAction(CardInfo info)
   {
        if (info.motion!=0)
        {
            model.PlayAnim(info.motion);
        }
        else if (info.expression!=0)
        {
            model.PlayExpression(info.expression);
        }
        
        int test = 0;
        for (int i = 0; i < info.actions.Count;i++)
        {
            test++;
            if (test > 100)
            {
                Debug.LogError("DoCardAction infinite loop");
                break;
            }
            string action = info.actions[i];

            if (!info.actions.Contains("attack"))
            {
                
                GameManager.Instance.updateElement(info);
            }

            switch (info.actions[i])
            {
                case "base":
                {
                    i++;
                    int value = int.Parse(info.actions[i]);

                    GameManager.Instance.BaseValue += value;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_industry_card");
                    break;
                }
                case "multiplier":
                {
                    i++;
                    int value = int.Parse(info.actions[i]);

                    GameManager.Instance.MultiplyValue += value;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_industry_card");
                    break;
                }
                case "returnCard":
                {
                    i++;
                    int value = int.Parse(info.actions[i]);
                    HandsView.Instance.ReturnCard(value);
                    break;
                }
                case "clearLastCard":
                {
                    GameManager.Instance.ClearLastCard();
                    break;
                }
                case "addCard":
                {
                    i++;
                    
                    break;
                }
                case "addBuff":
                {
                    i++;
                    
                    string key = info.actions[i];
                    i++;
                    
                    int value = int.Parse(info.actions[i]);
                    foreach (var cardInfo in HandManager.Instance.handInBattle)
                    {
                        cardInfo.addBuff(key,value);
                    }
                    break;
                }

            case "attack":
                {
                    GameManager.Instance.Calculate(info);
                    break;
                }
                
                case "draw":
                {
                    i++;
                    int value = int.Parse(info.actions[i]);
                    HandsView.Instance.DrawCards(value);
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_neutral_card");
                        break;
                }
                case "discard":
                {
                    i++;
                    int value = int.Parse(info.actions[i]);
                    HandsView.Instance.DiscardCards(value);
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_neutral_card");
                        break;
                }
                case "discardHand":
                {
                    HandsView.Instance.DiscardHand();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_neutral_card");
                        break;
                }
                
        }
        }


        if (info.types.Contains("industry"))
        {
            var boostCount = GameManager.Instance.industryBoost;
            if (ItemManager.Instance.buffManager.GetBuffValue("shareBoost")>0)
            {
                
               boostCount += GameManager.Instance.natureBoost;
               if ( GameManager.Instance.natureBoost>0)
               {
                   
                   EventPool.Trigger<string>("ItemTrigger","shareBoost");
               }
            }
            GameManager.Instance.BaseValue += GameManager.Instance.industryManCount * CSVLoader.Instance.miscellaneousInfoDict["valueAddPerMan"].intValue * (1+ boostCount);
            if (GameManager.Instance.industryManCount>0)
            foreach (var animator in SceneRenderer.Instance.characterSpawner.spawnArea.transform.GetComponentsInChildren<Animator>())
            {
                animator.SetTrigger("use");
            }
        }
        if (info.types.Contains("nature"))
        {
            var boostCount = GameManager.Instance.natureBoost;
            if (ItemManager.Instance.buffManager.GetBuffValue("shareBoost")>0)
            {
               boostCount += GameManager.Instance.industryBoost;
               
               if ( GameManager.Instance.industryBoost>0)
               {
                   
                   EventPool.Trigger<string>("ItemTrigger","shareBoost");
               }
            }
            GameManager.Instance.MultiplyValue += GameManager.Instance.natureManCount* CSVLoader.Instance.miscellaneousInfoDict["valueAddPerMan"].intValue * (1+ boostCount);
            
            {if (GameManager.Instance.natureManCount>0)
                
                foreach (var animator in SceneRenderer.Instance.characterSpawner.spawnArea2.transform.GetComponentsInChildren<Animator>())
                {
                    animator.SetTrigger("use");
                }
            }
        }
    }

    public int effectiveNatureBoost()
    {
        
        var boostCount = GameManager.Instance.natureBoost;
        if (ItemManager.Instance.buffManager.GetBuffValue("shareBoost") > 0)
        {
            boostCount += GameManager.Instance.industryBoost;
        }
        return boostCount;
    }

    public int effectiveIndustryBoost()
    {
        
        var boostCount = GameManager.Instance.industryBoost;
        if (ItemManager.Instance.buffManager.GetBuffValue("shareBoost") > 0)
        {
            boostCount += GameManager.Instance.natureBoost;
        }
        return boostCount;
    }
    

    public void DrawSpecificCard(string key,bool fromdeck = true)
    {
        var infect = CSVLoader.Instance.cardDict[key];
        if (fromdeck)
        {
            if(!deck.Contains(infect))
            {
                deck .AddRange( discardedInBattle);
            }
            
            if (deck.Contains(infect))
            {
                deck.Remove(infect);
                handInBattle.Add(infect);
            }
        }
        else
        {
            handInBattle.Add(infect);
        }
        
        EventPool.Trigger("DrawHand");
    }

    void TriggerDiscardEffect()
    {
        
        if (ItemManager.Instance.buffManager.hasBuff("addEnergyWhenDiscard"))
        {
            //GameManager.Instance.Energy += 1;
            EventPool.Trigger<string>("ItemTrigger","addEnergyWhenDiscard");

        }
        
        
        if (ItemManager.Instance.buffManager.hasBuff("discardAndDraw"))
        {
            EventPool.Trigger<string>("ItemTrigger","discardAndDraw");

            DrawCard(1);
        }
        
    }
    public void DiscardCards(int count)
    {
        TriggerDiscardEffect();
        for (int i = 0; i < count; i++)
        {
            
            if (handInBattle.Count == 0)
            {
                break;
            }
            var info = handInBattle.PickItem();
            handInBattle.Remove(info);
            if (info.exhaust)
            {
            
            }
            else
            {
                discardedInBattle.Add(info);
            }
        }
      
        EventPool.Trigger("DrawHand");  
    }

    public void DiscardHand()
    {
        
        TriggerDiscardEffect();
        discardedInBattle.AddRange(handInBattle);
        handInBattle.Clear();
    }
    public void DrawCard(int count)
    {
        //discardedInBattle.AddRange(handInBattle);
        //handInBattle.Clear();
        for (int i = 0; i < count; i++)
        {
            if (deck.Count == 0)
            {
                deck = discardedInBattle;
            }

            if (deck.Count == 0)
            {
                break;
            }

           
            {
                
                handInBattle.Add(deck.PickItem());
            }
        }
        EventPool.Trigger("DrawHand");
    }

    public void ReturnCard(int count)
    {
        for (int i = 0; i < count; i++)
        {
            
            if (handInBattle.Count == 0)
            {
                break;
            }
            var info = handInBattle.PickItem();
            handInBattle.Remove(info);
            // if (info.exhaust)
            // {
            //
            // }
            // else
            // {
            //     discardedInBattle.Add(info);
            // }
            deck.Add(info);
        }
      
        EventPool.Trigger("DrawHand");  
    }

    public void ClearHandAndDrawHand()
    {
        ClearHand();
        DrawHand();
      
    }

    public void ClearHand()
    {
        InitDeck();
        discardedInBattle.Clear();
        handInBattle.Clear();
    }
    public void DrawHand()
    {
        discardedInBattle.AddRange(handInBattle);
        handInBattle.Clear();
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_draw_card");


        for (int i = 0; i < handMax; i++)
        {
            if (deck.Count == 0)
            {
                deck = discardedInBattle.ToList();
                discardedInBattle.Clear();
            }

            if (deck.Count == 0)
            {
                break;
            }

           
            {
                
                handInBattle.Add(deck.PickItem());
            }
        }
        EventPool.Trigger("DrawHand");
    }

    public void ClearBattleHand()
    {
        handInBattle.Clear();
        discardedInBattle.Clear();
        EventPool.Trigger("DrawHand");
    }
    public void AddCard(CardInfo info)
    {
        var copied = info.ShallowCopy();
        copied.element1 = Random.Range(0, ElementManager.Instance.sprites.Count);
        ownedCards.Add(copied);
        EventPool.Trigger("HandUpdate");
    }
    public void Init()
    {
        model = FindObjectOfType<CardGameCubism>();
        ownedCards.Clear();
        handInBattle.Clear();
        discardedInBattle.Clear();
        foreach (var info in CSVLoader.Instance.cardDict.Values)
        {
            for (int i = 0; i < info.start; i++)
            {
                AddCard(info);
            }
            
        }
    }
}
