using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField] public int currentCost;
    [SerializeField] public int currentLevel;
    [SerializeField] public int maxLevel = 45;
    [SerializeField] public string description;
    [SerializeField] public string unit;
    [SerializeField] public float upgradeStat;
    [SerializeField] public string currentStat;
    [SerializeField] public Sprite sprite;
}
