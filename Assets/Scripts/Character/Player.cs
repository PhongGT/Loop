using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
[System.Serializable]
public class Player : Character , I_CastSkill
{
    bool SunBlazeBladeLearn = false;
    int potionCount = 3;


    public override void CastSkill(Character target)
    {
        base.CastSkill(target);

        if (base.health < base.maxHealth/2 && potionCount > 0 )
        {
            HealingPotion(0.15f);
            potionCount--;
        }
        Tackle(target);

    }
    public void Tackle (Character target)
    {
        Attack(target);
    }
    protected void HealingPotion(float healPercent)
    {
        Debug.Log("Heal");
        Heal((int)(base.maxHealth * healPercent));
    }
    protected void SunBlazeBlade()
    {
        Debug.Log("SunBlazeBlade");
        
        float dame = 1.25f* (BattleManager.instance.loopCount+ base.damage);
        foreach (var target in base.targets)
        {
            Attack(target, base.damage * 1.5f);
        }
        
    }
    protected void ShieldBash(Character target)
    {
        Debug.Log("ShieldBash");
        float dame = 2.5f * (base.armor + 5 * BattleManager.instance.loopCount);
        Attack(target, dame);
        // Add Effect Stun to target

    }
    protected void NewDay()
    {
        Debug.Log("New Day");

        base.Heal(healthRegen);
        if (SunBlazeBladeLearn)
        {
            SunBlazeBlade();
        }
        BattleManager.instance.isNewDay = false;
    }


}
