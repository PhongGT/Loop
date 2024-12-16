using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
        public Tilemap tilemap {  get; private set; }
    // Start is called before the first frame update
        public Tile Vampiermansion;
        public Tile RoadLantern;
        public Tile Goblincamp;
        public Tile Spidercocoon;
        //Road
        public Tile Cemetery;
        public Tile Grove;
        public Tile Ruins;
        public Tile Swamp;
        public Tile Village;
        //Landscape
        public Tile Rock;
        public Tile Mountain;
        public Tile Mountainpeak;
        public Tile Thicket;
        public Tile Forest;
        public Tile River;
        public Tile Meadow;
        //Special
        public Tile Treasury;
        public Tile Emptytreasury;
        public Tile Beacon;
        public Tile Empty;
        public Tile Road;
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void Draw(Cell[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        for (int i = 0; i < width; i++) 
        {
            for (int j = 0; j < height; j++)
            {
                Cell cell = state[i, j];
                tilemap.SetTile(cell.position, Empty);
            }
        }
    }
    public void Draw(Cell cell)
    {
        tilemap.SetTile(cell.position, GetTile(cell));
    }
    private Tile GetTile(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Beacon: return Beacon;
            case Cell.Type.River: return River;
            case Cell.Type.Mountain: return Mountain;
            case Cell.Type.Mountainpeak: return Mountainpeak;
            case Cell.Type.Treasury: return Treasury;
            case Cell.Type.Emptytreasury: return Emptytreasury;
            case Cell.Type.Cemetery: return Cemetery;
            case Cell.Type.Spidercocoon: return Spidercocoon;
            case Cell.Type.Thicket: return Thicket;
            case Cell.Type.Rock: return Rock;
            case Cell.Type.Vampiermansion: return Vampiermansion;
            case Cell.Type.RoadLantern: return RoadLantern;
            case Cell.Type.Goblincamp: return Goblincamp;
            case Cell.Type.Forest: return Forest;
            case Cell.Type.Grove: return Grove;
            case Cell.Type.Ruins: return Ruins;
            case Cell.Type.Swamp: return Swamp;
            case Cell.Type.Village: return Village;
            case Cell.Type.Meadow: return Meadow;
            case Cell.Type.Empty: return Empty;
            case Cell.Type.Road: return Road;
            default: return null;
        }
    }
}
