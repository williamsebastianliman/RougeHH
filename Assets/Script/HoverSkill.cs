using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class HoverSkill : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public Skill skill;
    public GameObject panel;
    public TextMeshProUGUI text;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(skill == null)
        {
            return;
        }
        panel.SetActive(true);
        if(skill.isUnlocked())
        {
            text.text = skill.description;
        }
        else
        {
            text.text = "Unlocked At Level: " + skill.minimumLevel;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
}
