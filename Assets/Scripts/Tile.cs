using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Tile : ScriptableObject
{
        [Serializable]public enum Type
    {
        //Sideroad
        Vampiermansion,
        RoadLantern,
        Goblincamp,
        Spidercocoon,
        //Road
        Road,
        Cemetery,
        Grove,
        Ruins,
        Swamp,
        Village,
        //Landscape
        Rock,
        Mountain,
        Mountainpeak,
        Thicket,
        Forest,
        River,
        Meadow,
        //Special
        Treasury,
        Emptytreasury,
        Beacon,
        Empty,


    }
    [Serializable]public enum Placement
    {
        Road,
        Sideroad,
        Landscape,
        Special,
    }
    [Serializable]public enum Effect
    {
        None,
        UDLR,
        Ef_3x3, // 3*3
        
    }
   
    public Vector3 position;
    public Type type;
    public Placement placement;
    public Effect effect;
    public Sprite sprite;
    public Sprite spritePreview;
    public string nameMob;
    public float chanceToSpawn;
    public float dayNeedToSpawn;
    public string cellDescription;
    public string cellEffectDescription;


    //Mob can Spawn

    public Tile(Tile type)
    {
        this.type = type.type;
        this.placement = type.placement;
        this.effect = type.effect;
        this.sprite = type.sprite;
        this.spritePreview = type.spritePreview;
        this.nameMob = type.nameMob;
        this.chanceToSpawn = type.chanceToSpawn;
        this.dayNeedToSpawn = type.dayNeedToSpawn;
        this.cellDescription = type.cellDescription;
        this.cellEffectDescription = type.cellEffectDescription;
        
    }
    public Tile()
    {
        this.type = Type.Empty;
        this.placement = Placement.Special;
        this.effect = Effect.None;
        this.sprite = null;
        this.spritePreview = null;
        this.nameMob = "";
        this.chanceToSpawn = 0;
        this.dayNeedToSpawn = 0;
        this.cellDescription = "";
        this.cellEffectDescription = "";
    }

}
