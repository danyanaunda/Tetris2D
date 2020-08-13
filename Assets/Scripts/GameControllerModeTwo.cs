using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerModeTwo : GameController
{
    private Transform rightFigure, leftFigure;

    protected override void Start()
    {
        width = 12;
        base.Start();
    }

    protected override void RotateFigure()
    {
        targetFigure.Rotate(0, 0, 90);
        rightFigure.Rotate(0, 0, 90);
        leftFigure.Rotate(0, 0, 90);

        UpdateVisibleFigures();

        if (!ValidMove()) RotateFigure();
    }

    protected override void SpawnFigure()
    {
        base.SpawnFigure();
        rightFigure = Instantiate(targetFigure);
        rightFigure.position += Vector3.right * width;

        leftFigure = Instantiate(targetFigure);
        leftFigure.position -= Vector3.right * width;

        UpdateVisibleFigures();
    }

    protected override void FigureDown()
    {
        targetFigure.position += new Vector3(0, -1, 0);
        rightFigure.position += new Vector3(0, -1, 0);
        leftFigure.position += new Vector3(0, -1, 0);
        if (!ValidMove())
        {
            targetFigure.position -= new Vector3(0, -1, 0);
            rightFigure.position -= new Vector3(0, -1, 0);
            leftFigure.position -= new Vector3(0, -1, 0);

            BuildFigureForGrid();
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

    private void BuildFigureForGrid()
    {
        var newFigure = new GameObject(targetFigure.name).transform;
        newFigure.position = targetFigure.position;
        for (int i = 0; i < 3; i++)
        {
            Transform figure = leftFigure;
            if (i == 1) figure = targetFigure;
            if (i == 2) figure = rightFigure;

            while (figure.childCount > 0)
            {
                var child = figure.GetChild(0);
                if (child.gameObject.activeSelf) child.parent = newFigure;
                else DestroyImmediate(child.gameObject);
            }
        }

        Destroy(targetFigure.gameObject);
        Destroy(rightFigure.gameObject);
        Destroy(leftFigure.gameObject);

        targetFigure = newFigure;
    }

    protected override void MoveFigureRight()
    {
        targetFigure.position += new Vector3(1, 0, 0);
        rightFigure.position += new Vector3(1, 0, 0);
        leftFigure.position += new Vector3(1, 0, 0);

        UniteFigure();
        UpdateVisibleFigures();
        if (!ValidMove())  MoveFigureLeft();

        }
    protected override void MoveFigureLeft()
    {
        targetFigure.position += new Vector3(-1, 0, 0);
        rightFigure.position += new Vector3(-1, 0, 0);
        leftFigure.position += new Vector3(-1, 0, 0);

        UniteFigure();
        UpdateVisibleFigures();
        if (!ValidMove()) MoveFigureRight();
    }

    private void UpdateVisibleFigures()
    {
        UpdateVisible(targetFigure);
        UpdateVisible(rightFigure);
        UpdateVisible(leftFigure);
    }

    private bool IsFigureVisible(Transform figure)
    {
        for (int i = 0; i < figure.childCount; i++)
        {
            var child = figure.GetChild(i);
            bool isVisible = (int)child.position.x >= 0 && (int)child.position.x < width;
            if (isVisible) return true;
        }
        return false;
    }

    private void UpdateVisible(Transform figure)
    {
        foreach (Transform child in figure)
        {
            bool isVisible = Mathf.Round(child.position.x) > -1 && (int)child.position.x < width;
            child.gameObject.SetActive(isVisible);
        }
    }


    void UniteFigure()
    {
        if (!IsFigureVisible(targetFigure))
        {
            var figure = targetFigure;
            if (IsFigureVisible(leftFigure))
            {
                targetFigure = leftFigure;
                leftFigure = figure;
            }
            else
            {
                targetFigure = rightFigure;
                rightFigure = figure;
            }
            CreateHiddenFigures();
        }
    }

    protected virtual void CreateHiddenFigures()
    {
        if (leftFigure != null) DestroyImmediate(leftFigure.gameObject);
        if (rightFigure != null) DestroyImmediate(rightFigure.gameObject);

        rightFigure = Instantiate(targetFigure);
        rightFigure.position += Vector3.right * width;

        leftFigure = Instantiate(targetFigure);
        leftFigure.position -= Vector3.right * width;
    }

    public override void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasTwoLine(i))
            {
                DeleteLine(i);
                RowDown(i);
                DeleteLine(i);
                RowDown(i);
                scoreController.AddScore(100);
            }
        }
    }
    
    public override bool ValidMove()
    {
        for (int i = 0; i < 3; i++)
        {
            var figure = leftFigure;
            if (i == 1) figure = rightFigure;
            if (i == 2) figure = targetFigure;

            foreach (Transform children in figure)
            {
                int roundedX = Mathf.RoundToInt(children.transform.position.x);
                int roundedY = Mathf.RoundToInt(children.transform.position.y);

                if (roundedY < 0 || roundedY >= height)
                {
                    return false;
                }
                if (children.gameObject.activeSelf)
                {
                    if (grid[roundedX, roundedY] != null)
                        return false;
                }
            }
        }
        return true;
    }
    

    public override void RestartGame()
    {
        SceneManager.LoadScene("ModeTwo");
        ScoreController.lastScore = 0;
    }
}
