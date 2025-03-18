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
        if (BattleManager.instance.startBattle)
        {
            Debug.Log("Battle Start");
            Attack();
        }
    }
    public void Attack()
    {
        if (!currentChar.canAttack)
            return;

        currentChar.CastSkill(ReturnRandomTarget(currentChar.targets));
        StartCoroutine(currentChar.WaitToAttack());
        Debug.Log("In Attack");

    }
    public void LoadHealth()
    {
        
    }

    public void Dead()
    {
        if (currentChar.isDead)
        {
            //currentChar.animator.SetTrigger("Dead");
            currentChar.canAttack = false;
            currentChar.targets.Clear();
            currentChar = null;
            //BattleManager.instance.CheckBattle();
        }
        this.gameObject.SetActive(false);
    }
    public Character ReturnRandomTarget(List<Character> targets)
    {
        int index = UnityEngine.Random.Range(0, targets.Count);
        return targets[index];
    }


}
