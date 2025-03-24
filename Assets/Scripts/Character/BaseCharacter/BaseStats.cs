using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New BaseStats", menuName = "BaseStats")]
[System.Serializable]
public class BaseStats : ScriptableObject
{
    public string name;

    public Sprite icon;
    public float maxHealth ;
    public int healthRegen ; //% health regen per Day;
    public float damage ;
    public float attackSpeed ;
    public float evasion ;
    public float armor ;
    public float baseShield ;
    public float vamprirism ; // % of damage dealt that is returned as health
    public float counterChance ; // % of ignore damege and dealt back to attacker
    public float critChance ;
    public float critDamage ; // % of damage dealt that is returned as health
    public float pureDamage ;


    public BaseStats (BaseStats input)
    {
        this.name = input.name;
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
