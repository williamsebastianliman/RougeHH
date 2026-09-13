using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SkillManager : MonoBehaviour
{
    public List<Skill> skillList;
    public GameObject activeObj;
    public GameObject container;

    public PlayerStatSO stat;
    public List<Skill> currentActiveSkill;
    public AudioManager audioManager;
    GameObject FindChildByTag(GameObject parent, string tag)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject; 
            }
        }
        return null; 
    }
    public void Start()
    {
        foreach (Skill skill in skillList)
        {
            if(stat.currentLevel<skill.minimumLevel)
            {
                int id = skill.skillId;
                string tags = "Skill" + id;
                GameObject containers = GameObject.FindGameObjectWithTag(tags);
                containers.GetComponent<HoverSkill>().skill = skill;
                GameObject coverObjs = FindChildByTag(containers, "SkillCover");
                Image imageCover = coverObjs.GetComponent<Image>();
                imageCover.fillAmount = 1;
                coverObjs.SetActive(true);
                GameObject skillImg = FindChildByTag(containers, "SkillImage");
                Image icon = skillImg.GetComponent<Image>();
                GameObject textObjs = FindChildByTag(coverObjs, "CooldownText");
                skill.textObj = textObjs;
                skill.textObj.SetActive(false);
                icon.sprite = skill.skillIcon;
                continue;
            }
            skill.isSelected = false;
            skill.activeLeft = 0;
            skill.currentCooldown = 0;
            int containerId = skill.skillId;
            string tag = "Skill"+containerId;
            GameObject container = GameObject.FindGameObjectWithTag(tag);
            container.GetComponent<HoverSkill>().skill = skill;
            GameObject coverObj = FindChildByTag(container, "SkillCover");
            Image cover = coverObj.GetComponent<Image>();
            skill.coverObj = coverObj;
            cover.fillAmount = skill.currentCooldown / skill.maxCooldown;
            GameObject textObj = FindChildByTag(coverObj, "CooldownText");
            skill.textObj = textObj;
            skill.textObj.SetActive(false);
            GameObject imageObj = FindChildByTag(container, "SkillImage");
            Image image = imageObj.GetComponent<Image>();
            image.sprite = skill.skillIcon;
        }
    }
    public void updateCooldown(Skill skill)
    {
        int containerId = skill.skillId;
        string tag = "Skill" + containerId;
        GameObject container = GameObject.FindGameObjectWithTag(tag);
        GameObject coverObj = FindChildByTag(container, "SkillCover");
        Image cover = coverObj.GetComponent<Image>();
        cover.fillAmount = skill.currentCooldown / skill.maxCooldown;
        TextMeshProUGUI cooldownText = skill.textObj.GetComponent<TextMeshProUGUI>();
        if (skill.currentCooldown <= 0)
        {
            skill.textObj.SetActive(false);
        }
        else
        {
            skill.textObj.SetActive(true);
            cooldownText.text = skill.currentCooldown.ToString();
        }
    }
    public void updateActiveTime(Skill skill)
    {
        if(skill.activeInstance==null)
        {
            return;
        }
        if(skill.activeLeft<=0)
        {
            skill.deactivateSkill();
            Destroy(skill.activeInstance);
            skill.activeInstance = null;
        }
        else
        {
            GameObject textObj = FindChildByTag(skill.activeInstance, "CooldownText");
            TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
            text.text = skill.activeLeft.ToString();
        }
    }
    public void activateSkill(int idx)
    {
/*        if (idx > skillList.Count + 1)
        {
            return;
        }*/
        if (stat.currentLevel<skillList[idx-1].minimumLevel)
        {
            return;
        }

        if (!skillList[idx-1].isActive)
        {
            if (skillList[idx - 1].currentCooldown == 0 && skillList[idx - 1].activeLeft == 0)
            {
                skillList[idx - 1].activateSkill();
                updateCooldown(skillList[idx - 1]);
                if (!skillList[idx - 1].isActive)
                {
                    stat.PlayAnimation("Buff");
                    audioManager.Play("Buff");
                    GameObject activeImage = Instantiate(activeObj);
                    activeImage.transform.SetParent(container.transform);
                    skillList[idx - 1].activeInstance = activeImage;
                    Image img = activeImage.GetComponent<Image>();
                    img.sprite = skillList[idx - 1].skillIcon;
                    updateActiveTime(skillList[idx - 1]);
                }
            }
        }
        else
        {
            if (skillList[idx-1].currentCooldown>0)
            {
                return;
            }
            if (skillList[idx-1].isSelected)
            {
                currentActiveSkill.Remove(skillList[idx - 1]);
                skillList[idx - 1].isSelected = false;
                skillList[idx - 1].coverObj.SetActive(false);
                skillList[idx - 1].basicDeactivate();
            }
            else
            {
                currentActiveSkill.Add(skillList[idx - 1]);
                
                skillList[idx - 1].isSelected = true;
                skillList[idx - 1].coverObj.SetActive(true);
                skillList[idx-1].coverObj.GetComponent<Image>().fillAmount = 1;
                skillList[idx - 1].activateSkill();
            }
        }

        
    }
    public void makeAllToggleSkillCooldown()
    {
        Debug.Log("Ada!");
        foreach(Skill skill in currentActiveSkill) 
        {
            Debug.Log("ga ada!");
            skill.deactivateSkill();
        }
    }
    public void minusCooldown()
    {
        foreach (Skill skill in skillList)
        {
            if(!skill.isSelected&&stat.currentLevel>=skill.minimumLevel)
            {
                skill.endTurn();
                updateCooldown(skill);
                updateActiveTime(skill);
            }
        }
    }
}
