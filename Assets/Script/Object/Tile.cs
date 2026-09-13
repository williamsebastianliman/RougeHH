using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : IComparableItem 
{
    private GameObject tileContent;
    private GameObject thisTile;
    private bool isSpace;
    private bool isWall;
    private int x;
    private int y;
    private bool isCorridor;
    private Enemy enemy;
    private float realDistance;
    private float heuristicDistance;
    private Tile prevTile;
    public bool isPlayer;
    private const float maxFiller = 9999999;
    public Tile(bool isSpace, int x, int y)
    {
        tileContent = null;
        this.isSpace = isSpace;
        this.x = x;
        this.y = y;
        realDistance = maxFiller;
        heuristicDistance = maxFiller;
        isCorridor = false;
        isWall = false;
    }
    public int getX()
    {
        return x;
    }
    public int getY()
    {
        return y;
    }
    public bool isFloor()
    {
        return isSpace;
    }
    public void setX(int x)
    {
        this.x = x;
    }
    public void setY(int y)
    {
        this.y = y;
    }
    public void setSpace(bool isSpace)
    {
        this.isSpace = isSpace;
    }
    public void setCorridor(bool isCorridor)
    {
        this.isCorridor = isCorridor;
    }
    public bool getCorridor()
    {
        return isCorridor;
    }
    public void setTileContent(GameObject tileContent)
    {
        this.tileContent = tileContent;
    }
    public GameObject getTileContent()
    {
        return tileContent;
    }
    public float getRealDistance()
    {
        return realDistance;
    }
    private void setRealDistance(float realDistance)
    {
        this.realDistance = realDistance;
    }
    public float getHeuristicDistance()
    {
        return heuristicDistance;
    }
    private void setHeuristicDistance(float heuristicDistance)
    {
        this.heuristicDistance = heuristicDistance;
    }
    public float getValue()
    {
        return realDistance + heuristicDistance;
    }
    public void setUp(float realDistance, float heuristicDistance)
    {
        setRealDistance(realDistance);
        setHeuristicDistance(heuristicDistance);
    }
    public void setPrevTile(Tile tile)
    {
        prevTile = tile;
    }
    public Tile getPrevTile()
    {
        return prevTile;
    }
    public void reset()
    {
        realDistance = maxFiller;
        heuristicDistance = maxFiller;
        prevTile = null;
    }
    public void setTile(GameObject tile)
    {
        this.thisTile = tile;
    }
    public GameObject getTile()
    {
        return this.thisTile;
    }
    public bool IsWall()
    {
        return isWall;
    }
    public void setWall(bool isWall)
    {
        this.isWall = isWall;
        if (isWall)
        {
            thisTile.GetComponent<TileInfo>().enableCollider();
        }
        else
        {
            thisTile.GetComponent<TileInfo>().disableCollider();
        }
        
        
    }
    public bool IsEnemy()
    {
        return enemy!=null;
    }
    public void setEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public Enemy getEnemy()
    {
        return enemy;
    }

}
