using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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


}
