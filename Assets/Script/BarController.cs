using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BarController : MonoBehaviour
{
    [SerializeField] private Image health;
    

    [SerializeField] private Image exp;

    [SerializeField] private TextMeshProUGUI zhenText;
    [SerializeField] private TextMeshProUGUI floorText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI enemyText;
    [SerializeField] private GameObject enemyClearedModal;
    public IntegerEventChannel UpdateHpBar;
    public IntegerEventChannel UpdateExpBar;
    public IntegerEventChannel UpdateZhenEvent;
    public IntegerEventChannel updateFloor;
    public IntegerEventChannel updateLevel;
    public TwoFloatEventChannel updateHpText;
    public TwoFloatEventChannel updateExpText;
    public IntegerEventChannel updateEnemyText;
    public VoidEventChannel clearModal;

    private void UpdateHealth(float value)
    {
        health.fillAmount = value;
    }
    private void UpdateExp(float value)
    {
        exp.fillAmount = value;
    }
    private void UpdateZhen(float value)
    {
        zhenText.text = ((int)value).ToString();
    }
    private void UpdateFloor(float value)
    {
        floorText.text = "Floor: "+((int)value).ToString();
    }
    private void UpdateLevel(float value)
    {
        levelText.text ="Level " +((int)value).ToString();
    }
    
    private void UpdateHpText(float value, float value2)
    {
        hpText.text = ((int)value).ToString() + "/" + ((int)value2).ToString();
    }
    private void UpdateExpText(float value, float value2)
    {
        expText.text = ((int)value).ToString() + "/" + ((int)value2).ToString();
    }
    private void UpdateEnemyText(float value)
    {
        enemyText.text = "Enemy: " + ((int)value).ToString();
    }
    private void showModal()
    {
        enemyClearedModal.SetActive(true);
    }
    private void OnDisable()
    {
        UpdateHpBar.OnEventRaised -= UpdateHealth;
        UpdateExpBar.OnEventRaised -= UpdateExp;
        UpdateZhenEvent.OnEventRaised -= UpdateZhen;
        updateFloor.OnEventRaised -= UpdateFloor;
        updateLevel.OnEventRaised -= UpdateLevel;
        updateHpText.OnEventRaised -= UpdateHpText;
        updateExpText.OnEventRaised -= UpdateExpText;
        updateEnemyText.OnEventRaised -= UpdateEnemyText;
        clearModal.OnEventRaised -= showModal;
    }
    private void OnEnable()
    {
        UpdateHpBar.OnEventRaised += UpdateHealth;
        UpdateExpBar.OnEventRaised += UpdateExp;
        UpdateZhenEvent.OnEventRaised += UpdateZhen;
        updateFloor.OnEventRaised += UpdateFloor;
        updateLevel.OnEventRaised += UpdateLevel;
        updateHpText.OnEventRaised += UpdateHpText;
        updateExpText.OnEventRaised += UpdateExpText;
        updateEnemyText.OnEventRaised += UpdateEnemyText;
        clearModal.OnEventRaised += showModal;
    }

}
