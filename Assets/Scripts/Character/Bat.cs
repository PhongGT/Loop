using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]

public class Bat : Enemy, I_CastSkill
{

    public Bat(BaseStats data) : base(data) { }

    public override void CastSkill(Character target)
    {
        base.CastSkill(target);
        if (R_Helper.CheckRandom(10f))
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
