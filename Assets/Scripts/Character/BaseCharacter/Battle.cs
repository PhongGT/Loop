using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Battle : MonoBehaviour
{
    public Character currentChar;
    public LoadCharacter loadCharacter;
    private void Start()
    {
        loadCharacter = GetComponentInParent<LoadCharacter>();
    }

    void Update()
    {
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
        if(BattleManager.instance.CheckBattle())
        {
            StartCoroutine(currentChar.WaitToAttack());
        }
        
        Debug.Log("In Attack");

    }
    public void LoadHealth()
    {
        
    }

    public bool  Dead()
    {
        if (currentChar.isDead)
        {
            if(currentChar.isPlayer)
            {
                //Gameover
                return true;
            }
            currentChar.canAttack = false;
            loadCharacter.isLoaded = false;
            this.gameObject.SetActive(false);
            BattleManager.instance.LoadTarget(this.currentChar.isPlayer);

            if(!BattleManager.instance.CheckBattle())
            {
                BattleManager.instance.EndBattle(); 
                Destroy(this.gameObject, 2f);
            }
            return true;
        }
        return false;
        

    }



}
