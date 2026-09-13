using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue <T> where T:IComparableItem
{
    private List<T> pq = new List<T>();
    public void push(T item)
    {
        pq.Add(item);
    }
    public T pop()
    {
        float min = int.MaxValue;
        T savedItem = default;
        foreach(T item in pq)
        {
            if(item.getValue() < min)
            {
                min = item.getValue();
                savedItem = item;
            }
        }
        pq.Remove(savedItem);
        return savedItem;
    }
    public int size()
    {
        return pq.Count;
    }
    public List<T> getList()
    {
        return pq;
    }
}
