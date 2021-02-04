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
            { 1,0,1,0,0,0,0,0,0,0,1,1,0,1 },
            { 1,0,1,1,1,1,0,0,0,0,0,1,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,1,1,1,1,1,1,1,1,1,1,1,2,1 },
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
                   return IndexToWorldPosition(xi, yi);
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
                int blockType = m_maze[yi,xi];
                GameObject wall = null;
                if (blockType == 1) {
                    wall = Instantiate<GameObject>(m_resWall);                    
                }
                else if (blockType == 3) {
                    wall = Instantiate<GameObject>(m_resWallEnd);                    
                }

                if (wall != null) {
                    wall.transform.position = IndexToWorldPosition(xi, yi);
                    wall.name = yi + "," + xi;
                }
            }
        }
    }

    const float BLOCK_SIZE = 0.25f;
    public Vector3 IndexToWorldPosition (int x, int y)
    {
        return new Vector3(
            -(BLOCK_SIZE * 7) + (x * BLOCK_SIZE),
            -(-(BLOCK_SIZE * 7) + (y * BLOCK_SIZE)),
            0f
            );
    }

    private GameObject m_resWall;
    private GameObject m_resWallEnd;
    private void CacheWall ()
    {
        if (m_resWall == null) {
            m_resWall = Resources.Load<GameObject>("Wall");
        }

        if (m_resWallEnd == null) {
            m_resWallEnd = Resources.Load<GameObject>("Wall_End");
        }
    }

    internal Judgment GetBlockInfo (Vector2Int v2)
    {
        if (IsOutOfBlock(v2)) {
            //Debug.LogError("out of arry");
            return Judgment.Wall;
        }

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
                return Judgment.Wall;
        }
    }

    private bool IsOutOfBlock (Vector2Int v2)
    {
        if (v2.y < 0 || v2.y >= m_maze.GetLength(0)) {
            return true;
        }

        if (v2.x < 0 || v2.x >= m_maze.GetLength(1)) {
            return true;
        }

        return false;
    }
}
