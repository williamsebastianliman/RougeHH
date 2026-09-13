using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
public class ScrollManager : MonoBehaviour
{
    public AudioManager audioManager;
    public List<ItemSelect> selectList = new List<ItemSelect>();
    public List<ItemSO> itemList = new List<ItemSO>();
    public static ScrollManager instance { get; private set; }

    public PlayerStatSO stat;
    public TextMeshProUGUI zhenText;
    public TextMeshProUGUI title;
    public Image itemIcon;
    public TextMeshProUGUI description;
    public TextMeshProUGUI current;
    public TextMeshProUGUI upgrade;
    public TextMeshProUGUI upgradeCost;
    public GameObject errorText;

    public TextMeshProUGUI playerZhen;
    public GameObject master;
    public ItemSelect selectedItem = null;


    public TMP_Dropdown dropdown;
    public DungeonConfigurationSO config;

    public FloorManager floorManager;
    
    private void Start()
    {
        audioManager.Play("TavernTheme");
        setAllUpgradedValue();
        initAllLevel();
        setDropdown();
        this.zhenText.text = stat.currentZhen.ToString();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void setScroll(string title, Sprite sprite, string desc, string current, string upgrade, string cost)
    {
        master.SetActive(true);
        
        this.title.text = title;
        this.itemIcon.sprite = sprite;
        this.description.text = desc;
        this.current.text = "Current: " + current;
        this.upgrade.text = "Upgrade: " + upgrade;
        this.upgradeCost.text = cost + " to Upgrade";
        errorText.SetActive(false);
    }
    public void buyButton()
    {
        if(selectedItem.item.currentLevel >= selectedItem.item.maxLevel)
        {
            return;
        }
        if(selectedItem == null)
        {
            return;
        }
        if(stat.currentZhen - selectedItem.item.currentCost>=0)
        {
            selectedItem.attribute2.attr += selectedItem.item.upgradeStat;
            selectedItem.attribute.attr += selectedItem.item.upgradeStat;
            stat.currentZhen -= selectedItem.item.currentCost;
            playerZhen.text = stat.currentZhen.ToString();
            current.text = "Current: "+selectedItem.attribute2.attr.ToString()+" "+selectedItem.item.unit;
        }
        else
        {
            errorText.SetActive(true);
            return;
        }
        audioManager.playPurchaseSound();
        selectedItem.item.currentLevel++;
        increasePrice(selectedItem.item);
        updateLevel();
    }
    public void initAllLevel()
    {
        foreach(ItemSelect item in selectList)
        {
            item.levelText.text = "Lvl." + item.item.currentLevel + "/" + item.item.maxLevel;
        }
    }
    public void updateLevel()
    {
        selectedItem.levelText.text = "Lvl."+selectedItem.item.currentLevel+"/"+selectedItem.item.maxLevel;
    }
    public void updateZhen()
    {
        playerZhen.text = stat.currentZhen.ToString();
    }
    public void increasePrice(ItemSO item)
    {
        foreach(ItemSO cur in itemList)
        {
            if(cur == item)
            {
                cur.currentCost = cur.currentCost + 50;
                upgradeCost.text = cur.currentCost + " to Upgrade";
            }
            else
            {
                cur.currentCost += 10;
            }
        }
    }
    public void setAllUpgradedValue()
    {
        foreach(ItemSelect cur in selectList)
        {
            Debug.Log("Level: " + cur.item.currentLevel);
            float amount = cur.item.currentLevel * cur.item.upgradeStat;
            cur.attribute.attr = amount;
        }
    }
    public void playButton()
    {
        int selectedIndex = dropdown.value;
        int count = dropdown.options.Count;
        if(selectedIndex == count -1 &&config.bossFloorUnlocked)
        {
            Debug.Log("sELECT BOSS FLOOR!");
            config.isBossFloor = true;
            config.currentFloor = 0;
            SceneManager.LoadScene(2);
            return;
        }
        config.isBossFloor = false;
        config.currentFloor = selectedIndex + 1;
        floorManager.selectConfig();
        SceneManager.LoadScene(2);
    }
    public void setDropdown()
    {
        dropdown.ClearOptions();

        for (int i = 1; i <= config.maxFloor; i++)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData("Floor " + i));
        }
        if (config.bossFloorUnlocked)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData("Boss"));
        }
        dropdown.RefreshShownValue();
    }
}
