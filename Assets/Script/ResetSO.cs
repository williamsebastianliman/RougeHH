using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSO : MonoBehaviour
{
    [SerializeField] private PlayerStatSO stat;
    [SerializeField] private DungeonConfigurationSO config;
    private void Start()
    {
        stat.init();
        config.resetSO();
    }
}
