using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : TurnState
{
    private List<Enemy> enemyList = new List<Enemy>();
    private int currentCount;
    private int maxIndex;
    private int savedIndex;
    public EnemyTurnState(TurnManager turnManager, List<Enemy> enemyList) : base(turnManager)
    {
        this.enemyList = enemyList;
    }
    public override void EnterState()
    {
        savedIndex = 0;
        currentCount = 0;
        maxIndex = enemyList.Count -1;
        calculatePath(currentCount);
        foreach(Enemy e in enemyList)
        {
            enemyList[currentCount].getObject().GetComponent<EnemyController>().justMove = false;
        }
    }
    private void calculatePath(int idx)
    {
        enemyList[idx].getObject().GetComponent<EnemyController>().getCurrentState().Reset();
        enemyList[idx].getObject().GetComponent<EnemyController>().getCurrentState().EnterState();
    }
    public override void UpdateState()
    {
        enemyList[currentCount].getObject().GetComponent<EnemyController>().getCurrentState().UpdateState();
        if (enemyList[currentCount].getObject().GetComponent<EnemyController>().justMove)
        {
            enemyList[currentCount].getObject().GetComponent<EnemyController>().justMove = false;
            currentCount++;
        }
        if(currentCount>maxIndex)
        {
            turnManager.SwitchState(new PlayerTurnState(turnManager, turnManager.playerMovement));
        }
        else if(savedIndex!=currentCount)
        {
            savedIndex = currentCount;
            calculatePath(currentCount); 
        }
    }
}
