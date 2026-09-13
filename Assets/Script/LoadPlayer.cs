using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayer : MonoBehaviour
{
    public PlayerStatSO stat;
    void Start()
    {
        stat.loadAnimator();
    }
    private void Update()
    {
        stat.checkFinishedDeath();
        stat.checkFinishedHit();
    }
}
