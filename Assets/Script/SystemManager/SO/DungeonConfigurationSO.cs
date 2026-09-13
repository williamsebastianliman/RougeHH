using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonConfiguration", menuName = "System/DungeonConfig")]
public class DungeonConfigurationSO : ScriptableObject
{
    public int maxFloor;
    public int defenseScaleFactor = 90;
    public int currentFloor;
    public int enemyCountMax;
    public int enemyCountMin;
    public int enemyCount;
    public float tileSize = 1f;
    public Vector2 roomMin;
    public Vector2 roomMax;
    public int minimumRoom;
    public int maximumRoom;
    public int roomGap = 1;
    public float mapFactor = 1.1f;
    public int constant = 1;
    public int decorationChance = 25;
    public int tileChance = 60;
    public List<Enemy> enemies = new List<Enemy>();
    public GameObject[] decorationPrefab;
    public GameObject tile;
    public GameObject enemy;
    public GameObject enemy1;
    public GameObject enemy2;

    public GameObject boss;
    public int commonChance;
    public int mediumChance;
    public int eliteChance;
    public bool blockAllInput = false;
    public int unlockedFloor;

    [SerializeField] VoidEventChannel EnemyClearedEvent;
    [SerializeField] IntegerEventChannel UpdateEnemy;

    public bool bossFloorUnlocked = false;

    public bool isBossFloor;
    public void setEnemyCount()
    {
        UpdateEnemy.RaiseEvent(enemyCount);
    }
    public bool isDecoration()
    {
        int chance = Random.Range(0, 100);
        if(chance < decorationChance)
        {
            return true;
        }
        return false;
    }
    public GameObject getDecoration()
    {
        int chance = Random.Range(0,100);
        if(chance < tileChance)
        {
            return decorationPrefab[0];
        }
        int chance2 = Random.Range(0, 100 - tileChance);
        int idx = (chance2 % 4)+1;
        return decorationPrefab[idx];
    }
    public void addEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    public void gameOverTest()
    {
        blockAllInput = true;
        EnemyClearedEvent.RaiseEvent();
    }
    public void removeEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        UpdateEnemy.RaiseEvent(enemies.Count);
        if (enemies.Count <=0)
        {
            blockAllInput = true;
            EnemyClearedEvent.RaiseEvent();
        }
    }
    public GameObject getBoss()
    {
        boss.GetComponent<EnemyController>().attack = 1000;
        boss.GetComponent<EntityInfo>().maxHealth = 100000;
        boss.GetComponent<EntityInfo>().defense = 100;
        boss.GetComponent<EntityInfo>().critMultiplier = 4;
        boss.GetComponent<EntityInfo>().critChance = 60;
        boss.GetComponent<EntityInfo>().zhenDrop = 99999;
        boss.GetComponent<EntityInfo>().expDrop = 999999;
        return boss;
    }
    public GameObject getEnemyType()
    {
        int num = Random.Range(0, 1000);
        if(num < commonChance)
        {
            enemy.GetComponent<EnemyController>().attack = (int)(0.4f * currentFloor*currentFloor) + 3+2*currentFloor;
            enemy.GetComponent<EntityInfo>().maxHealth = (int)(0.4f * currentFloor * currentFloor) + 10 + 5 * currentFloor;
            enemy.GetComponent<EntityInfo>().defense = (int)(0.1f * currentFloor * currentFloor) + 3 + 1 * currentFloor;
            enemy.GetComponent<EntityInfo>().critMultiplier = (0.0001f * currentFloor * currentFloor) + 1.1f + 0.01f * currentFloor;
            enemy.GetComponent<EntityInfo>().critChance = 5 + (int)currentFloor / 5;
            enemy.GetComponent<EntityInfo>().zhenDrop = 3 + currentFloor;
            enemy.GetComponent<EntityInfo>().expDrop = 3 + currentFloor;
            return enemy;
        }
        else if(num < commonChance + mediumChance)
        {
            enemy1.GetComponent<EnemyController>().attack = (int)(0.6f * currentFloor * currentFloor) + 3 + 3 * currentFloor;
            enemy1.GetComponent<EntityInfo>().maxHealth = (int)(0.6f * currentFloor * currentFloor) + 20 + 6 * currentFloor;
            enemy1.GetComponent<EntityInfo>().defense = (int)(0.2f * currentFloor * currentFloor) + 6 + 2 * currentFloor;
            enemy1.GetComponent<EntityInfo>().critChance = 10 + (int)currentFloor / 5;
            enemy1.GetComponent<EntityInfo>().critMultiplier = (0.0002f * currentFloor * currentFloor) + 1.2f + 0.015f * currentFloor;
            enemy1.GetComponent<EntityInfo>().zhenDrop = 6 + 2 * currentFloor;
            enemy1.GetComponent<EntityInfo>().expDrop = 6 + 2 * currentFloor;
            return enemy1;
        }
        enemy2.GetComponent<EnemyController>().attack = (int)(0.8f * currentFloor * currentFloor) + 3 + 4 * currentFloor;
        enemy2.GetComponent<EntityInfo>().maxHealth = (int)(0.8f * currentFloor * currentFloor) + 30 + 7 * currentFloor;
        enemy2.GetComponent<EntityInfo>().defense = (int)(0.3f * currentFloor * currentFloor) + 9 + 3 * currentFloor;
        enemy2.GetComponent<EntityInfo>().critChance = 15 + (int)currentFloor / 5;
        enemy2.GetComponent<EntityInfo>().critMultiplier = (0.0003f * currentFloor * currentFloor) + 1.3f + 0.02f * currentFloor;
        enemy2.GetComponent<EntityInfo>().zhenDrop = 9 + 3 * currentFloor;
        enemy2.GetComponent<EntityInfo>().expDrop = 9 + 3 * currentFloor;
        return enemy2;
    }
    public void resetSO()
    {
        enemies.Clear();
    }
}
