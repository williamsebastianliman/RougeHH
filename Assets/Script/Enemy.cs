using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    private int x;
    private int y;
    private float alertValue = 3.8f;
    private GameObject thisObject;
    private MapSO mapSO;
    public Enemy (GameObject thisObject, int x, int y, MapSO mapSO)
    {
        this.thisObject = thisObject;
        this.x = x;
        this.y = y;
        this.mapSO = mapSO;
    }
    public GameObject getObject()
    {
        return thisObject;
    }
    public void setObject(GameObject thisObject)
    {
        this.thisObject = thisObject;
    }
    public int getX()
    {
        return x;
    }
    public int getY()
    {
        return y;
    }
    public void setCoor(int x, int y)
    {
        Tile tile = mapSO.getTile(this.x, this.y);
        tile.setWall(false);
        tile.setEnemy(null);
        this.x = x;
        this.y = y;
        Tile tile1 = mapSO.getTile(this.x, this.y);
        tile1.setWall(true);
        tile1.setEnemy(this);
    }
    public void setDead()
    {
        Tile tile = mapSO.getTile(this.x, this.y);
        tile.setWall(false);
        tile.setEnemy(null);
    }
    public float getAlertValue()
    {
        return alertValue;
    }
}
