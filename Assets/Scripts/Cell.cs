using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell 
{
        public enum Type
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
    public enum Placement
    {
        Road,
        Sideroad,
        Landscape,
        Special,
    }
    public enum Effect
    {
        Adj,
        R1,
        R2,
    }

    public Vector3Int position;
    public Type type;
    public Placement placement;
    public Effect effect;
    public Sprite[] sprite;   

}
