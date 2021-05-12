using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{

    [Header("Objects")]
    [SerializeField] GameObject balloon;
    [SerializeField] GameObject launchPad;
    [SerializeField] GameObject landingPad;
    [SerializeField] GameObject mainCamera;
    [SerializeField] EnemySpawner enemySpawner;

    [Header("Object Settings")]
    [SerializeField] Quadrant playerStart;
    [SerializeField] Quadrant playerFinish;
    [SerializeField] int balloonOffset = 0;
    [SerializeField] int startPlatformOffset = 0;
    [SerializeField] int finishOffset = 2;
    [SerializeField] int cameraZOffset = -25;
    [SerializeField] int spawnPortion = 4;
    [SerializeField] int finishPortion = 4;

    private int[,] map = null;
    private Transform balloonTransform;
    private int width = 0;
    private int height = 0;
    private float squareSize = 0.0f;

    enum Quadrant { TopRight, TopLeft, BottomLeft, BottomRight }

    public void InitializeGameObjects(int[,] map, int width, int height)
    {
        this.map = map;
        this.width = width;
        this.height = height;
        squareSize = MapGenerator.squareSize;

        SetSpawnLocation();
        SetFinishLocation();
        CallSpawnEnemies();
        InitializeDisplay();
    }

    private void InitializeDisplay()
    {
        int pointsFactor = Display.SIZE_FACTOR_FOR_POINTS;

        Display display = FindObjectOfType<Display>();
        display.SetLevelPoints((height + width) * pointsFactor);
    }

    private void SetSpawnLocation()
    {
        bool isPlayerStart = true;

        Coord center = GetCenter(playerStart, isPlayerStart);

        int centerX = center.tileX;
        int centerY = center.tileY;

        float xPos = ((-width / 2) + centerX) * squareSize;
        float yPos = ((-height / 2) + centerY) * squareSize;
        InitializeBalloonAndPlatform(xPos, yPos);
    }

    private void SetFinishLocation()
    {
        bool isPlayerStart = false;
        Coord center = GetCenter(playerFinish, isPlayerStart);

        int centerX = center.tileX;
        int centerY = center.tileY;

        float xPos = ((-width / 2) + centerX) * squareSize;
        float yPos = ((-height / 2) + centerY) * squareSize;
        InitializeLandingPad(xPos, yPos);
    }

    public void CallSpawnEnemies()
    {
        enemySpawner.InstantiateEnemies(map, width, height, balloonTransform);
    }

    public void InitializeBalloonAndPlatform(float xPos, float yPos)
    {
        Vector3 balloonStartPos = new Vector3(xPos, yPos + balloonOffset, 0);
        Vector3 launchPadStartPos = new Vector3(xPos, yPos + startPlatformOffset, 0);
        Vector3 cameraStartPos = new Vector3(xPos, yPos + balloonOffset, cameraZOffset);

        GameObject thisBalloon = Instantiate(balloon, balloonStartPos, Quaternion.identity);
        GameObject thisCamera = Instantiate(mainCamera, cameraStartPos, Quaternion.identity);

        Instantiate(launchPad, launchPadStartPos, Quaternion.identity);

        CameraFollow cameraFollow = thisCamera.GetComponent<CameraFollow>();
        balloonTransform = thisBalloon.transform;
        cameraFollow.setTarget(balloonTransform);
    }

    public void InitializeLandingPad(float xPos, float yPos)
    {
        Vector3 platformStartPos = new Vector3(xPos, yPos + finishOffset, 0);
        Instantiate(landingPad, platformStartPos, Quaternion.identity);
    }

    private Coord GetCenter(Quadrant quadrant, bool isPlayerStart)
    {
        (int topBound, int bottomBound, int leftBound, int rightBound) = GetRegionBounds(quadrant, isPlayerStart);

        Coord rsf = new Coord(0, 0);
        int leftRsf = 0;
        int topRsf = 0;

        for (int y = bottomBound; y < topBound; y++)
        {
            for (int x = leftBound; x < rightBound; x++)
            {
                if (map[x, y] == 0 && !IsEdge(x, y))
                {
                    int left = CountLeft(x, y, leftBound);
                    int right = CountRight(x, y, rightBound);
                    int top = CountTop(x, y, topBound);
                    int bottom = CountBottom(x, y, bottomBound);

                    if (Mathf.Abs(left - right) <= 1 && Mathf.Abs(top - bottom) <= 1)
                    {
                        if (left >= leftRsf && top >= topRsf)
                        {
                            leftRsf = left;
                            topRsf = top;

                            rsf = new Coord(x, y);
                        }
                    }
                }
            }
        }
        return rsf;
    }

    private int CountLeft(int x, int y, int leftBound)
    {
        int left = 0;
        for (int i = x - 1; i >= leftBound; i--)
        {
            if (map[i, y] == 0)
            {
                left++;
            }
            else if (map[i, y] == 1)
            {
                break;
            }
        }
        return left;
    }

    private int CountRight(int x, int y, int rightBound)
    {
        int right = 0;
        for (int i = x + 1; i < rightBound; i++)
        {
            if (map[i, y] == 0)
            {
                right++;
            }
            else if (map[i, y] == 1)
            {
                break;
            }
        }
        return right;
    }

    private int CountTop(int x, int y, int topBound)
    {
        int top = 0;

        for (int i = y + 1; i < topBound; i++)
        {
            if (map[x, i] == 0)
            {
                top++;
            }
            else if (map[x, i] == 1)
            {
                break;
            }
        }
        return top;
    }

    private int CountBottom(int x, int y, int bottomBound)
    {
        int bottom = 0;
        for (int i = y - 1; i >= bottomBound; i--)
        {
            if (map[x, i] == 0)
            {
                bottom++;
            }
            else if (map[x, i] == 1)
            {
                break;
            }
        }
        return bottom;
    }

    private (int, int, int, int) GetRegionBounds(Quadrant quadrant, bool isPlayerStart)
    {
        int top = height;
        int bottom = 0;
        int left = 0;
        int right = width;

        int portion = isPlayerStart ? spawnPortion : finishPortion;

        switch (quadrant)
        {
            case Quadrant.BottomLeft:
                top = height / portion;
                right = width / portion;
                break;
            case Quadrant.TopLeft:
                bottom = height - (height / portion);
                right = width / portion;
                break;
            case Quadrant.TopRight:
                bottom = height - (height / portion);
                left = width - (width / portion);
                break;
            default:
                top = height / portion;
                left = width - (width / portion);
                break;
        }

        return (top, bottom, left, right);
    }

    private bool IsEdge(int x, int y)
    {
        bool isEdge = true;
        int yBound = height - 1;
        int xBound = width - 1;

        if (x > 0 && y > 0 && x < xBound && y < yBound)
        {
            isEdge = false;

            for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {
                    if (map[i, j] == 1)
                    {
                        isEdge = true;
                    }
                }
            }
        }
        return isEdge;
    }

    struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }
}