using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridor
{
    private Room room1;
    private Room room2;
    public int idx1;
    public int idx2;
    public int distance;
    public Corridor(Room room1, Room room2, int idx1, int idx2) 
    {
        this.room1 = room1;
        this.room2 = room2;
        this.idx1 = idx1;
        this.idx2 = idx2;
        this.distance = interRoomDistance();
    }
    public Room GetRoom1()
    {
        return room1;
    }
    public Room GetRoom2()
    {
        return room2;
    }
    public int interRoomDistance()
    {
        Vector3 roomInitial = room1.getCenter();
        Vector3 roomDestination = room2.getCenter();
        return (int)(Mathf.Abs(roomInitial.x - roomDestination.x) + Mathf.Abs(roomInitial.z - roomDestination.z));
    }
}
