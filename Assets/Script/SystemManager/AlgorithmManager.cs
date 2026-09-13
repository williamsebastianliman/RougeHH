using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmManager : MonoBehaviour
{
    [SerializeField] private MapSO mapSO;

    public AStar aStar;
    void Start()
    {
        aStar = new AStar(mapSO);
    }
}
