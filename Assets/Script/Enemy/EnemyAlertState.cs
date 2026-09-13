using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertState : EnemyState
{
    private PlayerPositionSO playerSO;
    private Enemy enemy;
    public EnemyAlertState(EnemyController enemyController, Enemy enemy, PlayerPositionSO playerSO) : base(enemyController)
    {
        this.enemy = enemy;
        this.playerSO = playerSO;
    }
    public override void EnterState()
    {
        enemyController.stateText.text = "??";
    }
    public override void ExitState()
    {
        enemyController.shootRay();
    }
    public override void Reset()
    {
        enemyController.shootRay();
    }
    private float ecludian(int x, int y, int x1, int y1)
    {
        return Mathf.Sqrt(Mathf.Pow(x1 - x, 2) + Mathf.Pow(y1 - y, 2));
    }
}
