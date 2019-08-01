using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class NewMapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public int smooth = 2;
    public int BasicEnemySpawns = 8;
    public int ArmorEnemySpawns = 4;
    public int PillarEnemySpawns = 3;
    public int ArmorPillarEnemySpawns = 2;

    public int AbsorberEnemySpawns = 2;
    public int HunterEnemySpawns = 6;

    public int switchSpawns = 1;

    public string Seed;
    public bool useRandomSeed;
    [Range(0f, 100f)]
    public int randomFillPercent;
    private int[,] map;
    public Wave[] Waves = new Wave[0];
    private List<Vector2> enemySpawns = new List<Vector2>();
    private List<Vector2> playerSpawns = new List<Vector2>();
    private List<int> usedEnemySpawns = new List<int>();

    private Wave lastWave;

    private void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
    {
        List<Room> list = new List<Room>();
        List<Room> list2 = new List<Room>();
        if (!forceAccessibilityFromMainRoom)
        {
            list = allRooms;
            list2 = allRooms;
        }
        else
        {
            foreach (Room room in allRooms)
            {
                if (room.isAccessibleFromMainRoom)
                {
                    list2.Add(room);
                    continue;
                }
                list.Add(room);
            }
        }
        int num = 0;
        Coord tileA = new Coord();
        Coord tileB = new Coord();
        Room roomA = new Room();
        Room roomB = new Room();
        bool flag = false;
        foreach (Room room4 in list)
        {
            if (!forceAccessibilityFromMainRoom)
            {
                flag = false;
                if (room4.connectedRooms.Count > 0)
                {
                    continue;
                }
            }
            foreach (Room room5 in list2)
            {
                if (ReferenceEquals(room4, room5))
                {
                    continue;
                }
                if (!room4.IsConnected(room5))
                {
                    int num2 = 0;
                    while (num2 < room4.edgeTiles.Count)
                    {
                        int num3 = 0;
                        while (true)
                        {
                            if (num3 >= room5.edgeTiles.Count)
                            {
                                num2++;
                                break;
                            }
                            Coord coord3 = room4.edgeTiles[num2];
                            Coord coord4 = room5.edgeTiles[num3];
                            int num4 = (int)(Mathf.Pow((float)(coord3.tileX - coord4.tileX), 2f) + Mathf.Pow((float)(coord3.tileY - coord4.tileY), 2f));
                            if ((num4 < num) || !flag)
                            {
                                num = num4;
                                flag = true;
                                tileA = coord3;
                                tileB = coord4;
                                roomA = room4;
                                roomB = room5;
                            }
                            num3++;
                        }
                    }
                }
            }
            if (flag && !forceAccessibilityFromMainRoom)
            {
                this.CreatePassage(roomA, roomB, tileA, tileB);
            }
        }
        if (flag && forceAccessibilityFromMainRoom)
        {
            this.CreatePassage(roomA, roomB, tileA, tileB);
            this.ConnectClosestRooms(allRooms, true);
        }
        if (!forceAccessibilityFromMainRoom)
        {
            this.ConnectClosestRooms(allRooms, true);
        }
    }

    private Vector3 CoordToWorldPoint(Coord tile) =>
        new Vector3(((-this.width / 2) + 0.5f) + tile.tileX, 2f, ((-this.height / 2) + 0.5f) + tile.tileY);

    private void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        Room.ConnectRooms(roomA, roomB);
        foreach (Coord coord in this.GetLine(tileA, tileB))
        {
            this.DrawCircle(coord, 5);
        }
    }

    private void DrawCircle(Coord c, int r)
    {
        int num = -r;
        while (num <= r)
        {
            int num2 = -r;
            while (true)
            {
                if (num2 > r)
                {
                    num++;
                    break;
                }
                if (((num * num) + (num2 * num2)) <= (r * r))
                {
                    int x = c.tileX + num;
                    int y = c.tileY + num2;
                    if (this.IsInMapRange(x, y))
                    {
                        this.map[x, y] = 0;
                    }
                }
                num2++;
            }
        }
    }

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
                if (this.map[num, num2] != 0)
                {
                    if (this.map[num, num2] == 1)
                    {
                        Instantiate<GameObject>(GameManager.Instance.GamePrefabs.RegularWall, new Vector3((float)(num - (this.width / 2)), 0f, (float)(num2 - (this.height / 2))), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer);
                    }
                }
                else
                {
                    int num3 = UnityEngine.Random.Range(0, 100);
                    if (num3 > 90f)
                    {
                        Instantiate<GameObject>(GameManager.Instance.GamePrefabs.DestructableWall, new Vector3((float)(num - (this.width / 2)), 0f, (float)(num2 - (this.height / 2))), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer);
                    }
                    else if (num3 > 80)
                    {
                        this.enemySpawns.Add(new Vector2((float)(num - (this.width / 2)), (float)(num2 - (this.height / 2))));
                    }
                    else if (num3 > 40)
                    {
                        this.playerSpawns.Add(new Vector2((float)(num - (this.width / 2)), (float)(num2 - (this.height / 2))));
                    }
                }
                num2++;
            }
        }
    }

    private void GenerateEnemies()
    {
        int num = UnityEngine.Random.Range(0, this.playerSpawns.Count);
        Instantiate<GameObject>(GameManager.Instance.GamePrefabs.Player, new Vector3(this.playerSpawns[num].x, 0f, this.playerSpawns[num].y), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer);
        int item = UnityEngine.Random.Range(0, this.enemySpawns.Count);
        this.usedEnemySpawns.Add(item);
        //generate BOSS
        int level = GameManager.Instance.CurrentLevel;
        if (level == 2 || level == 4 || level == 6 || level == 7)
        {
            GameObject boss = Instantiate<GameObject>(GameManager.Instance.GamePrefabs.BossEnemy, new Vector3(this.enemySpawns[item].x, 0f, this.enemySpawns[item].y), Quaternion.identity);
            boss.transform.SetParent(GameManager.Instance.LevelContainer);
            GameManager.Instance.SetLevelBoss(boss);

            GameManager.Instance.bossExist = true;
            GameManager.Instance.bossIsAlive = true;

            if(level == 2)
            {
                boss.SetActive(false);
            }

        }        
        else
        {
            GameManager.Instance.bossExist = false;
            GameManager.Instance.bossIsAlive = false;
        }
        
        int index = GameManager.Instance.CurrentLevel - 1;
        /*
        Wave thisWave = new Wave();
        if(index < this.Waves.Length)
        {
            thisWave = this.Waves[index];
        }
        else
        {
            thisWave = new Wave(lastWave);
        }*/
        GenerateEnemiewByWave(this.Waves[index]);
        //lastWave = thisWave;
    }

    private void GenerateEnemiewByWave(Wave wave)
    {
        int basicEnemyCnt = 0;
        while (basicEnemyCnt < Mathf.Min(wave.BasicEnemy, this.enemySpawns.Count - this.usedEnemySpawns.Count))
        {
            if (this.usedEnemySpawns.Count >= this.enemySpawns.Count)
            {
                return;
            }
            int rnd = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            while (true)
            {
                if (!this.usedEnemySpawns.Contains(rnd))
                {
                    this.usedEnemySpawns.Add(rnd);
                    GameObject obj = Instantiate(GameManager.Instance.GamePrefabs.BasicEnemy, new Vector3(this.enemySpawns[rnd].x, 0f, this.enemySpawns[rnd].y), Quaternion.identity);
                    obj.transform.SetParent(GameManager.Instance.LevelContainer);

                    PositionShower pos =  PositionShowerManager.Instance.CreatePositionShower();
                    pos.target = obj;


                    basicEnemyCnt++;
                    break;
                }
                rnd = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            }
        }
        int armorEnemyCnt = 0;
        while (armorEnemyCnt < Mathf.Min(wave.ArmorEnemy, this.enemySpawns.Count - this.usedEnemySpawns.Count))
        {
            if (this.usedEnemySpawns.Count >= this.enemySpawns.Count)
            {
                return;
            }
            int rnd = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            while (true)
            {
                if (!this.usedEnemySpawns.Contains(rnd))
                {
                    this.usedEnemySpawns.Add(rnd);
                    Instantiate<GameObject>(GameManager.Instance.GamePrefabs.ArmorEnemy, new Vector3(this.enemySpawns[rnd].x, 0f, this.enemySpawns[rnd].y), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer);
                    armorEnemyCnt++;
                    break;
                }
                rnd = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            }
        }
        int pillarEnemyCnt = 0;
        while (pillarEnemyCnt < Mathf.Min(wave.PillarEnemy, this.enemySpawns.Count - this.usedEnemySpawns.Count))
        {
            if (this.usedEnemySpawns.Count >= this.enemySpawns.Count)
            {
                return;
            }
            int rnd = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            while (true)
            {
                if (!this.usedEnemySpawns.Contains(rnd))
                {
                    this.usedEnemySpawns.Add(rnd);
                    Instantiate<GameObject>(GameManager.Instance.GamePrefabs.PillarEnemy, new Vector3(this.enemySpawns[rnd].x, 0f, this.enemySpawns[rnd].y), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer);
                    pillarEnemyCnt++;
                    break;
                }
                rnd = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            }
        }
        int armorPillarEnemyCnt = 0;
        while (armorPillarEnemyCnt < Mathf.Min(wave.ArmorPillarEnemy, this.enemySpawns.Count - this.usedEnemySpawns.Count))
        {
            if (this.usedEnemySpawns.Count >= this.enemySpawns.Count)
            {
                return;
            }
            int cnt = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            while (true)
            {
                if (!this.usedEnemySpawns.Contains(cnt))
                {
                    this.usedEnemySpawns.Add(cnt);
                    Instantiate<GameObject>(GameManager.Instance.GamePrefabs.PillarArmorEnemy, new Vector3(this.enemySpawns[cnt].x, 0f, this.enemySpawns[cnt].y), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer);
                    armorPillarEnemyCnt++;
                    break;
                }
                cnt = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            }
        }

        int absorberEnemyCnt = 0;
        while (absorberEnemyCnt < Mathf.Min(wave.AbsorberEnemy, this.enemySpawns.Count - this.usedEnemySpawns.Count))
        {
            if (this.usedEnemySpawns.Count >= this.enemySpawns.Count)
            {
                return;
            }
            int cnt = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            while (true)
            {
                if (!this.usedEnemySpawns.Contains(cnt))
                {
                    this.usedEnemySpawns.Add(cnt);
                    Instantiate<GameObject>(GameManager.Instance.GamePrefabs.AbsorberEnemy, new Vector3(this.enemySpawns[cnt].x, 0f, this.enemySpawns[cnt].y), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer);
                    absorberEnemyCnt++;
                    break;
                }
                cnt = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            }
        }

        int switchCnt = 0;
        while (switchCnt < Mathf.Min(wave.Switch, this.enemySpawns.Count - this.usedEnemySpawns.Count))
        {
            if (this.usedEnemySpawns.Count >= this.enemySpawns.Count)
            {
                return;
            }
            int rnd = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            while (true)
            {
                if (!this.usedEnemySpawns.Contains(rnd))
                {
                    this.usedEnemySpawns.Add(rnd);
                    Instantiate<GameObject>(GameManager.Instance.GamePrefabs.Switch, new Vector3(this.enemySpawns[rnd].x, 0f, this.enemySpawns[rnd].y), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer);
                    switchCnt++;
                    break;
                }
                rnd = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            }
        }
        
        int hunterCnt = 0;
        while (hunterCnt < Mathf.Min(wave.Hunter, this.enemySpawns.Count - this.usedEnemySpawns.Count))
        {
            if (this.usedEnemySpawns.Count >= this.enemySpawns.Count)
            {
                return;
            }
            int rnd = UnityEngine.Random.Range(0, this.enemySpawns.Count);
            while (true)
            {
                if (!this.usedEnemySpawns.Contains(rnd))
                {
                    this.usedEnemySpawns.Add(rnd);
                    Instantiate<GameObject>(GameManager.Instance.GamePrefabs.Hunter, new Vector3(this.enemySpawns[rnd].x, 0f, this.enemySpawns[rnd].y), Quaternion.identity).transform.SetParent(GameManager.Instance.LevelContainer);
                    hunterCnt++;
                    break;
                }
                rnd = UnityEngine.Random.Range(0, this.enemySpawns.Count);
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
        this.ProcessMap();
        Debug.Log("Seed: " + this.Seed);
        Debug.Log("Level: " + GameManager.Instance.CurrentLevel);
        this.enemySpawns.Clear();
        this.playerSpawns.Clear();
        this.usedEnemySpawns.Clear();
        this.GenerateBlocks();
        this.GenerateEnemies();
        GameManager.Instance.SetGameStats();
    }

    private List<Coord> GetLine(Coord from, Coord to)
    {
        int num = 0;
        List<Coord> list = new List<Coord>();
        int tileX = from.tileX;
        int tileY = from.tileY;
        int w = to.tileX - from.tileX;
        int h = to.tileY - from.tileY;
        bool flag = false;
        int signW = Math.Sign(w);
        int signH = Math.Sign(h);
        int absW = Mathf.Abs(w);
        if (absW < Mathf.Abs(h))
        {
            flag = true;
            absW = Mathf.Abs(h);
            num = Mathf.Abs(w);
            signW = Math.Sign(h);
            signH = Math.Sign(w);
        }
        int sum = absW / 2;
        for (int i = 0; i < absW; i++)
        {
            list.Add(new Coord(tileX, tileY));
            if (flag)
            {
                tileY += signW;
            }
            else
            {
                tileX += signW;
            }
            sum += num;
            if (sum >= absW)
            {
                if (flag)
                {
                    tileX += signH;
                }
                else
                {
                    tileY += signH;
                }
                sum -= absW;
            }
        }
        return list;
    }

    private List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> list = new List<List<Coord>>();
        int[,] numArray = new int[this.width, this.height];
        int startX = 0;
        while (startX < this.width)
        {
            int startY = 0;
            while (true)
            {
                if (startY >= this.height)
                {
                    startX++;
                    break;
                }
                if ((numArray[startX, startY] == 0) && (this.map[startX, startY] == tileType))
                {
                    List<Coord> regionTiles = this.GetRegionTiles(startX, startY);
                    list.Add(regionTiles);
                    foreach (Coord coord in regionTiles)
                    {
                        numArray[coord.tileX, coord.tileY] = 1;
                    }
                }
                startY++;
            }
        }
        return list;
    }

    private List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> list = new List<Coord>();
        int[,] numArray = new int[this.width, this.height];
        int num = this.map[startX, startY];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        numArray[startX, startY] = 1;
        while (queue.Count > 0)
        {
            Coord item = queue.Dequeue();
            list.Add(item);
            int x = item.tileX - 1;
            while (x <= (item.tileX + 1))
            {
                int y = item.tileY - 1;
                while (true)
                {
                    if (y > (item.tileY + 1))
                    {
                        x++;
                        break;
                    }
                    if ((this.IsInMapRange(x, y) && (((y == item.tileY) || (x == item.tileX)) && (numArray[x, y] == 0))) && (this.map[x, y] == num))
                    {
                        numArray[x, y] = 1;
                        queue.Enqueue(new Coord(x, y));
                    }
                    y++;
                }
            }
        }
        return list;
    }

    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int num = 0;
        int x = gridX - 1;
        while (x <= (gridX + 1))
        {
            int y = gridY - 1;
            while (true)
            {
                if (y > (gridY + 1))
                {
                    x++;
                    break;
                }
                if (!this.IsInMapRange(x, y))
                {
                    num++;
                }
                else if ((x != gridX) || (y != gridY))
                {
                    num += this.map[x, y];
                }
                y++;
            }
        }
        return num;
    }

    private bool IsInMapRange(int x, int y)
    {
        bool num1;
        if (((x < 0) || (x >= this.width)) || (y < 0))
        {
            num1 = false;
        }
        else
        {
            num1 = y < this.height;
        }
        return num1;
    }

    private void ProcessMap()
    {
        int num = 50;
        foreach (List<Coord> list2 in this.GetRegions(1))
        {
            if (list2.Count < num)
            {
                foreach (Coord coord in list2)
                {
                    this.map[coord.tileX, coord.tileY] = 0;
                }
            }
        }
        int num2 = 50;
        List<Room> allRooms = new List<Room>();
        foreach (List<Coord> list5 in this.GetRegions(0))
        {
            if (list5.Count >= num2)
            {
                allRooms.Add(new Room(list5, this.map));
                continue;
            }
            foreach (Coord coord2 in list5)
            {
                this.map[coord2.tileX, coord2.tileY] = 1;
            }
        }
        if (allRooms.Count <= 0)
        {
            this.GenerateMap();
        }
        else
        {
            allRooms.Sort();
            allRooms[0].isMainRoom = true;
            allRooms[0].isAccessibleFromMainRoom = true;
            this.ConnectClosestRooms(allRooms, false);
        }
    }

    private void RandomFillMap()
    {
        if (this.useRandomSeed)
        {
            this.Seed = (SystemInfo.deviceUniqueIdentifier + Time.realtimeSinceStartup).GetHashCode().ToString();
            this.Seed = this.Seed.Replace("-", string.Empty);
            this.Seed = this.Seed.Substring(0, 4);
        }
        System.Random random = new System.Random(this.Seed.GetHashCode());
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
                if (((num2 == 0) || ((num2 == (this.width - 1)) || (num3 == 0))) || (num3 == (this.height - 1)))
                {
                    this.map[num2, num3] = 1;
                }
                else
                {
                    this.map[num2, num3] = (random.Next(0, 100) >= this.randomFillPercent) ? 0 : 1;
                }
                num3++;
            }
        }
    }

    private void RandomFillMap2()
    {
        if (this.useRandomSeed)
        {
            this.Seed = (SystemInfo.deviceUniqueIdentifier + Time.realtimeSinceStartup).GetHashCode().ToString();
            this.Seed = this.Seed.Substring(0, 4);
        }
        UnityEngine.Random.InitState(this.Seed.GetHashCode());
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
                if (((num2 > ((this.width / 2) - 5)) && ((num2 < ((this.width / 2) + 5)) && (num3 > ((this.height / 2) - 5)))) && (num3 < ((this.width / 2) + 5)))
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
                else if (surroundingWallCount < 4)
                {
                    this.map[gridX, gridY] = 0;
                }
                gridY++;
            }
        }
    }

    private void SmoothMap2()
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

    [StructLayout(LayoutKind.Sequential)]
    private struct Coord
    {
        public int tileX;
        public int tileY;
        public Coord(int x, int y)
        {
            this.tileX = x;
            this.tileY = y;
        }
    }

    private class Room : IComparable<NewMapGenerator.Room>
    {
        public List<NewMapGenerator.Coord> tiles;
        public List<NewMapGenerator.Coord> edgeTiles;
        public List<NewMapGenerator.Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        public Room()
        {
        }

        public Room(List<NewMapGenerator.Coord> roomTiles, int[,] map)
        {
            this.tiles = roomTiles;
            this.roomSize = this.tiles.Count;
            this.connectedRooms = new List<NewMapGenerator.Room>();
            this.edgeTiles = new List<NewMapGenerator.Coord>();
            foreach (NewMapGenerator.Coord coord in this.tiles)
            {
                int num = coord.tileX - 1;
                while (num <= (coord.tileX + 1))
                {
                    int num2 = coord.tileY - 1;
                    while (true)
                    {
                        if (num2 > (coord.tileY + 1))
                        {
                            num++;
                            break;
                        }
                        if (((num == coord.tileX) || (num2 == coord.tileY)) && (map[num, num2] == 1))
                        {
                            this.edgeTiles.Add(coord);
                        }
                        num2++;
                    }
                }
            }
        }

        public int CompareTo(NewMapGenerator.Room otherRoom) =>
            otherRoom.roomSize.CompareTo(this.roomSize);

        public static void ConnectRooms(NewMapGenerator.Room roomA, NewMapGenerator.Room roomB)
        {
            if (roomA.isAccessibleFromMainRoom)
            {
                roomB.SetAccessibleFromMainRoom();
            }
            else if (roomB.isAccessibleFromMainRoom)
            {
                roomA.SetAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool IsConnected(NewMapGenerator.Room otherRoom) =>
            this.connectedRooms.Contains(otherRoom);

        public void SetAccessibleFromMainRoom()
        {
            if (!this.isAccessibleFromMainRoom)
            {
                this.isAccessibleFromMainRoom = true;
                foreach (NewMapGenerator.Room room in this.connectedRooms)
                {
                    room.SetAccessibleFromMainRoom();
                }
            }
        }
    }

    [Serializable]
    public class Wave
    {
        public int BasicEnemy;
        public int ArmorEnemy;
        public int PillarEnemy;
        public int ArmorPillarEnemy;
        public int AbsorberEnemy;
        public int Hunter;

        public int Switch;


        private int zeroOrOne(float rate)
        {
            return (int)(new System.Random().NextDouble() + rate);
        }

        public Wave()
        {

        }

        public Wave(Wave lastWave)
        {
            BasicEnemy = lastWave.BasicEnemy+ zeroOrOne(0.8f);
            ArmorEnemy = lastWave.ArmorEnemy + zeroOrOne(0.8f);
            PillarEnemy = lastWave.PillarEnemy + zeroOrOne(0.7f);
            ArmorPillarEnemy = lastWave.ArmorPillarEnemy + zeroOrOne(0.7f);
            AbsorberEnemy = lastWave.AbsorberEnemy + zeroOrOne(0.5f);
            Hunter = lastWave.Hunter + zeroOrOne(0.8f);
            Switch = lastWave.Switch + zeroOrOne(0.3f);
        }
        
    }
}
