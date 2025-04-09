using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrewedInk.WFC;
using System;
using Random = UnityEngine.Random;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEditor.U2D.Animation;
using System.Linq;
using Unity.VisualScripting;
using Newtonsoft.Json;
[System.Serializable]
public class WFC : MonoBehaviour
{

    [SerializeField] public List<RoadsCord> roads = new List<RoadsCord>();
    [SerializeField] public List<SideRoad> sideRoads = new List<SideRoad>();
    [SerializeField] public Stack<Tile> roadStack = new Stack<Tile>();

    [SerializeField] protected Tile road;
    [SerializeField] protected Tile empty;
    [SerializeField] protected Tile campfire;
    public bool[,] visited;
    public Slot slot;
    int width = 17;
    int height = 12;
    public Tile[,] tiles;
    
    private void Awake()
    {
        transform.position =  R_Helper.PositionBottomLeftCamera();
    }
    void Start()
    {
        //GenerationCell();
        //GridGen();
        //map.Draw(cells);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void NewGame()
    {
        tiles = new Tile[width, height];
        visited = new bool[width, height];
    }
    public void GenerationCell()
    {
        NewGame();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Tile tile = new Tile(empty);
                tile.position = new Vector2(i, j);
                tiles[i, j] = tile;
            }
        }
    }
    public void GridGen()
    {
        float centerI = width / 2;
        float centerJ = height / 2;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                float distance = Mathf.Sqrt(Mathf.Pow(i - centerI, 2) + Mathf.Pow(j - centerJ, 2));
                if (distance < 5)
                {
                    Tile tile = new Tile(road);
                    tile.position = new Vector2(i, j);
                    tiles[i, j] = tile;
                    roads.Add(new RoadsCord(i , j));
                }
                else if (distance < 5.4)
                {
                    if (Random.Range(0, 4) < 3)
                    {
                        Tile tile1 = new Tile(road);
                        tile1.position = new Vector3(i, j, 0);
                        roads.Add(new RoadsCord(i, j));
                        tiles[i, j] = tile1;

                    }
                }
            }
        }

    }
    public int CountNeighbors(Tile cell)
    {
        int count = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                if ((cell.position.x + i) >= width || (cell.position.y + j) >= height || cell.position.x + i < 0 || cell.position.y + j < 0)
                {
                    continue;
                }
                if (tiles[(Int32)(cell.position.x + i), (Int32)(cell.position.y + j)].type == Tile.Type.Road)
                {
                    count++;
                }
            }
        }
        return count;
    }
    IEnumerator CellularAutomata(int Neighbors)
    {
        for (int i = 0; i < roads.Count; i++)
        {
            roads[i].count = CountNeighbors(tiles[roads[i].x, roads[i].y]);
        }
        for (int i = 0; i < roads.Count; i++)
        {
            if (roads[i].count <= Neighbors)
            {
                tiles[roads[i].x, roads[i].y].SetTile(empty);
                roads.Remove(roads[i]);
                i--;
            }
        }
        yield return null;
    }
    IEnumerator CellularAutomata(float a)
    {
       
        for (int i = 0; i < roads.Count; i++)
        {
            roads[i].SetCount(CountNeighbors(tiles[roads[i].x, roads[i].y]));
        }
        for (int i = 0; i < roads.Count; i++)
        {
            if (roads[i].count == a)
            {
                tiles[roads[i].x, roads[i].y].SetTile(empty);
                roads.Remove(roads[i]);
                i--;
            }
        }
        yield return null;

    }
    public void DFS(bool[,] visited, int yStart, int xStart, Tile[,] cells, int k, int x, int y)
    {

        if (yStart < 0 || yStart >= height || xStart < 0 || xStart >= width)
        {
            return;
        }
        else if (visited[xStart, yStart] )
        {
            return;
        }
        else if (cells[xStart, yStart].type == Tile.Type.Empty)
        {
            if(!visited[xStart, yStart])
            {
                visited[xStart, yStart] = true;
                sideRoads.Add(new SideRoad(xStart, yStart));
            }
            return;
        }
        visited[xStart, yStart] = true;
        roadStack.Push(tiles[xStart, yStart]);

        if (y == yStart && x == xStart && visited[yStart, xStart] && k != 0)
        {
            return;
        }
        DFS(visited, yStart, xStart - 1, cells, k + 1, x, y);
        DFS(visited, yStart - 1, xStart, cells, k + 1, x, y);
        DFS(visited, yStart + 1, xStart, cells, k + 1, x, y);
        DFS(visited, yStart, xStart + 1, cells, k + 1, x, y);
    }
    IEnumerator Road()
    {
        bool isFirst = true;
        RoadsCord start = roads[1];
        int xStart = start.x;
        int yStart = start.y;
        DFS(visited, yStart, xStart, tiles, 0, xStart, yStart);
        foreach (Tile cell in roadStack)
        {
            if(isFirst)
            {
                cell.SetTile(campfire);
                Map.instance.Draw(cell);
                isFirst = false;
                continue;
            }
            Map.instance.Draw(cell);
            yield return new WaitForSeconds(0.2f);
        }
        
        Actions.StartTimer?.Invoke();
        Map.instance.playerInMap.SetActive(true);
    }
    public void NewMap()
    {
        roads.Clear();
        roadStack.Clear();
        GenerationCell();
        GridGen();
        //Clean Conner 
        StartCoroutine(CellularAutomata(4));
        StartCoroutine(CellularAutomata(3));
        StartCoroutine(CellularAutomata(4));
        StartCoroutine(CellularAutomata(3));
        StartCoroutine(CellularAutomata((float)8));
        Map.instance.Draw(tiles);
        StartCoroutine(Road());
        foreach (SideRoad sideRoad in sideRoads)
        {
            Map.instance.LoadSideRoad(sideRoad.x, sideRoad.y);
        }


    }

    [System.Serializable]
    public class RoadsCord
    {
        public int x;
        public int y;
        public int count;

        public RoadsCord(int i, int j)
        {
            this.x = i;
            this.y = j;
            this.count = 0;
        }
        public void SetCount(int count)
        {
            this.count = count;
        }

    }
    public class SideRoad
    {
        public int x;
        public int y;
        public SideRoad(int i, int j)
        {
            this.x = i;
            this.y = j;
        }
       
    }
}
