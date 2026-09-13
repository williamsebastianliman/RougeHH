using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    void Start()
    {
        audioManager.Play("MainMenuTheme");
    }
}
