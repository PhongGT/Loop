using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item 
{
    public BaseItem baseItem;
    public int itemRarity;
    public StatData mainStat;
    public List<StatData> subStats;
    protected List<string> statBaseNames ;
    protected List<string> subStatBaseNames;

    public Sprite GetSprite()
    {
        return baseItem.itemImage[itemRarity];
    }
    public StatData GetStat( int Level, string Name)
    {
        StatData stat = new StatData();
        switch (Name)
        {
            case "Sword":
                stat.statName = statBaseNames[0];
                stat.statValue = Random.Range(5+Level*2, 7+Level*3);
                return stat;
            case "Armor":
                stat.statName = statBaseNames[1];
                stat.statValue = Random.Range(300+Level*26, 370+Level*30);
                return stat;
            case "Bow":
                stat.statName = statBaseNames[2];
                stat.statValue = Random.Range(1+Level, (int)(5+Level*1.5));
                return stat;
            case "Staff":
                stat.statName = statBaseNames[3];
                stat.statValue = Random.Range(4+Level*2, 6+Level*3);
                return stat;
            case "Book":
                stat.statName = statBaseNames[4];
                stat.statValue = Random.Range(100+15*Level, 175+Level*17);
                return stat;

            case "AttackSpeed":
                stat.statName = subStatBaseNames[0];
                stat.statValue = Random.Range(1 + Level, 2+ Level * 2);
                return stat;
            case "Vamprirism":
                stat.statName = subStatBaseNames[1];
                stat.statValue = Random.Range(1 , 8);
                return stat;
            case "CritChance":
                stat.statName = subStatBaseNames[2];
                stat.statValue = Random.Range(1 + Level, 2 + Level * 2);
                return stat;
            case "CritDamage":
                stat.statName = subStatBaseNames[3];
                stat.statValue = Random.Range(5 + Level, 8 + Level * 2);
                return stat;
            case "Counter":
                stat.statName = subStatBaseNames[4];
                stat.statValue = Random.Range(1 + Level, 2 + Level * 2);
                return stat;
            case "Evasion":
                stat.statName = subStatBaseNames[5];
                stat.statValue = Random.Range(1 + Level, 2 + Level * 2);
                return stat;
            case "HealthRegen":
                stat.statName = subStatBaseNames[6];
                stat.statValue = Random.Range(1 + Level, 2 + Level * 2);
                return stat;
                
            default: Debug.LogError("Stat not found"); return stat; ;
                
        }
    }
    public void GenStat(int Level)
    {
        statBaseNames = new List<string> { "Damage", "Health", "PureDamage", "Magic", "Shield"};
        subStatBaseNames = new List<string> { "AttackSpeed", "Vamprirism", "CritChance", "CritDamage", "Counter", "Evasion", "HealthRegen" };
        List<string> subStatBaseNames1 = new List<string> { "AttackSpeed", "Vamprirism", "CritChance", "CritDamage", "Counter", "Evasion", "HealthRegen" };
        mainStat = GetStat(Level, baseItem.itemName.ToString());
        for (int i = 0; i < itemRarity; i++)
        {
            StatData stat = new StatData();
            stat.statName = subStatBaseNames[Random.Range(0, subStatBaseNames1.Count)];
            subStatBaseNames1.Remove(stat.statName);
            stat.statValue = GetStat(Level, stat.statName).statValue;
            subStats.Add(stat);
        }
    }
    [System.Serializable]

    public struct StatData
    {
        public string statName;
        public int statValue;
    }
}
