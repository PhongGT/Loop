using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

using Tile = ScripableObj.Tile.Tile;

public class Map : MonoBehaviour
{ 
    public GameObject empty;
    public GameObject road;

    public static Map Instance;

    public Cell[,] Cells;
    public List<GameObject> roadCells;
    public List<Cell> sideRoadCells;
    public Cell campFire;
    [FormerlySerializedAs("Scalesize")] 
    public float scalesize = 1.5f;
    public GameObject playerInMap;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }   
        

    }
    private void Start()
    {
        Cells = new Cell[17, 12];
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
                if (tile.type != Tile.Type.Road) 
                { 
                    Draw(tile); 
                }
               
            }
        }
        
    }
    public void Draw(Tile tile)
    {
        GameObject draw = GetTile(tile);
        Vector3 place = this.transform.position + tile.position * scalesize;
        if(draw !=null)
        {
            GameObject newObj = Instantiate(draw, place, transform.rotation , this.transform);
            newObj.name = tile.type.ToString() + tile.position.x + " " +tile.position.y;
            Cell cell = newObj.GetComponent<Cell>();
            cell.SetTile(tile);
            cell.defaultTile = tile;
            if(tile.type == Tile.Type.Campfire)
            {
                campFire = cell;
                newObj.tag = "Campfire";
                roadCells.Add(newObj);
            }
            else if (tile.type == Tile.Type.Road )
            {
                roadCells.Add(newObj);

            }
            Cells[(int)tile.position.x, (int)tile.position.y] = cell;
             
        }
    }
    private GameObject GetTile(Tile cell)
    {
        switch (cell.type)
        {
            case Tile.Type.Empty: return empty;
            case Tile.Type.Road: return road;
            case Tile.Type.Campfire: return road;
            default: return null;
        }
    }
    public Cell ReturnCell(int width, int height)
    {
       
        return Cells[width, height] ;
    }

    public void LoadSideRoad(int width, int height)
    {
        Cell cell = Cells[width, height];
        sideRoadCells.Add(cell);
        cell.defaultTile.SetSideRoad();
        cell.UpdateCell();

    }

}
