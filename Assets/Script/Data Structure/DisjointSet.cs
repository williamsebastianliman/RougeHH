using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisjointSet
{
    private List<int> parent;
    public DisjointSet(int size)
    {
        parent = new List<int>();
        for(int i=0;i<size;i++)
        {
            parent.Add(i);
        }
    }
    public int findParent(int val)
    {
        if (parent[val] != val)
        {
            parent[val] = findParent(parent[val]);
        }
        return parent[val];
    }
    public bool union(int a, int b)
    {
        int parent1 = findParent(a);
        int parent2 = findParent(b);
        if(parent1!=parent2)
        {
            parent[parent2] = parent1;
            return true;
        }
        return false;
    }

}
