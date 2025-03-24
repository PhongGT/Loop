using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
        public GameObject Empty;
        public GameObject Road;

    public static Map instance;

    public List<Cell> cells;
    public float Scalesize = 1.5f;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }   

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
            Cell cell = newObj.GetComponent<Cell>();
            cell.SetTile(tile);
            cell.defaultTile = tile;
            cells.Add(cell);  
        }
    }
    private GameObject GetTile(Tile cell)
    {
        switch (cell.type)
        {
            case Tile.Type.Empty: return Empty;
            case Tile.Type.Road: return Road;
            default: return null;
        }
    }
    public Cell ReturnCell(int index)
    {
       
        return cells[index] ;
    }
}
