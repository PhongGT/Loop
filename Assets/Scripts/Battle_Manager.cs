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
    [SerializeField] protected GameObject characterPrefab;
    [SerializeField] protected GameObject player_allys_Parent;
    [SerializeField] protected GameObject enemys_Parent;
    public float dayTime = 10f;
    public int loopCount = 1;
    public bool isNewDay = false;
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
        //enemys = new List<Character>();
        //player_allys = new List<Character>();   
        for (int i = 0; i < 5; i++)
        {
            GameObject player_ally = Instantiate(characterPrefab, player_allys_Parent.transform);
            player_allys_Load.Add(player_ally.GetComponent<LoadCharacter>());
            player_ally.SetActive(false);
            GameObject enemy = Instantiate(characterPrefab, enemys_Parent.transform);
            enemys_Load.Add(enemy.GetComponent<LoadCharacter>());
            enemy.SetActive(false);
        }

        

        StartBattle();
    }

    // Update is called once per frame
    void Update()
    {
        if(isNewDay)
        {

        }
    }
    public void StartBattle()
    {
        // Every time Player Collider with Cell Collider have Enemy Battle Start
        LoadNewCharacter(player_allys_Load[0], player);
        LoadNewCharacter(enemys_Load[0], ghoul);
        LoadTarget();
        //PreperBattle(ref player_allys, ref enemys);
        startBattle = true;
        //StartCoroutine(StartBattleCoroutine());
    }

    public void PreperBattle(ref List<Character> player_ally, ref List<Character> enemys)
    {
        foreach (var Char in enemys)
        {
            LoadCharacter l = returnInactiveCharacter(enemys_Load);
            if (l != null)
            {
                LoadNewCharacter(l, Char, player_ally);
            }

        }

    }
    public void LoadNewCharacter(LoadCharacter loadSlot, Character character, List<Character> enemyTarget)
    {
        Character Char = character;
        Char.canAttack = true;
        Char.Init();
        loadSlot.Load(Char);
        loadSlot.gameObject.SetActive(true);
    }    
    public void LoadNewCharacter(LoadCharacter loadSlot, Character character)
    {
        Character Char = character;
        Char.canAttack = true;
        Char.Init();
        loadSlot.Load(Char);
        loadSlot.gameObject.SetActive(true);
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
                    player_allys.Add(load.ReturnCharacter);
                }
                

            }
        
   
            enemys.Clear();
            foreach (var load in enemys_Load)
            {
                if (load.isLoaded)
                {
                    enemys.Add(load.ReturnCharacter);
                }
                
            }
        
    }    

    

    public Character ReturnCharacter(bool isPlayer)
    {
        int index;
        if (!isPlayer)
        {
            index = UnityEngine.Random.Range(0, player_allys_Load.Count);
            return player_allys[index-1];
        }
        else
        {
            index = UnityEngine.Random.Range(0, enemys_Load.Count);
            return enemys[index-1];
        }
    }

    public void EndBattle()
    {
        foreach (var load in player_allys_Load)
        {
            if (load.isLoaded)
            {
                load.isLoaded = false;
                load.gameObject.SetActive(false);
            }
        }
        foreach (var load in enemys_Load)
        {
            if (load.isLoaded)
            {
                load.isLoaded = false;
                load.gameObject.SetActive(false);
            }
        }
    }
    LoadCharacter returnInactiveCharacter(List<LoadCharacter> characters)
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
