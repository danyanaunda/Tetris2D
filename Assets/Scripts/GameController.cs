using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public static bool IsGameJustOver;
    protected float previousTime;
    public static float fallTime = 1f;
    public int height = 20;
    public int width = 10;
    protected Transform[,] grid;
    protected Transform targetFigure;
    [SerializeField] private SpawnTetromino spawner;
    public ScoreController scoreController;


    virtual protected void Start()
    {
        grid = new Transform[width, height];
        IsGameJustOver = false;
        SpawnFigure();
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveFigureLeft();
        else if (Input.GetKeyDown(KeyCode.RightArrow)) MoveFigureRight();
        else if (Input.GetKeyDown(KeyCode.UpArrow)) RotateFigure();

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            FigureDown();

        }
    }

    virtual protected void SpawnFigure()
    {
        targetFigure = spawner.NewTetromino().transform;
    }

    virtual protected void FigureDown()
    {
        targetFigure.position += new Vector3(0, -1, 0);
        if (!ValidMove())
        {
            targetFigure.position -= new Vector3(0, -1, 0);
            AddToGrid();
            CheckForLines();


            if (IsGameOver())
            {
                IsGameJustOver = true;
                return;
            }
            scoreController.AddScore(10);
            SpawnFigure();

        }
        previousTime = Time.time;
    }

    virtual protected void RotateFigure()
    {
        targetFigure.Rotate(0, 0, 90);
        if (!ValidMove()) RotateFigure();
    }

    virtual protected void MoveFigureRight()
    {
        targetFigure.position += new Vector3(1, 0, 0);
        if (!ValidMove())
            targetFigure.position -= new Vector3(1, 0, 0);
    }

    virtual protected void MoveFigureLeft()
    {
        targetFigure.position += new Vector3(-1, 0, 0);
        if (!ValidMove())
            targetFigure.position -= new Vector3(-1, 0, 0);
    }

    virtual public void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                scoreController.AddScore(100);
                RowDown(i);
            }
        }

    }

    public bool IsGameOver()
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, height - 1] != null)
            {
                enabled = false;
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
                return true;
            }
        }
        return false;
    }

    public bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    public bool HasTwoLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null) return false;
            if (grid[j, i + 1] == null) return false;
        }
        return true;
    }

    public void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    public void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }


    public void AddToGrid()
    {
        foreach (Transform children in targetFigure)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            grid[roundedX, roundedY] = children;
        }
    }

    virtual public bool ValidMove()
    {
        foreach (Transform children in targetFigure)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
                return false;

        }

        return true;
    }

    public virtual void RestartGame()
    {
        SceneManager.LoadScene("last");
        ScoreController.lastScore = 0;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }


}