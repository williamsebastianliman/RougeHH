using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SaveManager : MonoBehaviour
{
    private string path;

    public PlayerStatSO stat;
    public DungeonConfigurationSO config;
    public ItemSO item1;
    public ItemSO item2;
    public ItemSO item3;
    public ItemSO item4;
    public ItemSO item5;

    public bool isMainMenu;
    public bool isOverride;

    public GameObject newGameButton;
    public GameObject popUp;
    private void Start()
    {
        path = Application.persistentDataPath + "/data.ws";
        if (isMainMenu)
        {
            concreteLoad();
        }
    }

    public void SaveGame(SavedData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(path);

        formatter.Serialize(file, data);
        file.Close();
        Debug.Log("Saved File: "+file+" "+path);
    }

    public SavedData LoadGame()
    {
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);

            SavedData data = (SavedData)formatter.Deserialize(file);
            file.Close();

            return data;
        }
        return null;
    }
    public void concreteSaveButton()
    {
        List<int> levelList = new List<int>
        {
            item1.currentLevel,
            item2.currentLevel,
            item3.currentLevel,
            item4.currentLevel,
            item5.currentLevel
        };
        List<int> costList = new List<int>
        {
            item1.currentCost,
            item2.currentCost,
            item3.currentCost,
            item4.currentCost,
            item5.currentCost
        };
        SavedData data = new SavedData(stat.currentLevel, stat.currentExp, levelList,costList, config.maxFloor, stat.currentZhen, config.bossFloorUnlocked);
        SaveGame(data);
    }
    public void concreteLoad()
    {
        ButtonSelected buttonSelected = newGameButton.GetComponent<ButtonSelected>();
        SavedData data = LoadGame();
        if(data!=null)
        {
            buttonSelected.isDisabled = false;
            isOverride = true;
            stat.currentLevel = data.currentLevel;
            stat.currentExp = data.currentExp;
            config.maxFloor = data.maxFloor;
            stat.currentZhen = data.currentZhen;

            item1.currentLevel = data.levelList[0];
            item2.currentLevel = data.levelList[1];
            item3.currentLevel = data.levelList[2];
            item4.currentLevel = data.levelList[3];
            item5.currentLevel = data.levelList[4];

            item1.currentCost = data.costList[0];
            item2.currentCost = data.costList[1];
            item3.currentCost = data.costList[2];
            item4.currentCost = data.costList[3];
            item5.currentCost = data.costList[4];
            config.bossFloorUnlocked = data.isUnlocked;
        }
        else
        {
            Button button = newGameButton.GetComponent<Button>();
            buttonSelected.isDisabled = true;
            button.interactable = false;
        }
    }
    public void continueButton()
    {
        SceneManager.LoadScene(1);
    }
    public void newGameLoad()
    {
        DeleteSave();
        stat.currentLevel = 1;
        stat.currentExp = 0;
        config.maxFloor = 1;
        stat.currentZhen = 0;
        item1.currentLevel = 0;
        item2.currentLevel = 0;
        item3.currentLevel = 0;
        item4.currentLevel = 0;
        item5.currentLevel = 0;

        item1.currentCost = 10;
        item2.currentCost = 10;
        item3.currentCost = 10;
        item4.currentCost = 10;
        item5.currentCost = 10;

        config.bossFloorUnlocked = false;
        SceneManager.LoadScene(1);
    }
    public void DeleteSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    public void exitToMenu()
    {
        concreteSaveButton();
        SceneManager.LoadScene(0);
    }
    public void exitToUpgrade()
    {
        concreteSaveButton();
        SceneManager.LoadScene(1);
    }
    public void confirm()
    {
        if(isOverride)
        {
            popUp.SetActive(true);
        }
        else
        {
            newGameLoad();
        }
    }
    public void close()
    {
        popUp.SetActive(false);
    }
}
