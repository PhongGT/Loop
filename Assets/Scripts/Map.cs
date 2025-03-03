using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
       
    // Start is called before the first frame update
        public GameObject Vampiermansion;
        public GameObject RoadLantern;
        public GameObject Goblincamp;
        public GameObject Spidercocoon;
        //Road
        public GameObject Cemetery;
        public GameObject Grove;
        public GameObject Ruins;
        public GameObject Swamp;
        public GameObject Village;
        //LandsGameObject
        public GameObject Rock;
        public GameObject Mountain;
        public GameObject Mountainpeak;
        public GameObject Thicket;
        public GameObject Forest;
        public GameObject River;
        public GameObject Meadow;
        //SpeciGameObject
        public GameObject Treasury;
        public GameObject Emptytreasury;
        public GameObject Beacon;
        public GameObject Empty;
        public GameObject Road;

    public float Scalesize = 1.5f;
    private void Awake()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void Draw(Tile[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        for (int i = 0; i < width; i++) 
        {
            for (int j = 0; j < height; j++)
            {
                Tile tile = state[i, j];
                if (tile.type != Tile.Type.Road) { 
                    Draw(tile); }
               
            }
        }
    }
    public void Draw(Tile tile)
    {
        GameObject draw = GetTile(tile);
        Vector3 place = this.transform.position + tile.position * Scalesize;
        if(draw !=null)
        {
            GameObject newObj = Instantiate(draw, place, transform.rotation , this.transform);
            
        }
    }
    private GameObject GetTile(Tile cell)
    {
        switch (cell.type)
        {
            case Tile.Type.Beacon: return Beacon;
            case Tile.Type.River: return River;
            case Tile.Type.Mountain: return Mountain;
            case Tile.Type.Mountainpeak: return Mountainpeak;
            case Tile.Type.Treasury: return Treasury;
            case Tile.Type.Emptytreasury: return Emptytreasury;
            case Tile.Type.Cemetery: return Cemetery;
            case Tile.Type.Spidercocoon: return Spidercocoon;
            case Tile.Type.Thicket: return Thicket;
            case Tile.Type.Rock: return Rock;
            case Tile.Type.Vampiermansion: return Vampiermansion;
            case Tile.Type.RoadLantern: return RoadLantern;
            case Tile.Type.Goblincamp: return Goblincamp;
            case Tile.Type.Forest: return Forest;
            case Tile.Type.Grove: return Grove;
            case Tile.Type.Ruins: return Ruins;
            case Tile.Type.Swamp: return Swamp;
            case Tile.Type.Village: return Village;
            case Tile.Type.Meadow: return Meadow;
            case Tile.Type.Empty: return Empty;
            case Tile.Type.Road: return Road;
            default: return null;
        }
    }
}
