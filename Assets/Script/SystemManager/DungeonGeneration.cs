using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    [SerializeField] private PlayerPositionSO playerPos;
    [SerializeField] private DungeonConfigurationSO config;
    [SerializeField] private MapSO mapSO;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject emptyTile;

    private int roomAmount;
    private int mapRow;
    private int mapCol;
    private int globalRow;
    private int globalCol;

    private List<Room> roomList;
    private List<List<Tile>> grid = new List<List<Tile>>();
    public int getRow()
    {
        return globalRow;
    }
    public int getCol()
    {
        return globalCol;
    }
    public List<List<Tile>> getGrid()
    {
        return grid;
    }
    private void Start()
    {
        if(config.isBossFloor)
        {
            config.enemyCount = Random.Range(1, 2);
            config.setEnemyCount();
            roomList = new List<Room>();
            config.minimumRoom = 1;
            config.maximumRoom = 1;
            getRoomAmount();
            getMapSize();
            roomGeneration();
            genEmptyTile();
            addDecoration();
            setSO();
            initializePlayer();
            generateBoss();
            return;
        }
        config.enemyCount = Random.Range(config.enemyCountMin, config.enemyCountMax + 1);
        config.setEnemyCount();
        roomList = new List<Room>();
        getRoomAmount();
        getMapSize();
        roomGeneration();
        newCorridorGeneration();
        genEmptyTile();
        addDecoration();
        setSO();
        initializePlayer();
        initializeEnemy(config.enemyCount);
    }
    public void generateBoss()
    {
        Room randomRoom = mapSO.getRandomRoom();
        Tile randomTile = randomRoom.getRandomTile();
        GameObject enemyObj = Instantiate(config.getBoss(), new Vector3(randomTile.getX() * config.tileSize, playerPos.getPlayerHeight(), randomTile.getY() * config.tileSize), Quaternion.identity);
        enemyObj.GetComponent<EntityInfo>().setText();
        Enemy newEnemy = new Enemy(enemyObj, randomTile.getX(), randomTile.getY(), mapSO);
        enemyObj.GetComponent<EnemyController>().enemy = newEnemy;
        config.addEnemy(newEnemy);
        randomTile.setTileContent(enemyObj);
        randomTile.setWall(true);
        randomTile.setEnemy(newEnemy);
    }
    public void oneEnemyPos()
    {
        Room randomRoom = mapSO.getRandomRoom();
        Tile randomTile = randomRoom.getRandomTile();
        GameObject enemyObj = Instantiate(config.getEnemyType(), new Vector3(randomTile.getX() * config.tileSize, playerPos.getPlayerHeight(), randomTile.getY() * config.tileSize), Quaternion.identity);
        enemyObj.GetComponent<EntityInfo>().setText();
        Enemy newEnemy = new Enemy(enemyObj, randomTile.getX(), randomTile.getY(), mapSO);
        enemyObj.GetComponent<EnemyController>().enemy = newEnemy;
        config.addEnemy(newEnemy);
        randomTile.setTileContent(enemyObj);
        randomTile.setWall(true);
        randomTile.setEnemy(newEnemy);
    }
    public void initializeEnemy(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            oneEnemyPos();
        }
    }
    private void setupPlayer(int x, int y)
    {
        playerPos.changePos(x, y);
        GameObject instance = Instantiate(playerPrefab, new Vector3(x * config.tileSize, playerPos.getPlayerHeight(), y * config.tileSize), Quaternion.identity);
        playerPos.setPlayer(instance);
        grid[x][y].setTileContent(instance);
        grid[x][y].isPlayer = true;
    }
    private void getRoomAmount()
    {
        roomAmount = Random.Range(config.minimumRoom, config.maximumRoom + 1);
    }
    private void getMapSize()
    {
        mapRow = (int)(config.mapFactor * roomAmount * (int)config.roomMax.y + config.roomGap * config.constant * roomAmount);
        mapCol = (int)(config.mapFactor * roomAmount * (int)config.roomMax.x + config.roomGap * config.constant * roomAmount);
        globalRow = mapRow + (int)(8 * config.roomMax.x);
        globalCol = mapCol + (int)(8 * config.roomMax.y);
        for (int i = 0; i < globalRow; i++)
        {
            List<Tile> tileList = new List<Tile>();
            for (int j = 0; j < globalCol; j++)
            {
                tileList.Add(new Tile(false, i, j));
            }
            grid.Add(tileList);
        }
    }
    private Vector2 roomDimension()
    {
        int roomWidth = Random.Range((int)config.roomMin.x, (int)(config.roomMax.x) + 1);
        int roomHeight = Random.Range((int)config.roomMin.y, (int)(config.roomMax.y) + 1);
        return new Vector2(roomWidth, roomHeight);
    }
    private bool isCollision(int x, int y, int width, int height)
    {
        foreach (Room room in roomList)
        {
            if (x + width + config.roomGap <= room.getX() ||
            x >= room.getX() + room.getWidth() + config.roomGap ||
            y + height + config.roomGap <= room.getY() ||
            y >= room.getY() + room.getHeight() + config.roomGap)
            {
                continue;
            }
            return true;
        }
        return false;
    }
    private void initializePlayer()
    {
        Room room = mapSO.getRandomRoom();
        Tile randomTile = room.getRandomTile();
        setupPlayer(randomTile.getX(), randomTile.getY());

    }
    private void roomGeneration()
    {
        for (int x = 0; x < roomAmount; x++)
        {
            Vector2 dim = roomDimension();
            int seedX = 0;
            int seedY = 0;
            do
            {
                seedX = Random.Range(0, mapCol);
                seedY = Random.Range(0, mapRow);
            }
            while (isCollision(seedX, seedY, (int)dim.x, (int)dim.y));

            Room room = new Room(seedX, seedY, (int)dim.x, (int)dim.y);
            GameObject roomObject = new GameObject("Room" + x);

            roomObject.transform.position = room.getCenter();

            roomList.Add(room);
            for (int i = 0; i < dim.x; i++)
            {
                for (int j = 0; j < dim.y; j++)
                {
                    GameObject tileObj = Instantiate(config.tile, new Vector3((seedX + i) * config.tileSize, 0, (seedY + j) * config.tileSize), Quaternion.identity);

                    grid[seedX + i][seedY + j].setSpace(true);
                    grid[seedX + i][seedY + j].setTile(tileObj);
                    tileObj.transform.SetParent(roomObject.transform);
                    room.addTile(grid[seedX + i][seedY + j]);
                }
            }
        }
        
    }
    private void genEmptyTile()
    {
        for (int i = 0; i < globalRow; i++)
        {
            for (int j = 0; j < globalCol; j++)
            {
                if (!grid[i][j].isFloor())
                {
                    Instantiate(emptyTile, new Vector3(i * config.tileSize, 0, j * config.tileSize), Quaternion.identity);
                }
            }
        }
    }
    private void newCorridorGeneration()
    {
        List<Corridor> corridorList = new List<Corridor>();
        int room1Idx = 0;
        foreach (Room room in roomList)
        {
            int room2Idx = 0;
            foreach (Room room2 in roomList)
            {
                if (room != room2)
                {
                    corridorList.Add(new Corridor(room, room2, room1Idx, room2Idx));
                }
                room2Idx++;
            }
            room1Idx++;
        }
        corridorList.Sort((cor1, cor2) => cor1.distance.CompareTo(cor2.distance));
        DisjointSet set = new DisjointSet(roomList.Count);
        int mstSetCount = roomList.Count - 1;
        int currentCount = 0;
        int currentIdx = 0;
        while (currentCount < mstSetCount)
        {
            Corridor cor = corridorList[currentIdx];
            bool isNotLoop = set.union(cor.idx1, cor.idx2);
            if (isNotLoop)
            {
                Room room = cor.GetRoom1();
                Room targetRoom = cor.GetRoom2();
                buildCorridor(room, targetRoom);
                currentCount++;
            }
            currentIdx++;
        }
    }
    private void buildCorridor(Room room, Room targetRoom)
    {
        if (room.getCenter().x < targetRoom.getCenter().x)
        {
            for (int i = (int)room.getCenter().x; i <= (int)targetRoom.getCenter().x; i++)
            {
                if (!grid[i][(int)room.getCenter().z].isFloor())
                {
                    GameObject tileObj = Instantiate(config.tile, new Vector3(i * config.tileSize, 0, room.getCenter().z * config.tileSize), Quaternion.identity);
                    grid[i][(int)room.getCenter().z].setSpace(true);
                    grid[i][(int)room.getCenter().z].setCorridor(true);
                    grid[i][(int)room.getCenter().z].setTile(tileObj);
                }
            }
        }
        else
        {
            for (int i = (int)targetRoom.getCenter().x; i <= (int)room.getCenter().x; i++)
            {
                if (!grid[i][(int)room.getCenter().z].isFloor())
                {
                    GameObject tileObj = Instantiate(config.tile, new Vector3(i * config.tileSize, 0, room.getCenter().z * config.tileSize), Quaternion.identity);
                    grid[i][(int)room.getCenter().z].setSpace(true);
                    grid[i][(int)room.getCenter().z].setCorridor(true);
                    grid[i][(int)room.getCenter().z].setTile(tileObj);
                }
            }
        }
        //Generate Vertical Corridor
        if (room.getCenter().z < targetRoom.getCenter().z)
        {
            for (int i = (int)room.getCenter().z; i <= (int)targetRoom.getCenter().z; i++)
            {
                if (!grid[(int)targetRoom.getCenter().x][i].isFloor())
                {
                    GameObject tileObj = Instantiate(config.tile, new Vector3(targetRoom.getCenter().x * config.tileSize, 0, i * config.tileSize), Quaternion.identity);
                    grid[(int)targetRoom.getCenter().x][i].setSpace(true);
                    grid[(int)targetRoom.getCenter().x][i].setCorridor(true);
                    grid[(int)targetRoom.getCenter().x][i].setTile(tileObj);
                }
            }
        }
        else
        {
            for (int i = (int)targetRoom.getCenter().z; i <= (int)room.getCenter().z; i++)
            {
                if (!grid[(int)targetRoom.getCenter().x][i].isFloor())
                {
                    GameObject tileObj = Instantiate(config.tile, new Vector3(targetRoom.getCenter().x * config.tileSize, 0, i * config.tileSize), Quaternion.identity);
                    grid[(int)targetRoom.getCenter().x][i].setSpace(true);
                    grid[(int)targetRoom.getCenter().x][i].setCorridor(true);
                    grid[(int)targetRoom.getCenter().x][i].setTile(tileObj);
                }
            }
        }
    }
    public bool noObstacleAround(int x, int y)
    {
        if (x + 1 > globalRow || x - 1 < 0 || y + 1 > globalCol || y - 1 < 0)
        {
            Debug.Log("Edge Obstacle!");
            return false;
        }
        if (grid[x + 1][y].IsWall() || grid[x - 1][y].IsWall() || grid[x][y + 1].IsWall() || grid[x][y-1].IsWall())
        {
            return false;
        }
        if (grid[x + 1][y+1].IsWall() || grid[x - 1][y+1].IsWall() || grid[x+1][y - 1].IsWall() || grid[x-1][y - 1].IsWall())
        {
            return false;
        }
        if (grid[x + 1][y].getCorridor() || grid[x - 1][y].getCorridor() || grid[x][y + 1].getCorridor() || grid[x][y - 1].getCorridor())
        {
            return false;
        }
        return true;
    }
    private void addDecoration()
    {
        int row = grid.Count;
        int col = grid[0].Count;
        for(int i=0;i<row;i++)
        {
            for(int j=0;j<col;j++)
            {
                bool isDecoTile = config.isDecoration();
                if(isDecoTile && grid[i][j].isFloor() && !grid[i][j].getCorridor())
                {
                    if(noObstacleAround(i,j))
                    {
                        GameObject deco = config.getDecoration();
                        DecorationInfo info = deco.GetComponent<DecorationInfo>();
                        GameObject realDeco = Instantiate(deco, new Vector3(i * config.tileSize, info.getItemHeight(), j * config.tileSize), deco.transform.rotation);
                        int randomOrientation = Random.Range(1, 5);
                        Vector3 localEuler = realDeco.transform.localEulerAngles;
                        localEuler.y = randomOrientation * 90;
                        realDeco.transform.localEulerAngles = localEuler;

                        grid[i][j].setTileContent(realDeco);
                        grid[i][j].setWall(!info.getWalkable());
                    }
                }
            }
        }
    }
    private void setSO()
    {
        mapSO.setMap(grid);
        mapSO.setRooms(roomList);
    }
    
}
