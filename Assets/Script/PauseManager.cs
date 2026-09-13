using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public SaveManager saveManager;
    public DungeonConfigurationSO config;
    public GameObject pauseModal;
    public bool isOpen = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isOpen)
            {
                Time.timeScale = 0;
                pauseModal.SetActive(true);
                isOpen = true;
                config.blockAllInput = true;
            }
            else
            {
                Time.timeScale = 1;
                pauseModal.SetActive(false);
                isOpen = false;
                config.blockAllInput = false;
            }
        }
    }
    public void resumeButton()
    {
        if (!isOpen)
        {
            Time.timeScale = 0;
            pauseModal.SetActive(true);
            isOpen = true;
            config.blockAllInput = true;
        }
        else
        {
            Time.timeScale = 1;
            pauseModal.SetActive(false);
            isOpen = false;
            config.blockAllInput = false;
        }
    }
    public void menuButton()
    {
        saveManager.exitToMenu();
    }
    public void upgradeButton()
    {
        saveManager.exitToUpgrade();
    }
}
