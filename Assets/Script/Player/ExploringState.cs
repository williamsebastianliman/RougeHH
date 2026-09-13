using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploringState : PlayerState
{
    
    public ExploringState(PlayerMovement playerMovement) : base(playerMovement)
    {

    }

    public override void EnterState()
    {
        
    }
    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        playerMovement.movePlayer();
    }
}
