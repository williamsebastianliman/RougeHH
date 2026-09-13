using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skill : ScriptableObject
{
    public Sprite skillIcon;
    public int skillId;
    public string skillName;
    public string description;
    public int minimumLevel;
    public bool isActive;
    public float activeTime;
    public float currentCooldown;
    public float maxCooldown;
    public float activeLeft;
    public bool isSelected = false;

    public GameObject textObj;
    public GameObject activeInstance;
    public GameObject coverObj;
    public PlayerStatSO stat;
    public void endTurn()
    {
        if(!isSelected)
        {
            if (currentCooldown - 1 <= 0)
            {
                currentCooldown = 0;
            }
            else
            {
                currentCooldown--;
            }
            if (activeLeft - 1 <= 0)
            {
                activeLeft = 0;
            }
            else
            {
                activeLeft--;
            }
        }
    }
    public bool isUnlocked()
    {
        if(stat.currentLevel<minimumLevel)
        {
            return false;
        }
        return true;
    }
    public virtual void activateSkill()
    {
        if(currentCooldown==0&&!isActive)
        {
            currentCooldown = maxCooldown;
            activeLeft = activeTime;
        }
    }
    public virtual void deactivateSkill()
    {
        Debug.Log("Test! decativated!");
        if(currentCooldown==0)
        {
            isSelected = false;
            currentCooldown = maxCooldown;
        }
    }
    public virtual void basicDeactivate()
    {

    }
}
