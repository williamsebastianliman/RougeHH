using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurnState
{
    protected TurnManager turnManager;

    public TurnState(TurnManager turnManager)
    {
        this.turnManager = turnManager;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void UpdateState() { }


}
