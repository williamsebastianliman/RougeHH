using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : TurnState
{
    private PlayerMovement playerMovement;
    public PlayerTurnState(TurnManager turnManager, PlayerMovement playerMovement) : base(turnManager)
    {
        this.playerMovement = playerMovement;
    }
    public override void UpdateState()
    {
        playerMovement.hoverTile();
        playerMovement.getCurrentState().UpdateState();
        playerMovement.listenSkillInput();
    }
    public override void EnterState()
    {
        playerMovement.SwitchState(new CombatState(playerMovement));
    }
    public override void ExitState()
    {
        playerMovement.clearList();
    }

}
