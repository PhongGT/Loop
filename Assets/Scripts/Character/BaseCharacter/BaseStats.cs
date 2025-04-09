using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New BaseStats", menuName = "BaseStats")]
[System.Serializable]
public class BaseStats : ScriptableObject
{
    public string nameChar;
    public GameObject charPrefab;
    public float maxHealth ;
    public int healthRegen ; //% health regen per Day;
    public float damage ;
    public float attackSpeed ;
    public float magic; 
    public float evasion ;
    public float armor ;
    public float baseShield ;
    public float vamprirism ; // % of damage dealt that is returned as health
    public float counterChance ; // % of ignore damege and dealt back to attacker
    public float critChance ;
    public float critDamage ; // % of damage dealt that is returned as health
    public float pureDamage ;





}
