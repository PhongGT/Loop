using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Character : ScriptableObject
{
    [Header("Stats")]
    public string name;
    public Sprite icon;
    public Event attack;
   [SerializeField] protected float health;
   [SerializeField] protected float maxHealth;
   [SerializeField] protected int healthRegen; //% health regen per Day;
   [SerializeField] protected float damage;
   [SerializeField] protected float attackSpeed;
   [SerializeField] protected float evasion;
   [SerializeField] protected float armor;
   [SerializeField] protected float maxShield;
   [SerializeField] protected float shield;
   [SerializeField] protected float counterChance; // % of ignore damege and dealt back to attacker
   [SerializeField] protected float critChance;
   [SerializeField] protected float critDamage;
   [SerializeField] protected float vamprirism; // % of damage dealt that is returned as health
   [SerializeField] protected float pureDamage; // % of damage that ignores armor
    [Header("Status")]
    // Effects Bleed, Slow, 
    public bool isPlayer;
    [Header("Target")]
    public List<Character> targets;
    [Header("Trigger")]
    public bool canAttack = false;
    
    public Animator animator;
    public bool isDead;

    public void Init()
    {
        health = maxHealth;
        shield = maxShield;
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
       
        health += heal;
    }
    public void Attack(Character target)
    {
        int lastDamage = (int)damage;   
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
    protected void FindTarget()
    {
        if(isPlayer)
        {
            targets = BattleManager.instance.enemys;
        }
        else
        {
            targets = BattleManager.instance.player_allys;
        }
        
    }
    public virtual void CastSkill(Character target)
    {
        Debug.Log(this.name + " Attack");

    }
    public IEnumerator WaitToAttack()
    {
        canAttack = false;
        yield return null;
        //animator.ResetTrigger("Attack");
        yield return new WaitForSeconds(1/attackSpeed);
        canAttack = true;
    }



}
