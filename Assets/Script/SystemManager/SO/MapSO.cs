using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSO", menuName = "System/Map")]
public class MapSO : ScriptableObject
{
    private List<List<Tile>> grid = new List<List<Tile>>();
    private List<Room> rooms = new List<Room>();
    private List<Room> roomsCopy = new List<Room>();
    public List<List<Tile>> getGrid()
    {
        return grid;
    }
    public Tile getTile(int x, int y)
    {
        return grid[x][y];
    }
    public void setMap(List<List<Tile>> map)
    {
        grid = map;
    }
    public int getRow()
    {
        return grid.Count;
    }
    public int getCol()
    {
        if(grid.Count>0)
        {
            return grid[0].Count;
        }
        return 0;
    }
    public void resetMap()
    {
        int row = getRow();
        int col = getCol();
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                grid[i][j].reset();
            }
        }
    }
    public void setRooms(List<Room> rooms)
    {
        this.rooms = rooms;
        this.roomsCopy = new List<Room>(rooms);
    }
    public Room getRandomRoom()
    {
        int roomCount = roomsCopy.Count;
        int idx = Random.Range(0,roomCount);
        Debug.Log("Room Count: " + roomCount);
        Debug.Log("Index: "+idx);
        Room temp = roomsCopy[idx];
        roomsCopy.Remove(temp);
        if(roomCount<=1)
        {
            resetRoom();
        }
        return temp;
    }
    private void resetRoom()
    {
        roomsCopy = new List<Room>(rooms);
    }

}
