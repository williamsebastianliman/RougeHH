using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState 
{
    protected EnemyController enemyController;
    public EnemyState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void UpdateState() { }
    public virtual void Reset() { }

}
