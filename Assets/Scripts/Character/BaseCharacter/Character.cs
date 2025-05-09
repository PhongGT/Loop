using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Character 
{
    [Header("Stats")]
    public BaseStats baseStats;

    [SerializeField] public float health;
    public float shield;
    public float maxHealth;
    public int healthRegen; //% health regen per Day;
    public float damage;
    public float attackSpeed;
    public float magic;
    public float evasion;
    public float armor;
    public float baseShield;
    public float vamprirism; // % of damage dealt that is returned as health
    public float counterChance; // % of ignore damege and dealt back to attacker
    public float critChance;
    public float critDamage; // % of damage dealt that is returned as health
    public float pureDamage;
    // % of damage that ignores armor
    [Header("Status")]
    // Effects Bleed, Slow, 
    public bool isPlayer;

    [Header("Trigger")]

    public bool isDead;

    public Character (BaseStats Stats)
    {
        this.baseStats = Stats;
        Init();
    }
    public virtual void Init()
    {
        this.health = baseStats.maxHealth;
        this.shield = baseStats.baseShield;
        this.maxHealth = baseStats.maxHealth;
        this.healthRegen = baseStats.healthRegen;
        this.damage = baseStats.damage;
        this.attackSpeed = baseStats.attackSpeed;
        this.magic = baseStats.magic;
        this.evasion = baseStats.evasion;
        this.armor = baseStats.armor;
        this.baseShield = baseStats.baseShield;
        this.vamprirism = baseStats.vamprirism;
        this.counterChance = baseStats.counterChance;
        this.critChance = baseStats.critChance;
        this.critDamage = baseStats.critDamage;
        this.pureDamage = baseStats.pureDamage;
        isDead = false;
        // Reset Effect on Character
    }
    public int TakeDamage(float damage)
    {
        int lastDamage;
        if (R_Helper.CheckRandom(evasion))
        {
            //Degub.Log("Evasion");
            return 0;
        }
        if (R_Helper.CheckRandom(counterChance))
        {
            
            return 0;
        }
        if (shield > 0 && shield > damage)
        {
            shield -= damage;
            return 0;
        }
        else if(shield > 0 && shield < damage)
        {
            lastDamage = (int)(damage - shield);
            health -= lastDamage;
            
        }
        else
        {
            lastDamage = (int)damage;
            health -= lastDamage;
        }
        if (health <= 0)
        {
            Die();
        }
        return lastDamage;
            
        
    }
    public void Heal(int heal)
    {
       
        health = Mathf.Clamp(heal+health, 0, maxHealth);
    }
    public void Heal(float healPercent)
    {
        health = Mathf.Clamp(health + maxHealth * healPercent, 0, maxHealth);
    }
    public void Attack(Character target)
    {
        int lastDamage = (int)damage;   
        if (R_Helper.CheckRandom(critChance))
        {
            lastDamage = (int)(lastDamage * critDamage);
        }
          lastDamage = (int)(Mathf.Clamp(lastDamage - target.armor, 1f, int.MaxValue));
        int vamprirismDamage = target.TakeDamage(lastDamage);
        if (vamprirism > 0 && vamprirismDamage != 0)
        {
            Heal(vamprirismDamage);
        }
    }
    public void Attack(Character target, float dameBonus)
    {
        int lastDamage = Mathf.RoundToInt(dameBonus);
        if (R_Helper.CheckRandom(critChance))
        {
            lastDamage = (int)(lastDamage * critDamage);
        }
        lastDamage = (int)(lastDamage - Mathf.Clamp(target.armor, 0f, int.MaxValue));
        int vamprirismDamage = target.TakeDamage(lastDamage);
        if (vamprirism > 0 && vamprirismDamage != 0)
        {
            Heal(vamprirismDamage);
        }
    }
    public void CounterAttack(Character target)
    {
        //Debug.Log("Counter + target.name ");
        
        int lastDamage = (int)damage;
        lastDamage = (int)(lastDamage - Mathf.Clamp(target.armor, 0f, int.MaxValue));
        int vamprirismDamage = target.TakeDamage(lastDamage);
        if (vamprirism > 0 && vamprirismDamage != 0)
        {
            Heal(vamprirismDamage);
        }
    }
    public void AddEffect()
    {
        // Add Effect to Character
    }
    protected virtual void Die()
    {
        // If have Blood Groot Status Die when down to 20% max health


        if (health <= 0)
        {
            if (isPlayer)
            {
                GameManager.instance.GameOver();
            }
            else
            {
                isDead = true;
                
            }
        }

    }

    public virtual void CastSkill(Character target)
    {
        Debug.Log(this.baseStats.nameChar + " Attack v    " + target.baseStats.nameChar);

    }

}
