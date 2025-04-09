using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : Character , I_CastSkill
{
    bool SunBlazeBladeLearn = false;
    int potionCount = 3;
   



    public override void CastSkill(Character target)
    {
        base.CastSkill(target);

        if (base.health < base.maxHealth / 2 && potionCount > 0 )
        {
            HealingPotion(0.15f);
            potionCount--;
        }
        
        Tackle(target);

    }
    public void Tackle (Character target)
    {
        Attack(target);
    }
    protected void HealingPotion(float healPercent)
    {
        Debug.Log("Heal");
        Heal((int)(base.maxHealth * healPercent));
    }
    protected void SunBlazeBlade()
    {
        Debug.Log("SunBlazeBlade");
        
        float dame = 1.25f* (BattleManager.instance.loopCount+ base.damage);
        foreach (var target in BattleManager.instance.enemys)
        {
            Attack(target, base.damage * 1.5f);
        }
        
    }
    protected void ShieldBash(Character target)
    {
        Debug.Log("ShieldBash");
        float dame = 2.5f * (base.armor + 5 * BattleManager.instance.loopCount);
        Attack(target, dame);
        // Add Effect Stun to target

    }
    protected void NewDay()
    {
        Debug.Log("New Day");

        base.Heal(healthRegen);
        if (SunBlazeBladeLearn)
        {
            SunBlazeBlade();
        }
        
    }
    protected void ShieldWall()
    {
        Debug.Log("ShieldWall");
        // Add Effect Shield to target
        // Add Effect Stun to target
    }
    public void EquipItem(Item lastestItem, Item newestItem, string itemType )
    {
        switch (itemType)
        {
            case "Sword":
                damage += -newestItem.mainStat.statValue + lastestItem.mainStat.statValue;
                break;
            case "Armor":
                int healthDiff = -newestItem.mainStat.statValue + lastestItem.mainStat.statValue;
                maxHealth += healthDiff;
                if (healthDiff <= 0)
                health = Mathf.Clamp(health - healthDiff, 1 , int.MaxValue);
                else
                {
                    health = Mathf.Clamp(health + healthDiff, 1, maxHealth);
                }
                    break;
            case "Bow":
                pureDamage += -newestItem.mainStat.statValue + lastestItem.mainStat.statValue;
                break;
            case "Staff":
                magic += -newestItem.mainStat.statValue + lastestItem.mainStat.statValue;
                break;
            case "Book":
                baseShield += -newestItem.mainStat.statValue + lastestItem.mainStat.statValue;
                break;
        }
        for (int i = 0; i < newestItem.subStats.Count; i++)
        {
            switch (newestItem.subStats[i].statName)
            {
                case "AttackSpeed":
                    attackSpeed -= newestItem.subStats[i].statValue ;
                    break;
                case "Vamprirism":
                    vamprirism -= newestItem.subStats[i].statValue ;
                    break;
                case "CritChance":
                    critChance -= newestItem.subStats[i].statValue;
                    break;
                case "CritDamage":
                    critDamage -= newestItem.subStats[i].statValue;
                    break;
                case "Counter":
                    counterChance -= newestItem.subStats[i].statValue;
                    break;
                case "Evasion":
                    evasion -= newestItem.subStats[i].statValue;
                    break;
                case "HealthRegen":
                    healthRegen -= newestItem.subStats[i].statValue;
                    break;
                default:
                    break;
            }
        }
        for (int i = 0; i < lastestItem.subStats.Count; i++)
        {
            switch (lastestItem.subStats[i].statName)
            {
                case "AttackSpeed":
                    attackSpeed += lastestItem.subStats[i].statValue;
                    break;
                case "Vamprirism":
                    vamprirism += lastestItem.subStats[i].statValue;
                    break;
                case "CritChance":
                    critChance += lastestItem.subStats[i].statValue;
                    break;
                case "CritDamage":
                    critDamage += lastestItem.subStats[i].statValue;
                    break;
                case "Counter":
                    counterChance += lastestItem.subStats[i].statValue;
                    break;
                case "Evasion":
                    evasion += lastestItem.subStats[i].statValue;
                    break;
                case "HealthRegen":
                    healthRegen += lastestItem.subStats[i].statValue;
                    break;

                default:
                    break;
            }
        }

    }


}
