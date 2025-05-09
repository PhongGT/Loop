using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected List<LoadCharacter> player_allys_Pos;
    [SerializeField] protected List<LoadCharacter> enemys_Pos;
    public static Spawner instance;


    [Header("BaseStats")]
    [SerializeField] protected BaseStats[] baseStats;
    [SerializeField] protected Dictionary<string, BaseStats> baseStatsDict = new Dictionary<string, BaseStats>();

    [SerializeField] protected Dictionary<string, Func<BaseStats, Character>> characterCreators = new Dictionary<string, Func<BaseStats, Character>>
    {
        { "Player", data => new Player(data) },
        { "Bat", data => new Bat(data) },
        { "Bandit", data => null },
        { "Vampire", data => null },
        { "Goblin", data => null },
        { "Slime", data => new Slime(data) },
        { "Skeleton", data => null },
        { "Boss", data => null }
    };

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
        LoadBaseStat();
        AddBaseStast();
    }
    private void Start()
    {

        SpawnCharacter("Player", true, 0);
    }
    protected void LoadBaseStat()
    {
        baseStats = Resources.LoadAll<BaseStats>("ScriptableObj/BaseCharacterStat");
    }
    protected void AddBaseStast()
    {
        foreach (BaseStats item in baseStats)
        {
            if (!baseStatsDict.ContainsKey(item.name))
            {
                
                baseStatsDict.Add(item.nameChar, item);
            }
        }    
    }
    protected BaseStats GetBaseStats(String name)
    {
        if (baseStatsDict.ContainsKey(name))
        {
            return baseStatsDict[name];
        }
        else
        {
            Debug.LogError("BaseStats "+ name +" not found");

            return null;
        }
    }
    

    protected Character CreateCharacter(string name, BaseStats stats)
    {
        if(characterCreators.TryGetValue(name, out var func))
        {
            return func(stats);
        }
        else
        {
            Debug.Log("Character " + name + " not found");
            return null;
        }
    }
    public void SpawnCharacter(string name , bool isPlayer, int loopCount)
    {
        BaseStats stats = GetBaseStats(name);
        if (stats == null) return;
        
        Character character = CreateCharacter(name, stats);
        if (character != null)
        {
            ReturnPosValid(isPlayer).Load(character);
            BattleManager.instance.LoadTarget(isPlayer, character);
        }    

        


       
    }
    public LoadCharacter ReturnPosValid(bool isPlayer)
    {
        if (isPlayer)
        {
            return ReturnPosValidPlayer();
        }
        else
        {
            return ReturnPosValidEnemy();
        }

    }

    private LoadCharacter ReturnPosValidEnemy()
    {
        for (int i = 0; i < enemys_Pos.Count; i++)
        {
            if (enemys_Pos[i].isLoaded == false)
            {
                return enemys_Pos[i];
            }
        }
        return null;
    }

    private LoadCharacter ReturnPosValidPlayer()
    {
        for (int i = 0; i < player_allys_Pos.Count; i++)
        {
            if (player_allys_Pos[i].isLoaded == false)
            {
                return player_allys_Pos[i];
            }
        }
        return null;
    }
}
