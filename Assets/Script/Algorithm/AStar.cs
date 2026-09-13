using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    PriorityQueue<Tile> pq = new PriorityQueue<Tile>();
    List<List<Tile>> mapCopy;
    private int mapRow;
    private int mapCol;

    private MapSO mapSO;

    public AStar(MapSO mapSO)
    {
        this.mapCopy = new List<List<Tile>>(mapSO.getGrid());
        this.mapSO = mapSO;
        this.mapRow = mapSO.getRow();
        this.mapCol = mapSO.getCol();
    }
    
    private float heuristic(int startX, int startY, int endX, int endY)
    {
        return 1f * Mathf.Abs(startX - endX) + Mathf.Abs(startY - endY);
    }
    private void debugFloor()
    {
        Debug.Log("Bound: " + mapRow + " " + mapCol);
        Debug.Log("Dimension: " + mapSO.getGrid().Count + " " + mapSO.getGrid()[0].Count);
    }
    private void debugPq()
    {
        List<Tile> l = pq.getList();
        for(int i=0;i<l.Count;i++)
        {
            Debug.Log("Prio Q"+i+": "+l[i].getX()+" " + l[i].getY()+" h: " + l[i].getValue());
        }
    }
    public List<Tile> algorithm(int startX, int startY, int targetX, int targetY)
    {
        List<Tile> result = new List<Tile>();
        Tile newTile = mapSO.getTile(startX, startY);
        newTile.setUp(0,heuristic(startX, startY, targetX, targetY));
        pq.push(newTile);
        bool isFound = false;
        Tile tempNode = null;
        while(pq.size()!=0)
        {
            Tile popNode = pq.pop();
            if(popNode.getX()==targetX&&popNode.getY()==targetY)
            {
                isFound = true;
                tempNode = popNode;
                break;
            }
            //Down
            if(popNode.getX()+1 < mapRow && popNode.getRealDistance()+1 < mapCopy[popNode.getX() + 1][popNode.getY()].getRealDistance()&& mapCopy[popNode.getX() + 1][popNode.getY()].isFloor() && !mapCopy[popNode.getX() + 1][popNode.getY()].IsWall())
            {
                //Debug.Log("Bawah");
                Tile node = mapSO.getTile(popNode.getX() + 1, popNode.getY());
                node.setUp(popNode.getRealDistance()+1, heuristic(popNode.getX() + 1, popNode.getY(), targetX, targetY));
                node.setPrevTile(popNode);
                pq.push(node);
                
            }
            //Up
            if(popNode.getX() - 1 >= 0 && popNode.getRealDistance() + 1 < mapCopy[popNode.getX() - 1][popNode.getY()].getRealDistance()&& mapCopy[popNode.getX() - 1][popNode.getY()].isFloor() && !mapCopy[popNode.getX() - 1][popNode.getY()].IsWall())
            {
                //Debug.Log("Atas");
                Tile node = mapSO.getTile(popNode.getX() - 1, popNode.getY());
                node.setUp(popNode.getRealDistance() + 1, heuristic(popNode.getX() - 1, popNode.getY(), targetX, targetY));
                node.setPrevTile(popNode);
                pq.push(node);
            }
            //Left
            if(popNode.getY() - 1 >= 0 && popNode.getRealDistance() + 1 < mapCopy[popNode.getX()][popNode.getY() -1].getRealDistance()&& mapCopy[popNode.getX()][popNode.getY() - 1].isFloor() && !mapCopy[popNode.getX()][popNode.getY() - 1].IsWall())
            {
                //Debug.Log("Kiri");
                Tile node = mapSO.getTile(popNode.getX(), popNode.getY() -1);
                node.setUp(popNode.getRealDistance() + 1, heuristic(popNode.getX(), popNode.getY() -1, targetX, targetY));
                node.setPrevTile(popNode);
                pq.push(node);
            }
            //Right
            if(popNode.getY() + 1 < mapCol && popNode.getRealDistance() + 1 < mapCopy[popNode.getX()][popNode.getY() + 1].getRealDistance()&& mapCopy[popNode.getX()][popNode.getY() + 1].isFloor() && !mapCopy[popNode.getX()][popNode.getY() + 1].IsWall())
            {
                //Debug.Log("Kanan");
                Tile node = mapSO.getTile(popNode.getX(), popNode.getY() + 1);
                node.setUp(popNode.getRealDistance() + 1, heuristic(popNode.getX(), popNode.getY() + 1, targetX, targetY));
                node.setPrevTile(popNode);
                pq.push(node);
            }
        }
        if(isFound)
        {
            while (tempNode != null)
            {
                result.Add(tempNode);
                tempNode = tempNode.getPrevTile();
            }
        }
        clear();
        return result;
    }
    public bool isATileWall(int x, int y)
    {
        return mapCopy[x][y].IsWall();
    }
    public void clear()
    {
            int row = mapSO.getRow();
            int col = mapSO.getCol();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mapCopy[i][j].reset();
                }
            }
        
    }
}
