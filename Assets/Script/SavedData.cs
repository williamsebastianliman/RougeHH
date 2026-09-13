using System;
using System.Collections.Generic;

[Serializable]
public class SavedData
{
    public int currentLevel;
    public float currentExp;
    public List<int> levelList;
    public List<int> costList;
    public int maxFloor;
    public int currentZhen;
    public bool isUnlocked;

    public SavedData(int currentLevel, float currentExp, List<int> levelList, List<int> costList,int maxFloor, int currentZhen, bool isUnlocked)
    {
        this.currentLevel = currentLevel;
        this.currentExp = currentExp;
        this.levelList = levelList;
        this.costList = costList;
        this.maxFloor = maxFloor;
        this.currentZhen = currentZhen;
        this.isUnlocked = isUnlocked;
    }
}
