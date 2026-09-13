using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyController enemyController) : base(enemyController)
    {
    }
    public override void EnterState()
    {
        enemyController.stateText.text = "";
    }

}
