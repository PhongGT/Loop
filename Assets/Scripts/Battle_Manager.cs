using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    [SerializeField] public List<Character> enemys;
    [SerializeField] public List<Character> player_allys;
    [SerializeField] protected List<LoadCharacter> player_allys_Load;
    [SerializeField] protected List<LoadCharacter> enemys_Load;

    [SerializeField] protected GameObject playerPrefabs;
    public GameObject battlePanel;

    [SerializeField] protected GameObject player_allys_Parent;
    [SerializeField] protected GameObject enemys_Parent;
    public float dayTime = 7f;
    public int loopCount = 1;
    public int dayCount = 1;
    public int healthPotionCount = 2;
    protected bool timerStatState = false;
    public Player player;
    public Ghoul ghoul; 
    public bool startBattle = false;


    public static BattleManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        
    }
    void Start()
    {

        Actions.StartTimer += () => timerStatState = true;
        LoadNewCharacter(player_allys_Load[0], player);
        Actions.StartBattle += StartBattle;
        Actions.EndBattle += () => battlePanel.SetActive(false);
        //StartBattle();
    }


    void Update()
    {
        if (timerStatState)
        {
           dayTime -= Time.deltaTime;  
        }

        if (dayTime <= 0)
        {
            Actions.SpawnMob?.Invoke(dayCount);
            dayTime = 10f;
            dayCount++;
        }
        
    }
    // Input Enemy List
    public void StartBattle()
    {
        LoadTarget();
        startBattle = true;
    }

    public void PreperBattle( List<String> enemys)
    {
        battlePanel.SetActive(true);
        foreach (var Char in enemys)
        {
            LoadCharacter l = returnInactiveSlot(enemys_Load);
            if (l != null)
            {
                LoadNewCharacter(l, ReturnLoadChar(Char));
            }
        }

    }
    public void LoadNewCharacter(LoadCharacter loadSlot, Character character)
    {
        Character Char = character;
        Char.Init();
        loadSlot.Load(Char);
        //loadSlot.gameObject.SetActive(true);
    }
    public void LoadTarget(bool isPlayer)
    {
        if(isPlayer)
        {
            player_allys.Clear();
            foreach (var load in player_allys_Load)
            {
                if (load.isLoaded)
                {
                    player_allys.Add(load.ReturnCharacter);
                }
            }
        }
        else
        {
            enemys.Clear();
            foreach (var load in enemys_Load)
            {
                if (load.isLoaded)
                {
                    enemys.Add(load.ReturnCharacter);
                }
                
            }
        }
    }    
    public void LoadTarget()
    {

            player_allys.Clear();
            foreach (var load in player_allys_Load)
            {
                if (load.isLoaded)
                {
                    Character player = load.ReturnCharacter;
                    player.canAttack = true;
                    player_allys.Add(player);
            }
                

            }

            enemys.Clear();
            foreach (var load in enemys_Load)
            {
                if (load.isLoaded)
                {
                Character enemy = load.ReturnCharacter;
                enemy.canAttack = true;
                enemys.Add(enemy);
                }
                
            }
        
    }    

    

    public Character ReturnCharacter(bool isPlayer)
    {
        int index;
        if (!isPlayer)
        {
            if(player_allys.Count == 1)
            {
                return player_allys[0];
            }
            index = UnityEngine.Random.Range(0, player_allys_Load.Count);
            return player_allys[index];
        }
        else
        {
            if (player_allys.Count == 1)
            {
                return enemys[0];
            }
            index = UnityEngine.Random.Range(0, enemys_Load.Count);
            return enemys[index];
        }
    }
    protected Character ReturnLoadChar(string name)
    {
        switch (name)
        {
            case "Player":
                return player;
            case "Ghoul":
                return ghoul;
            case "Skeleton":
                return null;
            case "Slime":
                return ghoul;
            default:
                return null;
        }
    }    

    public bool CheckBattle()
    {
        if ( enemys.Count == 0)
        {
            return false;
        }
        return true;
    }
    public void EndBattle()
    {
        startBattle = false;
        foreach (var load in enemys_Load)
        {
            if (load.isLoaded)
            {
                load.isLoaded = false;
                
            }
        }
        Actions.EndBattle?.Invoke();
    }
    LoadCharacter returnInactiveSlot(List<LoadCharacter> characters)
    {
        foreach (var character in characters)
        {
            if (!character.isLoaded)
            {
                return character;
            }
        }
        return null;
    }
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

}
