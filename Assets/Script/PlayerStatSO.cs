using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatSO", menuName = "Character/Stat")]
public class PlayerStatSO : ScriptableObject
{
    [SerializeField] private float currentHealth;

    [SerializeField] private PlayerPositionSO pos;
    [SerializeField] private DungeonConfigurationSO config;

    [SerializeField] public AttributeFloatSO maxHealth;
    [SerializeField] public AttributeFloatSO defense;
    [SerializeField] public AttributeFloatSO attack;
    [SerializeField] public AttributeFloatSO critChance;
    [SerializeField] public AttributeFloatSO critMultiplier;

    [SerializeField] public float defenseScalingFactor;
    [SerializeField] public int currentLevel;
    [SerializeField] public float maxExp;
    [SerializeField] public float currentExp;
    [SerializeField] private GameObject levelUpText;
    [SerializeField] private GameObject critText;
    [SerializeField] private GameObject lifestealText;

    public AttributeFloatSO extraHealth;
    public AttributeFloatSO extraDefense;
    public AttributeFloatSO extraAttack;
    public AttributeFloatSO extraCritChance;
    public AttributeFloatSO extraCritMultiplier;


    public int currentZhen = 0;
    [SerializeField] private GameObject textObj;
    [SerializeField] IntegerEventChannel UpdateHpBarEvent;
    [SerializeField] IntegerEventChannel UpdateExpBarEvent;
    [SerializeField] IntegerEventChannel UpdateZhenEvent;
    [SerializeField] IntegerEventChannel UpdateLevelEvent;
    [SerializeField] IntegerEventChannel UpdateFloorEvent;
    [SerializeField] TwoFloatEventChannel UpdateHealthText;
    [SerializeField] TwoFloatEventChannel UpdateExpText;

    public bool isLifesteal = false;
    public float lifestealAmount;

    public bool isBuff = false;
    public float bonusDamage;
    public float bonusDefense;

    public bool enchanceBuff=false;
    [SerializeField] private GameObject gameOverModal;

