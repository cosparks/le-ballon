using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool instantiateEnemy1 = true;
    [SerializeField] Sector enemy1Sector = Sector.MiddleCenter;

    [SerializeField] bool instantiateEnemy2 = false;
    [SerializeField] Sector enemy2Sector = Sector.BottomLeft;

    [SerializeField] bool instantiateEnemy3 = false;
    [SerializeField] Sector enemy3Sector = Sector.MiddleRight;

    [SerializeField] bool instantiateEnemy4 = false;
    [SerializeField] Sector enemy4Sector = Sector.TopRight;

    [SerializeField] float attackDistance = 20f;
    [SerializeField] GameObject enemy;

    // private GameObject enemy;
    private Transform target;
    private List<Coord> enemySpawnCoords;
    private int[,] map;
    private int width;
    private int height;


    private enum Sector { TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight };


    public void InstantiateEnemies(int[,] map, int width, int height, Transform target)
    {
        this.map = map;
        this.width = width;
        this.height = height;
        this.target = target;

        CreateCoordsList();

        foreach (Coord location in enemySpawnCoords)
        {
            SpawnEnemy(location);
        }
    }

    private void CreateCoordsList()
    {
        enemySpawnCoords = new List<Coord>();

        if (instantiateEnemy1)
        {
            enemySpawnCoords.Add(GetCenter(enemy1Sector));
        }
        if (instantiateEnemy2)
        {
            enemySpawnCoords.Add(GetCenter(enemy2Sector));
        }
        if (instantiateEnemy3)
        {
            enemySpawnCoords.Add(GetCenter(enemy3Sector));
        }
        if (instantiateEnemy4)
        {
            enemySpawnCoords.Add(GetCenter(enemy4Sector));
        }
    }

        private void SpawnEnemy(Coord location)
    {
        float xPos = ((-width / 2) + location.tileX) * MapGenerator.squareSize;
        float yPos = ((-height / 2) + location.tileY) * MapGenerator.squareSize;

        Vector3 enemySpawnPoint = new Vector3(xPos, yPos, 0);
        GameObject tack = Instantiate(enemy, enemySpawnPoint, Quaternion.identity);
        InitializeEnemySettings(tack.GetComponent<TackPhysics>());
    }

    private void InitializeEnemySettings(TackPhysics tackPhysics)
    {
        tackPhysics.SetTarget(target);
        tackPhysics.SetAttackDistance(attackDistance);
    }

    private Coord GetCenter(Sector sector)
    {
        (int topBound, int bottomBound, int leftBound, int rightBound) = GetRegionBounds(sector);

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

    private (int, int, int, int) GetRegionBounds(Sector sector)
    {
        int top = height;
        int bottom = 0;
        int left = 0;
        int right = width;

        switch (sector)
        {
            case Sector.TopLeft:
                bottom = height - (height / 3);
                right = width / 3;
                break;
            case Sector.TopCenter:
                bottom = height - (height / 3);
                left = width / 3;
                right = (width / 3) * 2;
                break;
            case Sector.TopRight:
                bottom = height - (height / 3);
                left = width - (width / 3);
                break;
            case Sector.MiddleLeft:
                bottom = height / 3;
                top = height - (height / 3);
                right = width / 3;
                break;
            case Sector.MiddleCenter:
                bottom = height / 3;
                top = height - (height / 3);
                left = width / 3;
                right = width - (width / 3);
                break;
            case Sector.MiddleRight:
                bottom = height / 3;
                top = height - (height / 3);
                left = width - (width / 3);
                break;
            case Sector.BottomLeft:
                top = height / 3;
                right = width / 3;
                break;
            case Sector.BottomCenter:
                top = height / 3;
                left = width / 3;
                right = width - (width / 3);
                break;
            default:
                top = height / 3;
                left = width - (width / 3);
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
