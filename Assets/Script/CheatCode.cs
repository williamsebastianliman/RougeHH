using UnityEngine;
using TMPro;

public class CheatCode : MonoBehaviour
{
    public TMP_InputField inputField;
    public PlayerStatSO stat;
    public DungeonConfigurationSO config;
    public AudioManager audioManager;
    private void Start()
    {
        inputField.onValueChanged.AddListener(OnTextChanged);
    }

    private void OnTextChanged(string newText)
    {
        StartCoroutine(HandleTextChanged(newText));
    }

    private System.Collections.IEnumerator HandleTextChanged(string newText)
    {
        yield return null; 
        Debug.Log("Updated Field Text: " + inputField.text);

        if (inputField.text == "hesoyam")
        {
            audioManager.Play("CheatActivated");
            inputField.text = "";
            stat.currentLevel = stat.currentLevel + 1;

        }
        else if(inputField.text == "opensesame")
        {
            audioManager.Play("CheatActivated");
            inputField.text = "";
            config.maxFloor = 100;
            config.bossFloorUnlocked = true;
            ScrollManager.instance.setDropdown();
        }
        else if(inputField.text == "tpagamegampang")
        {
            audioManager.Play("CheatActivated");
            inputField.text = "";
            stat.currentZhen = stat.currentZhen + 2000;
            ScrollManager.instance.updateZhen();
        }
    }
}