    public Animator anim;
    public void resetAllBuff()
    {
        lifestealAmount = 0;
        isLifesteal = false;
        isBuff = false;
        enchanceBuff = false;
        bonusDamage = 0;
        bonusDefense = 0;
    }
    public void loadAnimator()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        anim = playerObj.GetComponent<Animator>();
        Debug.Log("Player Animator Loaded: " + anim);
    }
    public void PlayAnimation(string animationName)
    {
        anim.CrossFade(animationName, 0f);
    }
    public void init()
    {
        resetAllBuff();
        updateAttack();
        updateDefense();
        updateMaxExp();
        updateMaxHp();
        updateCrit();
        currentHealth = maxHealth.attr;
        UpdateHpBarEvent.RaiseEvent(currentHealth/maxHealth.attr);
        UpdateExpBarEvent.RaiseEvent(currentExp/maxExp);
        UpdateZhenEvent.RaiseEvent(currentZhen);
        UpdateFloorEvent.RaiseEvent(config.currentFloor);
        UpdateLevelEvent.RaiseEvent(currentLevel);
        UpdateHealthText.RaiseEvent(currentHealth,maxHealth.attr);
        UpdateExpText.RaiseEvent(currentExp,maxExp);
    }
    private float calcDamage(int damage)
    {
        float finDef = defense.attr + defense.attr * bonusDefense;
        float defenseFactor = 1 - (defense .attr / (finDef + config.defenseScaleFactor));
        float finalDamage = (int)(damage * defenseFactor);
        return finalDamage;
    }

    public bool takeDamage(int damage, bool isCrit)
    {
        float finalDamage = calcDamage(damage);
        if(!isCrit)
        {
            GameObject text = Instantiate(textObj, new Vector3(pos.getPlayerX() * 2, 0, pos.getPlayerY() * 2), Quaternion.identity);
            text.GetComponent<SetText>().setText(finalDamage.ToString());
        }
        else
        {
            GameObject text = Instantiate(critText, new Vector3(pos.getPlayerX() * 2, 0, pos.getPlayerY() * 2), Quaternion.identity);
            text.GetComponent<SetText>().setText(finalDamage.ToString());
        }
        
        if(currentHealth-finalDamage <=0)
        {
            currentHealth = 0;
            PlayAnimation("Death");
            config.blockAllInput = true;
            UpdateHpBarEvent.RaiseEvent(currentHealth /maxHealth.attr);
            UpdateHealthText.RaiseEvent(currentHealth, maxHealth.attr);
            return true;
        }
        else
        {
            PlayAnimation("GetHit");
            currentHealth -= finalDamage;
            UpdateHpBarEvent.RaiseEvent(currentHealth /maxHealth.attr);
            UpdateHealthText.RaiseEvent(currentHealth, maxHealth.attr);
        }
        return false;
    }
    public void checkFinishedHit()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("GetHit"))
        {
            if (stateInfo.normalizedTime >= 1f && !anim.IsInTransition(0))
            {
                PlayAnimation("Idle");
            }
        }
    }
    public void checkFinishedDeath()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Death"))
        {
            if (stateInfo.normalizedTime >= 1f && !anim.IsInTransition(0))
            {
                gameOver();
            }
        }
    }
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
    public void gameOver()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        gameOverModal = FindChildByTag(canvas, "GameOver");
        gameOverModal.SetActive(true);
    }
    public void updateAttack()
    {
        attack.attr = 10 + 2 * currentLevel + currentLevel * currentLevel * 0.1f + extraAttack.attr;
        Debug.Log("Attack Attribute: "+attack);
    }
    public void updateMaxExp()
    {
        maxExp = currentLevel * 5 + currentLevel*currentLevel;
    }
    public void updateMaxHp()
    {
        maxHealth.attr = currentLevel * 15 + 5 + extraHealth.attr;
        currentHealth = maxHealth.attr;
    }
    public void updateDefense()
    {
        Debug.Log(currentLevel);
        defense.attr = currentLevel * 3 + 2 + extraDefense.attr;
        Debug.Log("Raw Lavel: "+(currentLevel * 3 + 2));
        Debug.Log("Extra: "+extraDefense.attr);
        Debug.Log("total: " + defense.attr);
    }
    public void updateCrit()
    {
        critChance.attr = (currentLevel/5) + 10 + extraCritChance.attr;
        critMultiplier.attr = currentLevel * 0.05f + 2 + extraCritMultiplier.attr;
    }
    public void checkLevelUp()
    {
        while(currentExp >= maxExp)
        {
            currentExp -= maxExp;
            currentLevel++;
            UpdateLevelEvent.RaiseEvent(currentLevel);
            Instantiate(levelUpText, new Vector3(pos.getPlayerX()*config.tileSize, 3, pos.getPlayerY()*config.tileSize), Quaternion.identity);
            updateMaxExp();
            updateMaxHp();
            updateDefense();
            updateCrit();
            updateAttack();
        }
    }
    public void gainHealth(int amount)
    {
        if (currentHealth+amount>=maxHealth.attr)
        {
            currentHealth = maxHealth.attr;
        }
        else
        {
            currentHealth += amount;
        }
        GameObject text = Instantiate(lifestealText, new Vector3(pos.getPlayerX() * 2, 0, pos.getPlayerY() * 2), Quaternion.identity);
        text.GetComponent<SetText>().setText(amount.ToString());
        UpdateHpBarEvent.RaiseEvent(currentHealth / maxHealth.attr);
        UpdateHealthText.RaiseEvent(currentHealth, maxHealth.attr);
    }
    public void getZhen(int z)
    {
        currentZhen += z;
        UpdateZhenEvent.RaiseEvent(currentZhen);
    }
    public void getExp(int e)
    {
        currentExp += e;
        checkLevelUp();
        UpdateExpBarEvent.RaiseEvent(currentExp/maxExp);
        UpdateExpText.RaiseEvent(currentExp, maxExp);
        UpdateHpBarEvent.RaiseEvent(currentHealth/maxHealth.attr);
        UpdateHealthText.RaiseEvent(currentHealth, maxHealth.attr);
    }
}
