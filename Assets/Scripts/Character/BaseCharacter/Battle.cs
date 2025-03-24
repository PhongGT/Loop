using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Battle : MonoBehaviour
{
    public Character currentChar;
    public LoadCharacter loadCharacter;
    private void Start()
    {
        loadCharacter = GetComponent<LoadCharacter>();
    }
    // Update is called once per frame
    void Update()
    {
        //if(currentChar.animator == null) {
        //    return;
        //}
        
        if (BattleManager.instance.startBattle && !Dead())
        {
           
            Attack();
        }
        
    }
    public void Attack()
    {
        if (!currentChar.canAttack)
            return;

        currentChar.CastSkill(BattleManager.instance.ReturnCharacter(this.currentChar.isPlayer));
        StartCoroutine(currentChar.WaitToAttack());
        Debug.Log("In Attack");

    }
    public void LoadHealth()
    {
        
    }

    public bool  Dead()
    {
        if (currentChar.isDead)
        {
            //currentChar.animator.SetTrigger("Dead");
            currentChar.canAttack = false;
          
            currentChar = null;
            //BattleManager.instance.CheckBattle();
            this.gameObject.SetActive(false);
            BattleManager.instance.LoadTarget(this.currentChar.isPlayer);
            return true;
        }
        return false;
        

    }



}
