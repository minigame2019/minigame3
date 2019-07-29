using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public int smooth = 5;
    public int difficulty;
    public string seed;
    public bool useRandomSeed;
    [Range(0f, 100f)]
    public int randomFillPercent;
    private int[,] map;
    private List<Vector2> enemySpawns = new List<Vector2>();
    private List<int> usedEnemySpawns = new List<int>();

    private void GenerateBlocks()
    {
        int num = 0;
        while (num < this.width)
        {
            int num2 = 0;
            while (true)
            {
                if (num2 >= this.height)
                {
                    num++;
                    break;
                }
                if (this.map[num, num2] == 1)
                {
                    Instantiate<GameObject>(GameManager.Instance.GamePrefabs.RegularWall, new Vector3((float)(num - (this.width / 2)), 0f, (float)(num2 - (this.height / 2))), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer.transform);
                }
                else if (this.map[num, num2] == 2)
                {
                    Instantiate<GameObject>(GameManager.Instance.GamePrefabs.DestructableWall, new Vector3((float)(num - (this.width / 2)), 0f, (float)(num2 - (this.height / 2))), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer.transform);
                }
                else if (this.map[num, num2] == 3)
                {
                    this.enemySpawns.Add(new Vector2((float)(num - (this.width / 2)), (float)(num2 - (this.height / 2))));
                }
                else if (this.map[num, num2] == 9)
                {
                    Instantiate<GameObject>(GameManager.Instance.GamePrefabs.Player, new Vector3((float)(num - (this.width / 2)), 0f, (float)(num2 - (this.height / 2))), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer.transform);
                }
                num2++;
            }
        }
    }

    private void GenerateEnemies()
    {
        Debug.Log("Total Enemy Spawns: " + this.enemySpawns.Count);
        int num = 0;
        while (num < Mathf.Min(10, this.enemySpawns.Count))
        {
            if (this.usedEnemySpawns.Count >= this.enemySpawns.Count)
            {
                Debug.Log("Out of spawn points!");
                return;
            }
            int item = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            while (true)
            {
                if (!this.usedEnemySpawns.Contains(item))
                {
                    this.usedEnemySpawns.Add(item);
                    Instantiate<GameObject>(GameManager.Instance.GamePrefabs.BasicEnemy, new Vector3(this.enemySpawns[item].x, 0f, this.enemySpawns[item].y), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer.transform);
                    num++;
                    break;
                }
                item = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            }
        }
    }

    public void GenerateMap()
    {
        if (GameManager.Instance.LevelContainer)
        {
            Destroy(GameManager.Instance.LevelContainer.gameObject);
        }
        GameManager.Instance.LevelContainer = new GameObject("LevelContainer").transform;
        this.map = new int[this.width, this.height];
        this.RandomFillMap();
        for (int i = 0; i < this.smooth; i++)
        {
            this.SmoothMap();
        }
        this.enemySpawns.Clear();
        this.usedEnemySpawns.Clear();
        this.GenerateBlocks();
        this.GenerateEnemies();
    }

    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int num = 0;
        int num2 = gridX - 1;
        while (num2 <= (gridX + 1))
        {
            int num3 = gridY - 1;
            while (true)
            {
                if (num3 > (gridY + 1))
                {
                    num2++;
                    break;
                }
                if (((num2 < 0) || ((num2 >= this.width) || (num3 < 0))) || (num3 >= this.height))
                {
                    num++;
                }
                else if ((num2 != gridX) || (num3 != gridY))
                {
                    int num4 = this.map[num2, num3];
                    if ((num4 > 0) && (num4 <= 2))
                    {
                        num++;
                    }
                }
                num3++;
            }
        }
        return num;
    }

    private void RandomFillMap()
    {
        if (this.useRandomSeed)
        {
            this.seed = (SystemInfo.deviceUniqueIdentifier + Time.realtimeSinceStartup).GetHashCode().ToString();
        }
        UnityEngine.Random.InitState(this.seed.GetHashCode());
        int num2 = 0;
        while (num2 < this.width)
        {
            int num3 = 0;
            while (true)
            {
                if (num3 >= this.height)
                {
                    num2++;
                    break;
                }
                if (((num2 > ((this.width / 2) - 3)) && ((num2 < ((this.width / 2) + 3)) && (num3 > ((this.height / 2) - 3)))) && (num3 < ((this.width / 2) + 3)))
                {
                    if ((num2 != (this.width / 2)) || (num3 != (this.height / 2)))
                    {
                        this.map[num2, num3] = 0;
                    }
                    else
                    {
                        this.map[num2, num3] = 9;
                    }
                }
                else if (((num2 == 0) || ((num2 == (this.width - 1)) || (num3 == 0))) || (num3 == (this.height - 1)))
                {
                    this.map[num2, num3] = 1;
                }
                else
                {
                    int num4 = UnityEngine.Random.Range(0, 100);
                    if (num4 < this.randomFillPercent)
                    {
                        this.map[num2, num3] = 1;
                    }
                    else if (num4 < (this.randomFillPercent + 5))
                    {
                        this.map[num2, num3] = 2;
                    }
                    else if ((num4 > (this.randomFillPercent + 10)) && (num4 <= (this.randomFillPercent + 20)))
                    {
                        this.map[num2, num3] = 3;
                    }
                }
                num3++;
            }
        }
    }

    private void SmoothMap()
    {
        int gridX = 0;
        while (gridX < this.width)
        {
            int gridY = 0;
            while (true)
            {
                if (gridY >= this.height)
                {
                    gridX++;
                    break;
                }
                int surroundingWallCount = this.GetSurroundingWallCount(gridX, gridY);
                if (surroundingWallCount > 4)
                {
                    this.map[gridX, gridY] = 1;
                }
                else if ((surroundingWallCount < 4) && (this.map[gridX, gridY] <= 1))
                {
                    this.map[gridX, gridY] = 0;
                }
                gridY++;
            }
        }
    }

    private void Start()
    {
        if (this.seed.Length > 4)
        {
            this.seed = this.seed.Substring(0, 4);
        }
        this.GenerateMap();
    }
}
