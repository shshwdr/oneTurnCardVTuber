using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pool;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private FMOD.Studio.EventInstance musicGameplay;
    public FMODUnity.EventReference musicEvent;

    private void Awake()
    {
        CSVLoader.Instance.Init();
        HandManager.Instance.Init();
        HandManager.Instance.InitDeck();

    }

    public Dictionary<string, int> states = new Dictionary<string, int>();

    private int startEnergy = 4;
    //private int energy;
    public int Energy {
        get => HandManager.Instance.deck.Count;
        // set
        // {
        //     Debug.LogError("Energy can't be set");
        //     // energy = value;
        //     // EventPool.Trigger("EnergyChanged");
        // }
    }

    public bool hasEnoughEnergy(int e)
    {
        return Energy >= e;
    }

    public void ConsumeEnergy(int e)
    {
        HandManager.Instance.DrawCard(e);
        //Energy -= e;
    }

    public void ResetEnergy()
    {
        // startEnergy = CSVLoader.Instance.miscellaneousInfoDict["startEnergy"].intValue;
        // if (ItemManager.Instance.buffManager.hasBuff("addEnergy"))
        // {
        //     startEnergy += 1;
        //
        // }
        // Energy = startEnergy;
    }

    public void DoubleBoost()
    {
        foreach (var key in states.Keys.ToList())
        {
             states[key] *= 2;
        }
        EventPool.Trigger("StateChanged");
    }
    public void AddState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;
        }
        else
        {
            states[key] = value;
        }
        EventPool.Trigger("StateChanged");
    }

    // public void AddEnergy(int value)
    // {
    //     Energy += value;
    //     EventPool.Trigger("EnergyChanged");
    // }

    public int GetState(string key)
    {
        if (states.ContainsKey(key))
        {
            return states[key];
        }
        return 0;
    }
    public int Turn
    {
        get => turn;
        set
        {
            turn = value;
            if (turnInDay == turn)
            {
                Day++;
                turn = 1;
                GameRoundManager.Instance.Next();
            }

            ResetEnergy();
            EventPool.Trigger("TurnChanged");
        }
    }

    public int turnInDay = 3;
    private int turn = 1;

    private int day = 1;
    public int Day
    {
        get => day;
        set
        {
            day = value;
            EventPool.Trigger("DayChanged");
        }
    }

    private int baseValue = 1;
    private int multiplyValue = 1;
    private int currentTotalValue = 0;
    private int targetValue = 200;

    private List<int> targets = new List<int>()
    {
        1000, 1200, 1500, 2000, 3000, 5000
    };

    private int targetIndex = 0;
    public int TargetValue {
        get
        {
            var res = targets[targetIndex];
            while (currentTotalValue >= res && targetIndex < targets.Count - 1)
            {
                targetIndex++;
                res = targets[targetIndex];
            }
            return targets[targetIndex];
        }
}
    public string TargetLevel =>targetIndex==0?"":"Lv."+targetIndex;

    public int boost = 0;

    public void calculateBoost(CardInfo info)
    {
        if (cardInfo != null)
        {
            if (info.element1 == cardInfo.element1 || cardInfo.actions.Contains("clearLastCard") || info.actions.Contains("clearLastCard")) //|| info.identifier == cardInfo.identifier)// || info.element2 == element1 ||
                // info.element2 == element2)
            {
                boost++;
            }
            else
            {
                boost = 0;
            }
        }
        else
        {
           // boost++;
        }
    }
    
    public void Calculate(CardInfo info)
    {
        BaseValue+=boost;
        var value = BaseValue * MultiplyValue;
        currentTotalValue += value;
        EventPool.Trigger("Calculate");
        StartCoroutine(afterCalculate(info));
    }

    // public int element1 = -1;
    // public int element2 = -1;
    public CardInfo cardInfo;

    public void ClearLastCard()
    {
        cardInfo = null;
    }
    public void updateElement(CardInfo info)
    {

        cardInfo = info;
        // element1 = info.element1;
        // element2 = info.element2;
    }
    IEnumerator afterCalculate(CardInfo info)
    {
        updateElement(info);
        yield return new WaitForSeconds(1f);

        // if (cardInfo != null)
        // {
        //     if (info.element1 == cardInfo.element1 )//|| info.identifier == cardInfo.identifier)// || info.element2 == element1 ||
        //        // info.element2 == element2)
        //     {
        //         BaseValue += 1;
        //     }
        //     else
        //     {
        //         // BaseValue = 1;
        //         // MultiplyValue = 1;
        //     }
        // }

        //BaseValue = half(BaseValue);
        //MultiplyValue = half(MultiplyValue);
        EventPool.Trigger("AfterCalculate");
    }

    int half(int v)
    {
        var vv = (float)v / 2;
        return (int) math.ceil(vv);
    }
    
    public int BaseValue
    {
        get => baseValue;
        set
        {
            // if(value!=baseValue)
            // DamageNumbersManager.Instance.ShowResourceCollection(Hud.Instance.industryMeter, value-baseValue, DamageNumberType.industry);
            var diff = value - baseValue;
            // if (diff < 0)
            // {
            //     if (DisasterManager.Instance.buffManager.hasBuff("doubleLose"))
            //     {
            //         
            //         EventPool.Trigger<string>("DisasterTrigger","doubleLose");
            //         diff *= 2;
            //         FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster_loss");
            //     }
            //     if (ItemManager.Instance.buffManager.GetBuffValue("industryLoseAddIndustryMan") > 0)
            //     {
            //         EventPool.Trigger<string>("ItemTrigger","industryLoseAddIndustryMan");
            //
            //         GameManager.Instance.AddCharacter("industryMan",  ItemManager.Instance.buffManager.GetBuffValue("industryLoseAddIndustryMan"));
            //         FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster_loss");
            //     }
            // }else if (diff > 0)
            // {
            //     if (DisasterManager.Instance.buffManager.hasBuff("IndustryLoseNature"))
            //     {
            //         
            //         EventPool.Trigger<string>("DisasterTrigger","IndustryLoseNature");
            //         MultiplyValue -= 10;
            //         FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster_loss");
            //     }
            // }
            baseValue += diff;
            EventPool.Trigger("BaseValueChanged");
        }
    }
    public int MultiplyValue
    {
        get => multiplyValue;
        set
        {
            // if(value!=multiplyValue)
            // DamageNumbersManager.Instance.ShowResourceCollection( Hud.Instance.natureMeter, value - multiplyValue, DamageNumberType.nature);
            var diff = value - multiplyValue;
            // if (diff < 0)
            // {
            //     if (DisasterManager.Instance.buffManager.hasBuff("doubleLose"))
            //     {                    EventPool.Trigger<string>("DisasterTrigger","doubleLose");
            //
            //         diff *= 2;
            //         FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster_loss");
            //     }
            //     if (ItemManager.Instance.buffManager.GetBuffValue("natureLoseAddNatureMan") > 0)
            //     {
            //         EventPool.Trigger<string>("ItemTrigger","natureLoseAddNatureMan");
            //
            //         GameManager.Instance.AddCharacter("natureMan",  ItemManager.Instance.buffManager.GetBuffValue("natureLoseAddNatureMan"));
            //         FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster_loss");
            //     }
            // }else if (diff > 0)
            // {
            //     if (DisasterManager.Instance.buffManager.hasBuff("natureLoseIndustry"))
            //     {
            //         EventPool.Trigger<string>("DisasterTrigger","natureLoseIndustry");
            //
            //         BaseValue -= 10;
            //         FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster_loss");
            //     }
            // }
            multiplyValue += diff;
            EventPool.Trigger("MultiplyValueChanged");
        }
    }
    public int CurrentTotalValue
    {
        get => currentTotalValue;
        set
        {
            currentTotalValue = value;
            EventPool.Trigger("CurrentTotalValueChanged");
        }
    }

    private Dictionary<string, int> charactersDict = new Dictionary<string, int>();

    public void AddCharacter(string key, int value)
    {
        if (charactersDict.ContainsKey(key))
        {
            charactersDict[key] += value;
        }
        else
        {
            charactersDict[key] = value;
        }
        
        SceneRenderer.Instance.characterSpawner.SpawnPrefab(key, value);
        EventPool.Trigger("CharacterChanged");
    }

    public int industryManCount => DisasterManager.Instance.buffManager.hasBuff("disableIndustryMan")? GetCharacter("industryMan")/2 : GetCharacter("industryMan");
    public int industryBoost => GetState("boostIndustry");
    public int natureManCount =>  DisasterManager.Instance.buffManager.hasBuff("disableNatureMan")? GetCharacter("natureMan")/2 : GetCharacter("natureMan");
    public int natureBoost => GetState("boostNature");
    public int GetCharacter(string key)
    {
        if (charactersDict.ContainsKey(key))
        {
            return charactersDict[key];
        }
        return 0;
    }

    public void StartNewDay()
    {
        baseValue = 1;
        multiplyValue = 1;
        
        clearBoost();
        
        foreach (var meterView in FindObjectsOfType<MeterView>())
        {
            meterView.UpdateViewForStartOfTurn();
        }
        
        
        EventPool.Trigger("DayChanged");
        //HandsView.Instance.ResetHandAndDrawHand();
        
        ResetEnergy();
    }
    public void clearBoost()
    {
        states.Clear();
        EventPool.Trigger("StateChanged");
    }
    public void InitNewTurn()
    {
        turnInDay = CSVLoader.Instance.miscellaneousInfoDict["turnPerDay"].intValue + 1;
        Turn = 1;
        Day = 1;
        HandsView.Instance.ResetHandAndDrawHand();
        foreach (var meterView in FindObjectsOfType<MeterView>())
        {
            meterView.UpdateViewForStartOfTurn();
        }

        GameRoundManager.Instance.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitNewTurn();

        musicGameplay = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicGameplay.start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                
                BaseValue -= 100;
            }
            else
            {
                BaseValue += 100;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                MultiplyValue -= 100;
            }
            else
            {
                MultiplyValue += 100;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            CurrentTotalValue += 10;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            
            ItemManager.Instance.AddAll();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            
            DisasterManager.Instance.AddAll();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            
            GameManager.Instance.AddCharacter("industryMan", 2);
            GameManager.Instance.AddCharacter("natureMan", 2);
        }
        if (day > 2)
        {
            MusicDisaster();
        }
            
    }

    public void RestartGame()
    {
        GameRoundManager.Instance.reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        musicGameplay.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void MusicDisaster()
    {
        musicGameplay.setParameterByName("Player Status", 1);
    }

    public void MusicDisasterClean()
    {
        musicGameplay.setParameterByName("Player Status", 0);
    }
}
