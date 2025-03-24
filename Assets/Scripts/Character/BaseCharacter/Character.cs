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
    protected float health { get; set; }
    protected float shield { get; set; }
    // % of damage that ignores armor
    [Header("Status")]
    // Effects Bleed, Slow, 
    public bool isPlayer;

    [Header("Trigger")]
    public bool canAttack = false;
    
    public Animator animator;
    public bool isDead;

    public void Load(BaseStats baseStats)
    {
        this.baseStats = new BaseStats(baseStats);
    }
    public void Init()
    {
        this.health = baseStats.maxHealth;
        this.shield = baseStats.baseShield;
        isDead = false;

        // Reset Effect on Character
    }
    public int TakeDamage(float damage)
    {
        int lastDamage;
        if (R_Helper.CheckRandom(baseStats.evasion))
        {
            //Degub.Log("Evasion");
            return 0;
        }
        if (R_Helper.CheckRandom(baseStats.counterChance))
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
       
        health = Mathf.Clamp(heal+health, 0, baseStats.maxHealth);
    }
    public void Attack(Character target)
    {
        int lastDamage = (int)baseStats.damage;   
        if (R_Helper.CheckRandom(baseStats.critChance))
        {
            lastDamage = (int)(lastDamage * baseStats.critDamage);
        }
          lastDamage = (int)(lastDamage - Mathf.Clamp(target.baseStats.armor, 0f, int.MaxValue));
        int vamprirismDamage = target.TakeDamage(lastDamage);
        if (baseStats.vamprirism > 0 && vamprirismDamage != 0)
        {
            Heal(vamprirismDamage);
        }
    }
    public void Attack(Character target, float dameBonus)
    {
        int lastDamage = Mathf.RoundToInt(dameBonus);
        if (R_Helper.CheckRandom(baseStats.critChance))
        {
            lastDamage = (int)(lastDamage * baseStats.critDamage);
        }
        lastDamage = (int)(lastDamage - Mathf.Clamp(target.baseStats.armor, 0f, int.MaxValue));
        int vamprirismDamage = target.TakeDamage(lastDamage);
        if (baseStats.vamprirism > 0 && vamprirismDamage != 0)
        {
            Heal(vamprirismDamage);
        }
    }
    public void CounterAttack(Character target)
    {
        //Debug.Log("Counter + target.name ");
        
        int lastDamage = (int)baseStats.damage;
        lastDamage = (int)(lastDamage - Mathf.Clamp(target.baseStats.armor, 0f, int.MaxValue));
        int vamprirismDamage = target.TakeDamage(lastDamage);
        if (baseStats.vamprirism > 0 && vamprirismDamage != 0)
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
        Debug.Log(this.baseStats.name + " Attack v    " + target.baseStats.name);

    }
    public IEnumerator WaitToAttack()
    {
        canAttack = false;
        yield return null;
        //animator.ResetTrigger("Attack");
        yield return new WaitForSeconds(1/baseStats.attackSpeed);
        canAttack = true;
    }
}
