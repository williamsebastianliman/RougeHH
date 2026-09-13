using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTurnState : TurnState
{
    PlayerMovement playerMovement;
    public IdleTurnState(TurnManager turnManager, PlayerMovement playerMovement) : base(turnManager)
    {
        this.playerMovement = playerMovement;
    }
    public override void EnterState()
    {
        playerMovement.clearList();
        playerMovement.SwitchState(new ExploringState(playerMovement));
    }
    public override void UpdateState()
    {
        playerMovement.hoverTile();
        playerMovement.getCurrentState().UpdateState();
        playerMovement.listenSkillInput();
    }
}
