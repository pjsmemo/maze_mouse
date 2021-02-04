using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    // index 밖은 벽.
    // 벽을 만나면 승리.
    // 시작 포인트에서 만나는 벽은 아무것도 아님.
    // 0 : 길
    // 1 : 벽
    // 2 : 시작
    // 3 : 종료.
    public int [,] m_maze = new int[,]
        {
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            { 3,0,1,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,1,0,1,0,1,0,1,1,1,1,0,1 },
            { 1,0,0,0,1,0,1,0,0,0,0,1,0,1 },
            { 1,1,1,1,1,0,1,0,0,1,1,1,0,1 },
            { 1,0,0,0,0,0,1,0,0,1,0,0,0,1 },
            { 1,0,1,1,1,0,1,0,0,1,0,0,0,1 },
            { 1,0,1,0,1,0,0,0,0,0,0,1,1,1 },
            { 1,0,1,0,1,1,1,1,1,1,0,1,0,1 },
            { 1,0,1,0,0,0,0,0,0,0,0,1,0,1 },
            { 1,0,1,0,0,0,0,0,0,0,0,1,0,1 },
            { 1,0,1,1,1,1,0,0,0,0,0,1,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,2,1 },
            { 1,1,1,1,1,1,1,1,1,1,1,1,3,1 },
        };

    internal int GetStartIndexY ()
    {
        for (int yi = 0; yi < m_maze.GetLength(0); yi++) {
            for (int xi = 0; xi < m_maze.GetLength(1); xi++) {
                if (m_maze[yi, xi] == 2) {
                    return yi;
                }
            }
        }

        Debug.LogError("there is no start index y");

        return 0;
    }

    internal int GetStartIndexX ()
    {
        for (int yi = 0; yi < m_maze.GetLength(0); yi++) {
            for (int xi = 0; xi < m_maze.GetLength(1); xi++) {
                if (m_maze[yi, xi] == 2) {
                    return xi;
                }
            }
        }

        Debug.LogError("there is no start index x");

        return 0;
    }

    internal Vector3 GetStartPosition ()
    {
        for (int yi = 0; yi < m_maze.GetLength(0); yi++) {
            for (int xi = 0; xi < m_maze.GetLength(1); xi++) {
                if (m_maze[yi, xi] == 2) {
                   return GetPosition(xi, yi);
                }
            }
        }

        Debug.LogError("there is no start position");

        return Vector3.zero;
    }

    internal void CreateMaze ()
    {
        CacheWall();

        for (int yi = 0; yi < m_maze.GetLength(0); yi++) {
            for (int xi = 0; xi < m_maze.GetLength(1); xi++) {
                if (m_maze[yi,xi] == 1) {
                    GameObject wall = Instantiate<GameObject>(m_resWall);
                    wall.transform.position = GetPosition(xi, yi);
                    wall.name = yi + "," + xi;
                }
            }
        }
    }

    const float BLOCK_SIZE = 0.25f;
    public Vector3 GetPosition (int x, int y)
    {
        return new Vector3(
            -(BLOCK_SIZE * 7) + (x * BLOCK_SIZE),
            -(-(BLOCK_SIZE * 7) + (y * BLOCK_SIZE)),
            0f
            );
    }

    private GameObject m_resWall;
    private void CacheWall ()
    {
        if (m_resWall == null) {
            m_resWall = Resources.Load<GameObject>("Wall");
        }
    }

    internal Judgment GetWayInfo (Vector2Int v2)
    {
        int n = m_maze[v2.y, v2.x];
        switch (n) {
            case 0:
                return Judgment.Space;

            case 1:
                return Judgment.Wall;

            case 2:
                return Judgment.Start;

            case 3:
                return Judgment.End;

            default:
                Debug.LogError("??");
                return Judgment.Null;
        }
    }
}
