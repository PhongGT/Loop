using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : ScriptableObject
{
    public string name;

    public Sprite icon;
    public float maxHealth { get; private set; }
    public int healthRegen { get; private set; } //% health regen per Day;
    public float damage { get; private set; }

    public float attackSpeed { get; private set; }
    public float evasion { get; private set; }
    public float armor { get; private set; }
    public float baseShield { get; private set; }
    public float vamprirism { get; private set; } // % of damage dealt that is returned as health
    public float counterChance { get; private set; } // % of ignore damege and dealt back to attacker
    public float critChance { get; private set; }
    public float critDamage { get; private set; } // % of damage dealt that is returned as health
    public float pureDamage { get; private set; }


    public BaseStats (BaseStats input)
    {
        
        this.maxHealth = input.maxHealth;
        this.healthRegen = input.healthRegen;
        this.damage = input.damage;
        this.attackSpeed = input.attackSpeed;
        this.evasion = input.evasion;
        this.armor = input.armor;
        this.counterChance = input.counterChance;
        this.critChance = input.critChance;
        this.critDamage = input.critDamage;
        this.pureDamage = input.pureDamage;
        this.counterChance = input.counterChance;
        this.critDamage = input.critDamage;
        this.pureDamage = input.pureDamage;
    }
}
