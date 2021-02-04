using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{

    public static Mouse Create(Maze maze)
    {
        GameObject res = Resources.Load<GameObject>("Mouse");
        GameObject go = Instantiate<GameObject>(res);

        Mouse mouse = go.GetComponent<Mouse>();

        mouse.Initlize();
        mouse.SetMaze(maze);
        mouse.ResetPosition();

        return mouse;
    }

    private Transform m_tn;
    private Maze m_maze;


    private int m_posX = 0;
    private int m_posY = 0;
    private Way m_dir = Way.Up;

    private const int RIGHT_INDEX = 0;
    private const int UP_INDEX = 1;
    private const int LEFT_INDEX = 2;
    private const int BACK_INDEX = 3;

    private void Initlize ()
    {
        m_tn = transform;
    }


    private void ResetPosition ()
    {
        SetIndexByStartPosition();
        m_tn.position = m_maze.GetPosition(m_posX, m_posY);
    }

    private void SetIndexByStartPosition ()
    {
        m_posX = m_maze.GetStartIndexX();
        m_posY = m_maze.GetStartIndexY();
        m_dir = Way.Up;
    }

    private void SetMaze (Maze maze)
    {
        m_maze = maze;
    }



    internal void FindDoor ()
    {
        StartCoroutine(FindDoorRoutine());
    }


    

    enum Way
    {
        Right = 0,
        Up = 1,
        Left = 2,
        Back = 3,
    }


    private int [,] m_mouseDirUp = new int[,]
        {
            // y, x
            { 0,1 },  // right
            { 1,0 },  // up
            { 0,-1 }, // left
            { -1,0 }, // back
        };

    private int [,] m_mouseDirRight = new int[,]
        {
            // y, x
            { -1,0 },  // right
            { 0,1 },  // up
            { 1,0 }, // left
            { 0,-1 }, // back
        };

    private int [,] m_mouseDirLeft = new int[,]
        {
            // y, x
            { 1,0 },  // right
            { 0,-1 },  // up
            { -1,0 }, // left
            { 0,1 }, // back
        };

    private int [,] m_mouseDirDown = new int[,]
        {
            // y, x
            { 0,-1 },  // right
            { -1,0 },  // up
            { 0,1 }, // left
            { 1,0 }, // back
        };

    private int [,] m_arrowIndex = new int[,]
        {
            { 1,0 },  // right
            { 0,1 },  // up
            { -1,0 }, // left
            { 0,-1 }, // back
        };


    IEnumerator FindDoorRoutine ()
    {
        while (true) {
            yield return new WaitForSeconds(0.2f);

            // 길찾기.
            // right 검사.
            Debug.Log(JudgeWay(Way.Right));
            Debug.Log(JudgeWay(Way.Left));
            Debug.Log(JudgeWay(Way.Up));
            Debug.Log(JudgeWay(Way.Back));
            // up 검사.
            // left 검사.
            // back 검사.

            // 방향 전환.

            // 이동.

            // 출구이면 종료.

            // 아니면 반복. 

            break;
        }
    }

    private Judgment JudgeWay (Way judgeWay)
    {
        Vector2Int v2 = WayToIndex(m_dir, judgeWay);

        Debug.Log("m_dir : " + m_dir);
        Debug.Log("way : " + judgeWay);
        Debug.Log("v2 : " + v2);

        v2.x += m_posX;
        v2.y += m_posY;

        return m_maze.GetWayInfo(v2);
    }

    private Vector2Int WayToIndex (Way dir, Way way)
    {
        switch (dir) {
            case Way.Right: {
                int y = m_mouseDirRight[(int)way, 0];
                int x = m_mouseDirRight[(int)way, 1];
                return new Vector2Int(x, y);
            }

            case Way.Up: {
                int y = m_mouseDirUp[(int)way, 0];
                int x = m_mouseDirUp[(int)way, 1];
                return new Vector2Int(x, y);
            }

            default:
                return Vector2Int.zero;
        }
    }
}
