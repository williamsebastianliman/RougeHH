using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EntityInfo : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    [SerializeField] private float attack;
    [SerializeField] public float defense;
    public int critChance = 15;
    public float critMultiplier = 2;
    [SerializeField] private Image image;
    [SerializeField] private GameObject popupText;
    [SerializeField] private GameObject critText;
    [SerializeField] private DungeonConfigurationSO config;
    [SerializeField] private TextMeshPro initial;
    [SerializeField] private PlayerStatSO stat;
    public int zhenDrop;
    public int expDrop;

    private List<string> names = new List<string>()
    {
        "AC",
        "AS",
        "BD",
        "BT",
        "CT",
        "FO",
        "GN",
        "GY",
        "HO",
        "KH",
        "MM",
        "MR",
        "MV",
        "NS",
        "OV",
        "PL",
        "RU",
        "TI",
        "VD",
        "VM",
        "WS",
        "WW",
        "YD"
    };
    private bool inAnimation=false;
    private Animator anim;
    public float currentHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
        image.fillAmount = currentHealth / maxHealth;
        anim = GetComponent<Animator>();
    }
    public void playAnimation(string animationName)
    {
        anim.CrossFade(animationName, 0f);
    }
    public void checkFinishAnimation(string animationName)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(animationName))
        {
            if (stateInfo.normalizedTime >= 1f && !anim.IsInTransition(0))
            {
                inAnimation = false;
                playAnimation("Idle");
            }
        }
    }
    private float calcDamage(int damage)
    {
        float defenseFactor = 1 - (defense / (defense + config.defenseScaleFactor));
        float finalDamage =(int)( damage * defenseFactor);
        return finalDamage;
    }
    public bool takeDamage(int damage, bool isCrit)
    {
        playAnimation("GetHit");
        inAnimation = true;
        float finalDamage = calcDamage(damage);
        if(stat.isLifesteal)
        {
            stat.gainHealth((int)(finalDamage * (stat.lifestealAmount / 100)));
        }
        if(isCrit)
        {
            GameObject text = Instantiate(critText, transform.position, Quaternion.identity);
            text.GetComponent<SetText>().setText(finalDamage.ToString());
        }
        else
        {
            GameObject text = Instantiate(popupText, transform.position, Quaternion.identity);
            text.GetComponent<SetText>().setText(finalDamage.ToString());
        }
        
        currentHealth -= finalDamage;
        image.fillAmount -= finalDamage;
        image.fillAmount = currentHealth / maxHealth;
        if (currentHealth<=0)
        {
            stat.getZhen(zhenDrop);
            stat.getExp(expDrop);
            return true;
        }
        return false;
    }
    public void setText()
    {
        int idx = Random.Range(0, names.Count);
        initial.text = names[idx];
    }
    private void Update()
    {
        if(inAnimation)
        {
            checkFinishAnimation("GetHit");
        }
    }
}
