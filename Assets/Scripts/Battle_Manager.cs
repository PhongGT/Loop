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

        LoadNewCharacter(player_allys_Load[0], player);

        StartBattle();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartBattle()
    {
        // Every time Player Collider with Cell Collider have Enemy Battle Start
        PreperBattle(ref player_allys, ref enemys);
        LoadTarget();
        player_allys_Load[0].UpdateEnemys(enemys);
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
    public void LoadTarget()
    {
        foreach (var load in player_allys_Load)
        {
            player_allys.Add( load.ReturnCharacter);
        }
        foreach (var load in enemys_Load)
        {
            enemys.Add(load.ReturnCharacter);
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
