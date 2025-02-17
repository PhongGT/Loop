using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] int rows = 21;
    [SerializeField] int cols = 12;
    Cell[,] nodes;
    System.Random random = new System.Random();
    private Plane plane;
    [SerializeField] protected Vector3 mousePos;
    [SerializeField] protected 

    private void Start()
    {
        CreateGrid();
        plane = new Plane(Vector3.up, transform.position);
    }
    void GetMousePosOnGrid()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray, out var enter))
        {
            mousePos = ray.GetPoint(enter);
            print(mousePos);
        }
    }
    private void CreateGrid()
    {
        nodes = new Cell[rows, cols];
        for(int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 worldPos = new Vector3(i, j, 0);
            }
        }
    }




}
