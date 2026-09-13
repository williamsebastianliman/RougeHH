using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FloorManager : MonoBehaviour
{
    [SerializeField] private int floor = 0;
    [SerializeField] private int maxFloor = 100;
    [SerializeField] private float enemyGrowth;
    [SerializeField] private int baseEnemy;
    [SerializeField] private float eliteFactor = 0.2f;
    [SerializeField] private DungeonConfigurationSO config;
    [SerializeField] private VoidEventChannel channel;
    private void Start()
    {
        setFloor();      
    }
    public void setFloor()
    {
        floor = config.currentFloor;
        updateConfig();
    }
    public void increaseFloor()
    {
        config.blockAllInput = true;
        if(floor ==config.maxFloor)
        {
            config.maxFloor++;
        }
        if(floor+1 > maxFloor)
        {
            floor = 0;
            config.isBossFloor = true;
            config.bossFloorUnlocked = true;
        }
        else
        {
            floor++;
        }
        updateConfig();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void selectConfig()
    {
        config.minimumRoom = 5;
        config.maximumRoom = 8;
        floor = config.currentFloor;
        config.enemyCountMin = (int)((float)floor * enemyGrowth + baseEnemy);
        config.enemyCountMax = (int)((float)floor * enemyGrowth + baseEnemy) + 3;
        float x = (float)floor;
        float pCommon = (maxFloor - x) / maxFloor;
        float pMedium = (x / maxFloor) * (1 - (float)eliteFactor);
        float pElite = (x / maxFloor) * (float)eliteFactor;

        float totalChance = pCommon + pMedium + pElite;

        float chanceCommon = (pCommon / totalChance) * 1000;
        float chanceMedium = (pMedium / totalChance) * 1000;
        float chanceElite = (pElite / totalChance) * 1000;

        config.commonChance = (int)chanceCommon;
        config.mediumChance = (int)chanceMedium;
        config.eliteChance = (int)chanceElite;
    }
    public void updateConfig()
    {
        config.minimumRoom = 5;
        config.maximumRoom = 8;
        config.currentFloor = floor;
        config.enemyCountMin = (int) ((float)floor * enemyGrowth + baseEnemy);
        config.enemyCountMax = (int) ((float)floor * enemyGrowth + baseEnemy)+3;
        float x = (float) floor;
        float pCommon = (maxFloor - x) / maxFloor;
        float pMedium = (x / maxFloor) * (1 - (float)eliteFactor);
        float pElite = (x / maxFloor) * (float)eliteFactor;

        float totalChance = pCommon+ pMedium + pElite;

        float chanceCommon = (pCommon / totalChance)*1000;
        float chanceMedium = (pMedium / totalChance)*1000;
        float chanceElite = (pElite / totalChance)*1000;

        config.commonChance = (int) chanceCommon;
        config.mediumChance = (int) chanceMedium;
        config.eliteChance = (int) chanceElite;
    }
}
