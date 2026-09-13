using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private int x;
    private int y;
    private int width;
    private int height;
    private List<Tile> tileList = new List<Tile>();
    public Room(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
    public Vector3 getCenter()
    {
        int centerX = x + width / 2;
        int centerY = y + height / 2;
        return new Vector3(centerX, 0, centerY);
    }
    public int getWidth()
    {
        return width;
    }
    public int getHeight()
    {
        return height;
    }
    public int getX()
    {
        return x;
    }
    public int getY()
    {
        return y;
    }
    public int getTileAmount()
    {
        return tileList.Count;
    }
    public void addTile(Tile tile)
    {
        tileList.Add(tile);
    }
    public Tile getRandomTile()
    {
        int idx = 0;
        int tileCount = getTileAmount();
        do
        {
            idx = Random.Range(0, tileCount);
        }
        while (tileList[idx].IsWall() || tileList[idx].isPlayer);
        return tileList[idx];
    }

}
