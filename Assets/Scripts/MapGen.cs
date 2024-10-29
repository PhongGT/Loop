using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    // Start is called before the first frame update


    int rows = 10;
    int cols = 10;
    int[,] matrix;
    System.Random random = new System.Random();
    public Map map;
    private void Awake()
    {
        map = GetComponentInChildren<Map>();
    }
    void Start()
    {
        matrix = new int[rows, cols];

        // Draw multiple random ellipses
        for (int k = 0; k < 5; k++) // Drawing 5 random ellipses
        {
            int centerX = random.Next(0, rows);
            int centerY = random.Next(0, cols);
            int radiusX = random.Next(1, rows / 2);
            int radiusY = random.Next(1, cols / 2);

            DrawEllipse(centerX, centerY, radiusX, radiusY);
        }

        PrintMatrix();
    }

    void DrawEllipse(int centerX, int centerY, int radiusX, int radiusY)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                double dx = (i - centerX) / (double)radiusX;
                double dy = (j - centerY) / (double)radiusY;
                if (dx * dx + dy * dy <= 1)
                {
                    matrix[i, j] = 1; // Marking the cell as part of the ellipse
                }
            }
        }
    }

    void PrintMatrix()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Debug.Log(matrix[i, j] == 1 ? "X " : ". ");
            }
            Debug.Log("\n");
        }
    }



}
