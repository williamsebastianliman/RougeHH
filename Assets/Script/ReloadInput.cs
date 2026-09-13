using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadInput : MonoBehaviour
{
    public DungeonConfigurationSO config;
    void Start()
    {
        StartCoroutine(waitReload());
    }

    IEnumerator waitReload()
    {
        
        yield return new WaitForSeconds(0.1f);
        config.blockAllInput = false;
    }
}
