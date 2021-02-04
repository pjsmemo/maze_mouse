using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Maze m_maze;
    private Mouse m_mouse;

    void Start()
    {
        m_maze = gameObject.AddComponent<Maze>();

        m_maze.CreateMaze();

        m_mouse = Mouse.Create(m_maze);

        m_mouse.FindDoor();
    }
}
