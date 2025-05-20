using System;
using UnityEngine;

namespace ScripableObj.Tile
{
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
            Campfire,
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
            Udlr,
            Ef3X3, // 3*3
        
        }
   
        public Vector3 position;
        public Type type;
        public Placement placement;
        public Effect effect;
        public Sprite sprite;
        public Sprite spritePreview;
        public Sprite mobSprite;
        public String mobName;
        public float chanceToSpawn;
        public int dayNeedToSpawn;
        public String tileDescription;
        public String tileEffectDescription;


        //Mob can Spawn

        public Tile(Tile type)
        {
            this.type = type.type;
            this.placement = type.placement;
            this.effect = type.effect;
            this.sprite = type.sprite;
            this.spritePreview = type.spritePreview;
            this.mobSprite = type.mobSprite;
            this.mobName = type.mobName;
            this.chanceToSpawn = type.chanceToSpawn;
            this.dayNeedToSpawn = type.dayNeedToSpawn;
            this.tileDescription = type.tileDescription;
            this.tileEffectDescription = type.tileEffectDescription;
        
        }    
        public void SetTile(Tile tile)
        {
            this.type = tile.type;
            this.placement = tile.placement;
            this.effect = tile.effect;
            this.sprite = tile.sprite;
            this.spritePreview = tile.spritePreview;
            this.mobSprite = tile.mobSprite;
            this.mobName = tile.mobName;
            this.chanceToSpawn = tile.chanceToSpawn;
            this.dayNeedToSpawn = tile.dayNeedToSpawn;
            this.tileDescription = tile.tileDescription;
            this.tileEffectDescription = tile.tileEffectDescription;
        }
        public Tile()
        {
            this.type = Type.Empty;
            this.placement = Placement.Special;
            this.effect = Effect.None;
            this.sprite = null;
            this.spritePreview = null;
            this.mobSprite = null;
            this.chanceToSpawn = 0;
            this.dayNeedToSpawn = 0;
            this.tileDescription = "";
            this.tileEffectDescription = "";
        }
        public void SetSideRoad()
        {
            this.placement = Placement.Sideroad;    
        }

        public bool IsValid(Tile tile)
        {
            return this.placement == tile.placement;
        }
    

    }
}
