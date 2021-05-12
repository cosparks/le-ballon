using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float zOffset = -2f;
    [SerializeField] int passageRadius = 3;
    [SerializeField] bool drawPassageLines = true;
    [SerializeField] int borderSize = 10;
    [SerializeField] int wallThresholdSize = 30;
    [SerializeField] int roomThresholdSize = 30;

    [SerializeField] string seed;
    [SerializeField] public bool useRandomSeed;

    [Range(0, 100)] [SerializeField] int randomFillPercent;

    [SerializeField] int smoothValue = 5;
    [SerializeField] int criticalTileDensity = 4;
    [SerializeField] bool animateSmoothIterations = false;
    [SerializeField] float frameRate = 2f;

    public static float squareSize = 1.0f;

    private float counter;
    private int[,] map;
    private int[,] map2;

    void Start()
    {
        counter = frameRate;
        GenerateMap();
    }

    private void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        InitializeSecondMap();

        for (int i = 0; i < smoothValue; i++)
        {
            SmoothMap();
            map = map2;
        }

        DisplayMap();
    }

    private void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandomNumber = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x,y] = (pseudoRandomNumber.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    private void InitializeSecondMap()
    {
        map2 = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int value = map[x, y];
                map2[x, y] = value;
            }
        }
    }

    private void SmoothMap()
    {
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                int neighborWallTiles = GetNeighborCount(x, y);

                if (neighborWallTiles > criticalTileDensity)
                {
                    map2[x, y] = 1;
                }
                else if (neighborWallTiles < criticalTileDensity)
                {
                    map2[x, y] = 0;
                }
            }
        }
    }

    private void DisplayMap()
    {
        var whiteCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        whiteCube.name = "room_tile";
        whiteCube.GetComponent<Renderer>().material.color = Color.white;

        var blackCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        whiteCube.name = "wall_tile";
        blackCube.GetComponent<Renderer>().material.color = Color.black;

        float leftSide = (-width / 2) * squareSize;
        float bottomSide = (-height / 2) * squareSize;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xPos = leftSide + (x * squareSize);
                float yPos = bottomSide + (y * squareSize);
                Vector3 cubePos = new Vector3(xPos, yPos, 0);

                if (map[x, y] == 1)
                {
                    Instantiate(blackCube, cubePos, Quaternion.identity);
                }
                else
                {
                    Instantiate(whiteCube, cubePos, Quaternion.identity);
                }
                
            }
        }
    }

    private int GetNeighborCount(int tileX, int tileY)
    {
        int count = 0;

        for (int x = tileX - 1; x < tileX + 2; x++)
        {
            for (int y = tileY - 1; y < tileY + 2; y++)
            {
                if (TileInMap(x, y))
                {
                    count += map[x, y];
                }
            }
        }
        return count;
    }

    private bool TileInMap(int tileX, int tileY)
    {
        return (tileX >= 0 && tileX < width && tileY >= 0 && tileY < height);
    }
}
