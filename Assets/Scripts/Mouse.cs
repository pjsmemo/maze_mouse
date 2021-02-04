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


    private Vector2Int m_pos = new Vector2Int(0,0);
    private Vector2Int m_dir = new Vector2Int(0,1);

    private float UP_DEGREE = 0f;
    private float RIGHT_DEGREE = -90f;
    private float BACK_DEGREE = 180f;
    private float LEFT_DEGREE = 90f;


    private void Initlize ()
    {
        m_tn = transform;
    }


    private void ResetPosition ()
    {
        SetIndexByStartPosition();

        SyncTransform();        
    }

    void SyncTransform ()
    {
        m_tn.position = m_maze.IndexToWorldPosition(m_pos.x, m_pos.y);

    }

    private void SetIndexByStartPosition ()
    {
        m_pos.x = m_maze.GetStartIndexX();
        m_pos.y = m_maze.GetStartIndexY();
        
        m_dir = new Vector2Int(0, 1);
        RotationHead(Direction.Up);
    }

    private void SetMaze (Maze maze)
    {
        m_maze = maze;
    }



    internal void FindDoor ()
    {
        StartCoroutine(FindDoorRoutine());
    }


    

    enum Direction
    {
        Right = 0,
        Up = 1,
        Left = 2,
        Back = 3,
    }



    IEnumerator FindDoorRoutine ()
    {
        while (true) {
            yield return new WaitForSeconds(0.1f);

            // 길찾기.
            // right 검사.
            if (JudgeDirection(Direction.Right) != Judgment.Wall) {
                yield return MoveRoutine(Direction.Right);
            } else if (JudgeDirection(Direction.Up) != Judgment.Wall) {
                yield return MoveRoutine(Direction.Up);
            } else if (JudgeDirection(Direction.Left) != Judgment.Wall) {
                yield return MoveRoutine(Direction.Left);
            } else if (JudgeDirection(Direction.Back) != Judgment.Wall) {
                yield return MoveRoutine(Direction.Back);
            } else {
                Debug.LogError("err");
            }

            // 출구이면 종료.
            if ( m_maze.GetBlockInfo(m_pos) == Judgment.End) {
                Debug.LogError("finish");
                break;
            }

            // 아니면 반복. 
        }
    }

    IEnumerator MoveRoutine (Direction dir)
    {
        RotationHead(dir);
        yield return new WaitForSeconds(0.1f);
        MoveFoward();
        yield return new WaitForSeconds(0.1f);
    }

    void RotationHead (Direction dir)
    {
        m_dir = RotationIndex(m_dir, dir);


        float angle = 0;

        if (m_dir.x == 0) {
            if (m_dir.y == 1) {
                angle = 0;
            }
            else {
                angle = 180;
            }
        }
        else {
            if (m_dir.x == 1) {
                angle = -90f;
            }
            else {
                angle = 90;
            }
        }
        
        m_tn.rotation = Quaternion.Euler(0, 0, angle);
    }

    void MoveFoward ()
    {
        m_pos.x = m_pos.x + m_dir.x;
        m_pos.y = m_pos.y - m_dir.y;
        SyncTransform();
    }

    private Judgment JudgeDirection (Direction judgeWay)
    {
        //Debug.Log("@dir : " + m_dir);
        //Debug.Log("@judgeWay : " + judgeWay);

        Vector2Int v2 = RotationIndex(m_dir, judgeWay);

        //Debug.Log("@v2-1 : " + v2);

        v2.x = m_pos.x + v2.x;
        v2.y = m_pos.y - v2.y;

        //Debug.Log("@v2-2 : " + v2);

        return m_maze.GetBlockInfo(v2);
    }

    public static Vector2 Rotate (Vector2 v, float degree)
    {
        float r = degree * Mathf.Deg2Rad;

        return new Vector2(
            v.x * Mathf.Cos(r) - v.y * Mathf.Sin(r),
            v.x * Mathf.Sin(r) + v.y * Mathf.Cos(r)
        );
    }

    private Vector2Int RotationIndex (Vector2Int fromV2, Direction toDir)
    {
        Vector2 v = Rotate(fromV2, DirToDegree(toDir));

        return new Vector2Int((int)v.x, (int)v.y);
    }

    private float DirToDegree (Direction dir)
    {
        switch (dir) {
            case Direction.Up:
                return UP_DEGREE;

            case Direction.Right:
                return RIGHT_DEGREE;

            case Direction.Back:
                return BACK_DEGREE;

            case Direction.Left:
                return LEFT_DEGREE;

            default:
                return 0;
        }
    }
}
