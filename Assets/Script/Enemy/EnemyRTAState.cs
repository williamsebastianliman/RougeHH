using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRTAState : EnemyState
{
    private Enemy enemy;
    public EnemyRTAState(EnemyController enemyController, Enemy enemy) : base(enemyController)
    {
        this.enemy = enemy;
    }
    public override void UpdateState()
    {
        enemy.getObject().GetComponent<EnemyController>().enemyAttack();
        enemy.getObject().GetComponent<EnemyController>().rotateEnemy();
        enemy.getObject().GetComponent<EnemyController>().checkFinishedAttack();
    }
    public override void EnterState()
    {
        
    }
    public override void Reset()
    {
        enemyController.clear();
    }
}
