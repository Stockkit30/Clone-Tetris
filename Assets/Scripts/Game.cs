using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 22;

    // 2D ����� ��� ��������� ����� � ��
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    // ��������, �� ���������� ����������� �������� ����
    public bool CheckIsInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0 && (int)pos.y < gridHeight);
    }

    // �������� ���������� �� ������ ������� ����
    public Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    // ������� ����, ������� �������� Tetromino
    public void UpdateGrid(Tetromino tetromino)
    {
        // ��������� ���� ������� Tetromino � ����
        for (int y = 0; y < gridHeight; ++y)
        {
            for (int x = 0; x < gridWidth; ++x)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetromino.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }

        // ������ ��� ������� Tetromino �� ����
        foreach (Transform mino in tetromino.transform)
        {
            Vector2 pos = Round(mino.position);
            if (CheckIsInsideGrid(pos))
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    // ������� �������� ����
    public void CheckAndDeleteFullRows()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                MoveRowsDown(y);
                y--; // ��������� ��� ����� ����� �� ��� ���� �������
            }
        }
    }

    // ��������, �� ���������� �����
    bool IsRowFull(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    // ������� ������ �����
    void DeleteRow(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    // ���� �� ����� ��� ��������� ����
    void MoveRowsDown(int startY)
    {
        for (int y = startY; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    // ������ Transform ������ �� ���� ������� � ����
    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y > gridHeight - 1) return null;
        return grid[(int)pos.x, (int)pos.y];
    }

    void Start()
    {
        // ����������� ���
    }

    void Update()
    {
        // ������� ����� ���
    }
}
