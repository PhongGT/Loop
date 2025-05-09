using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Enemy : Character
{
    public Enemy(BaseStats data) : base(data) { }

    public int changeToDropItem;

    public override void Init()
    {
        base.Init();
        changeToDropItem = baseStats.changeToDropItem;
    }
}
