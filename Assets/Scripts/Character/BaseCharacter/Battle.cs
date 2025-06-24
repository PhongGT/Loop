using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Battle : MonoBehaviour
{
    public Character currentChar;
    public LoadCharacter loadCharacter;
    public HealthBar healthBar;
    [SerializeField] public Transform healthBarTF;


    [Header("Trigger")]
    
    public bool canAttack;
    public bool triggerSkill;

    public Animator animator;
    private void Start()
    {
        loadCharacter = GetComponentInParent<LoadCharacter>();
        healthBar = GetComponent<HealthBar>();
        animator = GetComponent<Animator>();
        
        if (healthBar != null)
        {
            healthBar.SpawnHealthBar(healthBarTF);
            healthBar.healthBarSpawn.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Health bar not found");
        }

    }

    void Update()
    {
        healthBar.UpdateHealthBar(currentChar.health, currentChar.maxHealth);

        if (BattleManager.instance.startBattle && currentChar.isDead)
        {
            Attack();

        }
        if(currentChar.isDead)
        {
            Dead();
        }
    }
    public void Attack()
    {
        if (!canAttack)
            return;
        currentChar.CastSkill(BattleManager.instance.ReturnCharacter(this.currentChar.isPlayer));
        if(BattleManager.instance.CheckBattle())
        {
            StartCoroutine(WaitToAttack());
        }
        
        Debug.Log("In Attack");

    }
    protected void SetTrigger(string name)
    {
        if (animator != null)
        {
            animator.SetTrigger(name);
        }
        else
        {
            Debug.LogError("Animator not found");
        }
    }
    protected void ResetTrigger(string name)
    {
        if (animator != null)
        {
            animator.ResetTrigger(name);
        }
        else
        {
            Debug.LogError("Animator not found");
        }
    }

    protected void  Dead()
    {
        if (currentChar.isDead)
        {
            if(currentChar.isPlayer)
            {
                //Gameover
                return;
            }
            canAttack = false;
            loadCharacter.Clear();
            
            BattleManager.instance.UpdateTarget(this.currentChar.isPlayer);

            Drop_Manager.instance.DropOnEnemyDead();

            if (!BattleManager.instance.CheckBattle())
            {
                BattleManager.instance.EndBattle(); 
                Destroy(this.gameObject);
            }
            
        }
        
        

    }
    public IEnumerator WaitToAttack()
    {
        canAttack = false;
        yield return null;
        //animator.ResetTrigger("Attack");
        yield return new WaitForSeconds(1 / currentChar.attackSpeed);
        canAttack = true;
    }



}
