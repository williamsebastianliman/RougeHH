using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelect : MonoBehaviour
{
    public ItemSO item;
    private Sprite sprite;
    public int idx = 0;
    public AttributeFloatSO attribute;
    public AttributeFloatSO attribute2;
    public AudioManager audioManager;
    public TextMeshProUGUI levelText;
    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(displayItem);
    }
    private void displayItem()
    {
        audioManager.Play("MenuClicked");
        item.currentStat = System.Math.Round(attribute2.attr,2).ToString();
        string current = item.currentStat+" "+item.unit;
        string upgrade = item.upgradeStat.ToString() + " " + item.unit;
        ScrollManager.instance.setScroll(item.itemName, item.sprite, item.description, current, upgrade, item.currentCost.ToString());
        ScrollManager.instance.selectedItem = this;
    }

}
