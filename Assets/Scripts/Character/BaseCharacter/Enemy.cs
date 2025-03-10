using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public virtual void CastSkill(Character target)
    {
        Debug.Log("Enemy Attack");

    }
}
