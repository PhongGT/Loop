﻿using System.Collections;
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
    [SerializeField] public Stack<Cell> roadStack = new Stack<Cell>();
    public bool[,] visited;
    public Slot slot;
    int width = 17;
    int height = 12;
    public Cell[,] cells;

    public Map map;
    private void Awake()
    {
        map = GetComponentInChildren<Map>();
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
        cells = new Cell[width, height];
        visited = new bool[width, height];
    }
    public void GenerationCell()
    {
        NewGame();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cell cell = new Cell();
                cell.position = new Vector3Int(i, j, 0);
                cell.type = Cell.Type.Empty;
                cells[i, j] = cell;
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
                    Cell cell = new Cell();
                    cell.position = new Vector3Int(i, j, 0);
                    cell.type = Cell.Type.Road;
                    cells[i, j] = cell;
                    roads.Add(new RoadsCord(i, j));
                }
                else if (distance < 5.4)
                {
                    if (Random.Range(0, 4) < 3)
                    {
                        Cell cell1 = new Cell();
                        cell1.position = new Vector3Int(i, j, 0);
                        cell1.type = Cell.Type.Road;
                        roads.Add(new RoadsCord(i, j));
                        cells[i, j] = cell1;

                    }
                }
            }
        }
        cells[0, 0].type = Cell.Type.Beacon;

    }
    public int CountNeighbors(Cell cell)
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
                if (cells[cell.position.x + i, cell.position.y + j].type == Cell.Type.Road)
                {
                    count++;
                }
            }
        }
        return count;
    }
    public void CellularAutomata(int Neighbors)
    {
        for (int i = 0; i < roads.Count; i++)
        {
            roads[i].count = CountNeighbors(cells[roads[i].x, roads[i].y]);
        }
        for (int i = 0; i < roads.Count; i++)
        {
            if (roads[i].count <= Neighbors)
            {
                cells[roads[i].x, roads[i].y].type = Cell.Type.Empty;
                roads.Remove(roads[i]);
                i--;
            }
        }
        //Debug.Log(JsonConvert.SerializeObject(roads, Formatting.Indented));
    }
    public void CellularAutomata(float a)
    {

        for (int i = 0; i < roads.Count; i++)
        {
            roads[i].SetCount(CountNeighbors(cells[roads[i].x, roads[i].y]));
        }
        for (int i = 0; i < roads.Count; i++)
        {
            if (roads[i].count == a)
            {
                cells[roads[i].x, roads[i].y].type = Cell.Type.Empty;
                roads.Remove(roads[i]);
                i--;
            }
        }

    }
    public void DFS(bool[,] visited, int yStart, int xStart, Cell[,] cells, int k, int x, int y)
    {
        Debug.Log(yStart + "  " +xStart);
        if (yStart < 0 || yStart >= height || xStart < 0 || xStart >= width )
        {
            return;
        }
        else if(cells[xStart, yStart].type != Cell.Type.Road || visited[xStart, yStart])
        { return; }
        visited[xStart, yStart] = true;
        roadStack.Push(cells[xStart, yStart]);
        Debug.Log(" X: " + xStart + " Y: " + yStart);
        if (y == yStart && x == xStart && visited[yStart, xStart] && k!=0)
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

        RoadsCord start = roads[1];
        int xStart = start.x;
        int yStart = start.y;
        DFS(visited, yStart, xStart, cells, 0, xStart, yStart);


        foreach (Cell cell in roadStack)
        {
            map.Draw(cell);
            yield return new WaitForSeconds(0.3f);
        }
    }
    public void NewMap()
    {

        roads.Clear();
        roadStack.Clear();
        
        GenerationCell();
        GridGen();
        map.Draw(cells);
        //Clean Conner 
        CellularAutomata(4);
        CellularAutomata(3);
        CellularAutomata(4);
        CellularAutomata(3);
        CellularAutomata((float)8);
        StartCoroutine(Road());
        Debug.Log(JsonConvert.SerializeObject(roadStack, Formatting.Indented));


        Debug.Log("New");


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
}
