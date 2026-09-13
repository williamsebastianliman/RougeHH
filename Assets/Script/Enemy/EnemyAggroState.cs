using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyAggroState : EnemyState
{
    private Enemy enemy;

    private int savedX;
    private int savedY;
    private Rigidbody rb;
    
    public EnemyAggroState(EnemyController enemyController, Enemy enemy) : base(enemyController)
    {
        this.enemy = enemy;
        rb = enemy.getObject().GetComponent<Rigidbody>();
    }
    public override void EnterState()
    {
        enemyController.stateText.text = "!!";
    }
    public override void UpdateState()
    {
        enemyController.enemyMove();
        enemyController.enemyAnimation();
    }
    public override void Reset()
    {
        enemyController.enemyTurn();
    }
}
