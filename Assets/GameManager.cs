using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pool;
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
    private int energy;
    public int Energy {
        get => energy;
        set
        {
            energy = value;
            EventPool.Trigger("EnergyChanged");
        }
    }

    public bool hasEnoughEnergy(int e)
    {
        return energy >= e;
    }

    public void ConsumeEnergy(int e)
    {
        Energy -= e;
    }

    public void ResetEnergy()
    {
        startEnergy = CSVLoader.Instance.miscellaneousInfoDict["startEnergy"].intValue;
        if (ItemManager.Instance.buffManager.hasBuff("addEnergy"))
        {
            startEnergy += 1;

        }
        Energy = startEnergy;
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

    public void AddEnergy(int value)
    {
        Energy += value;
        EventPool.Trigger("EnergyChanged");
    }

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

    private int industry;
    private int nature;
    private int gold;

    public int Industry
    {
        get => industry;
        set
        {
            if(value!=industry)
            DamageNumbersManager.Instance.ShowResourceCollection(Hud.Instance.industryMeter, value-industry, DamageNumberType.industry);
            var diff = value - industry;
            if (diff < 0)
            {
                if (DisasterManager.Instance.buffManager.hasBuff("doubleLose"))
                {
                    
                    EventPool.Trigger<string>("DisasterTrigger","doubleLose");
                    diff *= 2;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster_loss");
                }
                if (ItemManager.Instance.buffManager.GetBuffValue("industryLoseAddIndustryMan") > 0)
                {
                    EventPool.Trigger<string>("ItemTrigger","industryLoseAddIndustryMan");

                    GameManager.Instance.AddCharacter("industryMan",  ItemManager.Instance.buffManager.GetBuffValue("industryLoseAddIndustryMan"));
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster_loss");
                }
            }else if (diff > 0)
            {
                if (DisasterManager.Instance.buffManager.hasBuff("IndustryLoseNature"))
                {
                    
                    EventPool.Trigger<string>("DisasterTrigger","IndustryLoseNature");
                    Nature -= 10;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster_loss");
                }
            }
            industry += diff;
            EventPool.Trigger("IndustryChanged");
        }
    }
    public int Nature
    {
        get => nature;
        set
        {
            if(value!=nature)
            DamageNumbersManager.Instance.ShowResourceCollection( Hud.Instance.natureMeter, value - nature, DamageNumberType.nature);
            var diff = value - nature;
            if (diff < 0)
            {
                if (DisasterManager.Instance.buffManager.hasBuff("doubleLose"))
                {                    EventPool.Trigger<string>("DisasterTrigger","doubleLose");

                    diff *= 2;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster_loss");
                }
                if (ItemManager.Instance.buffManager.GetBuffValue("natureLoseAddNatureMan") > 0)
                {
                    EventPool.Trigger<string>("ItemTrigger","natureLoseAddNatureMan");

                    GameManager.Instance.AddCharacter("natureMan",  ItemManager.Instance.buffManager.GetBuffValue("natureLoseAddNatureMan"));
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster_loss");
                }
            }else if (diff > 0)
            {
                if (DisasterManager.Instance.buffManager.hasBuff("natureLoseIndustry"))
                {
                    EventPool.Trigger<string>("DisasterTrigger","natureLoseIndustry");

                    Industry -= 10;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_disaster_loss");
                }
            }
            nature += diff;
            EventPool.Trigger("NatureChanged");
        }
    }
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            EventPool.Trigger("GoldChanged");
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
        industry = 0;
        nature = 0;
        
        clearBoost();
        
        foreach (var meterView in FindObjectsOfType<MeterView>())
        {
            meterView.UpdateViewForStartOfTurn();
        }
        
        
        EventPool.Trigger("DayChanged");
        HandsView.Instance.ResetHandAndDrawHand();
        
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
                
                Industry -= 100;
            }
            else
            {
                Industry += 100;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                Nature -= 100;
            }
            else
            {
                Nature += 100;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Gold += 10;
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
