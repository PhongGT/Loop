using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu]
[System.Serializable]

public class Ghoul : Enemy, I_CastSkill
{
    Character target ;
    
    public override void CastSkill(Character target)
    {
        base.name = "Ghoul";
        Debug.Log("Ghoul Attack");
        if(R_Helper.CheckRandom(10f))
        {
            Bite(target);
        }
        else
        {
            Tackle(target);
        }

    }

    public void Tackle(Character target)
    {
        Attack(target);
        
    }

    public void Bite(Character target)
    {
        Debug.Log("Bite");  
        Attack(target , 10f);
    }


}
