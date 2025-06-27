using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    [SerializeField] public List<Character> enemys;
    [SerializeField] public List<Character> player_allys;

    
    
    //[SerializeField] protected GameObject player_allys_Parent;
    //[SerializeField] protected GameObject enemys_Parent;
    [SerializeField] float dayTime = 6f;
    float timer = 0f;
    public int loopCount = 1;
    public int dayCount = 1;
    public int healthPotionCount = 2;
    protected bool timerStatState = false;
    public bool startBattle = false;
    public GameObject battlePanel;
    public Player player;
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
        Actions.StartBattle += StartBattle;
        Actions.EndBattle  += EndBattle;
        battlePanel.SetActive(false);
        timer = dayTime;
        //StartBattle();
    }
    void Update()
    {
        if (timerStatState)
        {
           timer -= Time.deltaTime;  
        }

        if (timer <= 0)
        {
            Actions.SpawnMob?.Invoke(dayCount);
            timer = dayTime;
            dayCount++;
        }
        
    }
   
    public void StartBattle()
    {
        startBattle = true;  
    }
    // Input Enemy String List to Load
    public void PreperBattle( List<String> enemys)
    {
        battlePanel.SetActive(true);
        for (int i = 0; i < enemys.Count; i++)
        {
            Spawner.instance.SpawnCharacter(enemys[i] , false ,loopCount);
        }
        

    }
    // Load Character when Battle Start
    public void LoadTarget(bool isPlayer, Character character)
    {
        if(isPlayer)
        {
            player_allys.Add(character);
            player = character as Player;
            player.isPlayer = true;
        }
        else
        {
            enemys.Add(character);
        }
    }
    // Update Target when Character Dead
    public void UpdateTarget(bool isPlayer)
    {
        if (isPlayer)
        {
            enemys.RemoveAll(x => x.isDead);
        }
        else
        {
            player_allys.RemoveAll(x => x.isDead);
        }
    }    
    // Return Character alive
    public Character ReturnCharacter(bool isPlayer)
    {
        int index;
        if (!isPlayer)
        {
            if(player_allys.Count == 1)
            {
                return player_allys[0];
            }
            index = UnityEngine.Random.Range(0, player_allys.Count);
            return player_allys[index];
        }
        else
        {
            if (player_allys.Count == 1)
            {
                return enemys[0];
            }
            index = UnityEngine.Random.Range(0, enemys.Count);
            return enemys[index];
        }
    }
  

    public bool CheckBattleEnd()
    {
        if (enemys.Count == 0 )
        {
            Debug.Log("Battle End");
            startBattle = false;
            Actions.EndBattle?.Invoke();
            return true;
        }
        return false;
    }
    public void EndBattle()
    {
       
        battlePanel.SetActive(false);

    }
    public void OnDisable()
    {
        Actions.StartTimer -= () => timerStatState = true;
        Actions.StartBattle -= StartBattle;
        Actions.EndBattle -= EndBattle;
    }


}
